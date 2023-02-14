using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Iot.Core.AspNetCore.Extensions;
using Iot.Core.AspNetCore.Http;
using Iot.Core.AspNetCore.Middlewares;
using Iot.Core.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ShopFComputerBackEnd.Cart.Api.Policies;
using ShopFComputerBackEnd.Cart.Domain.Dtos;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;
using ShopFComputerBackEnd.Cart.Infrastructure.Extensions.DependencyInjection;
using ShopFComputerBackEnd.Core.Authentication.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddOData(opt =>
{
    opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5000).AddRouteComponents("odata",GetEdmModel());
    opt.EnableQueryFeatures();
    opt.TimeZone=TimeZoneInfo.Utc;
}).AddJsonOptions(opts=>opts.JsonSerializerOptions.PropertyNamingPolicy=null);
builder.Services.AddCors(options => options.AddPolicy(
    "AllowInternal",
    policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowedToAllowWildcardSubdomains()
    ));

builder.Services.AddApplication(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
    options.AddPolicy(CartPolicies.View, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == CartPolicies.FullAccess || c.Value == CartPolicies.View)));
    options.AddPolicy(CartPolicies.Create, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == CartPolicies.FullAccess || c.Value == CartPolicies.Create)));
    options.AddPolicy(CartPolicies.Update, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == CartPolicies.FullAccess || c.Value == CartPolicies.Update)));
    options.AddPolicy(CartPolicies.Delete, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == CartPolicies.FullAccess || c.Value == CartPolicies.Delete)));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
}

// Configure the HTTP request pipeline.
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
app.UseHttpsRedirection();
app.UsePolicies();
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>(true);

app.Run();

IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();

    odataBuilder.ComplexType<CartItemValueObject>();

    odataBuilder.EntityType<CartDto>().HasKey(entity => entity.Id);
    odataBuilder.EntitySet<CartDto>("Cart");

    return odataBuilder.GetEdmModel();
}
