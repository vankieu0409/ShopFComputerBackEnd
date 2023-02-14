using Iot.Core.AspNetCore.Extensions;
using Iot.Core.AspNetCore.Http;
using Iot.Core.AspNetCore.Middlewares;
using Iot.Core.EventBus.Base.Abstractions;
using Iot.Core.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ShopFComputerBackEnd.Core.Authentication.Extensions.DependencyInjection;
using ShopFComputerBackEnd.Core.Authentication.Shared.IntegrationEvents;
using ShopFComputerBackEnd.Identity.Api;
using ShopFComputerBackEnd.Identity.Api.Grpc;
using ShopFComputerBackEnd.Identity.Api.Policies;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;
using ShopFComputerBackEnd.Identity.Infrastructure.Extensions.DependencyInjection;
using ShopFComputerBackEnd.Identity.Infrastructure.Handlers.IntegrationEvents;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddControllers().AddOData(opt =>
{
    opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5000)
    .AddRouteComponents("odata", GetEdmModel());
    opt.TimeZone = TimeZoneInfo.Utc;
}).AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddCors(options => options.AddPolicy(
    "AllowInternal",
    policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowedToAllowWildcardSubdomains()
    ));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
        RequireExpirationTime = false
    };
    options.Events = new JwtBearerEvents()
    {
        OnForbidden = ctx =>
        {
            var message = SingleResultMessage.Fail(new ForbiddenException());
            var result = JsonSerializer.Serialize(message, message.GetType(), new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles });
            return ctx.Response.WriteAsync(result);
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RolePolicies.View, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == RolePolicies.FullAccess || c.Value == RolePolicies.View)));
    options.AddPolicy(RolePolicies.Create, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == RolePolicies.FullAccess || c.Value == RolePolicies.Create)));
    options.AddPolicy(RolePolicies.Update, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == RolePolicies.FullAccess || c.Value == RolePolicies.Update)));
    options.AddPolicy(RolePolicies.Delete, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == RolePolicies.FullAccess || c.Value == RolePolicies.Delete)));
    options.AddPolicy(RolePolicies.AssignUsersToRole, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == RolePolicies.FullAccess || c.Value == RolePolicies.AssignUsersToRole)));

    options.AddPolicy(PermissionPolicies.View, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == PermissionPolicies.FullAccess || c.Value == PermissionPolicies.View)));
    options.AddPolicy(PermissionPolicies.Create, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == PermissionPolicies.FullAccess || c.Value == PermissionPolicies.Create)));
    options.AddPolicy(PermissionPolicies.Delete, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == PermissionPolicies.FullAccess || c.Value == PermissionPolicies.Delete)));
    options.AddPolicy(PermissionPolicies.AssignUsersToRole, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == PermissionPolicies.FullAccess || c.Value == PermissionPolicies.AssignUsersToRole)));

    options.AddPolicy(UserPolicies.View, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == UserPolicies.FullAccess || c.Value == UserPolicies.View)));
    options.AddPolicy(UserPolicies.Create, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == UserPolicies.FullAccess || c.Value == UserPolicies.Create)));
    options.AddPolicy(UserPolicies.Update, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == UserPolicies.FullAccess || c.Value == UserPolicies.Update)));
    options.AddPolicy(UserPolicies.Delete, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == UserPolicies.FullAccess || c.Value == UserPolicies.Delete)));
    options.AddPolicy(UserPolicies.AssignRolesToUser, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == UserPolicies.FullAccess || c.Value == UserPolicies.AssignRolesToUser)));

    options.AddPolicy(FunctionPolices.View, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == FunctionPolices.FullAccess || c.Value == FunctionPolices.View)));
    options.AddPolicy(FunctionPolices.Create, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == FunctionPolices.FullAccess || c.Value == FunctionPolices.Create)));
    options.AddPolicy(FunctionPolices.Update, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == FunctionPolices.FullAccess || c.Value == FunctionPolices.Update)));
    options.AddPolicy(FunctionPolices.Delete, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == FunctionPolices.FullAccess || c.Value == FunctionPolices.Delete)));

});
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
});
// Use odata route debug, /$odata
app.UseODataRouteDebug();
// Add OData /$query middleware
app.UseODataQueryRequest();
// Add the OData Batch middleware to support OData $Batch
app.UseODataBatching();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseGrpcWeb();
app.UseExceptionFormatter();
app.UseCors("AllowInternal");
app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UsePolicies();
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>(true);
app.MapGrpcService<FunctionGrpc>().EnableGrpcWeb();

ConfigureEventBus(app);

app.SeedAsync().ConfigureAwait(false).GetAwaiter();

app.Run();


IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();

    odataBuilder.EntityType<UserDto>().HasKey(entity => new { entity.Id });
    odataBuilder.EntitySet<UserDto>("Identity");

    odataBuilder.EntityType<RoleDto>().HasKey(entity => new { entity.Id });
    odataBuilder.EntitySet<RoleDto>("Roles");

    odataBuilder.EntityType<FunctionDto>().HasKey(entity => new { entity.Id });
    odataBuilder.EntitySet<FunctionDto>("Functions");

    odataBuilder.EntityType<PermissionDto>().HasKey(entity => new { entity.Type, entity.TypeId, entity.FunctionId });
    odataBuilder.EntitySet<PermissionDto>("Permissions");

    odataBuilder.EntityType<UserDto>().HasKey(entity => new { entity.Id });
    odataBuilder.EntitySet<UserDto>("Users");

    var removePermissionAction = odataBuilder.EntityType<FunctionDto>().Action("RemovePermission").ReturnsFromEntitySet<FunctionDto>("Functions");

    return odataBuilder.GetEdmModel();
}

void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<PoliciesIntegrationEvent, PoliciesIntegrationEventHandler>();
}