using Iot.Core.AspNetCore.Middlewares;
using Iot.Core.EventBus.Base.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ShopFComputerBackEnd.Core.Authentication.Extensions.DependencyInjection;
using ShopFComputerBackEnd.Identity.Shared.IntegrationEvents;
using ShopFComputerBackEnd.Profile.Api.Grpc;
using ShopFComputerBackEnd.Profile.Api.Policies;
using ShopFComputerBackEnd.Profile.Domain.Dtos;
using ShopFComputerBackEnd.Profile.Infrastructure.Extensions.DependencyInjection;
using ShopFComputerBackEnd.Profile.Infrastructure.Handlers.IntegrationEvents;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddGrpc();
builder.Services.AddControllers().AddOData(opt =>
{
    opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5000)
    .AddRouteComponents("odata", GetEdmModel());
    opt.TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
        options.AddPolicy(
            "AllowInternal",
            policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowedToAllowWildcardSubdomains()
        ));
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(ProfilePolicies.Create, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == ProfilePolicies.FullAccess || c.Value == ProfilePolicies.Create)));
    options.AddPolicy(ProfilePolicies.View, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == ProfilePolicies.FullAccess || c.Value == ProfilePolicies.View)));
    options.AddPolicy(ProfilePolicies.Update, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == ProfilePolicies.FullAccess || c.Value == ProfilePolicies.Update)));
    options.AddPolicy(ProfilePolicies.Delete, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == ProfilePolicies.FullAccess || c.Value == ProfilePolicies.Delete)));
});


IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();

    odataBuilder.EntityType<ProfileDto>().HasKey(entity => entity.Id);
    odataBuilder.EntitySet<ProfileDto>("Profiles");
    odataBuilder.EntitySet<ProfileDto>("Me");

    var searchProfileToAddGenealogyAction = odataBuilder.EntityType<ProfileDto>().Function("SearchProfileToAddGenealogy").ReturnsFromEntitySet<ProfileDto>("Public");

    var getProfileDetailFunction = odataBuilder.EntitySet<ProfileDto>("Me").EntityType.Function("GetProfileDetail").ReturnsFromEntitySet<ProfileDto>("Me").Parameter(typeof(Guid), "destinationProfileId");

    return odataBuilder.GetEdmModel();
}
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

app.UseRouting();
app.UseCors("AllowInternal");
app.UseHealthChecks("/health");
app.UseMiddleware<ExceptionMiddleware>(true);
app.UseAuthentication();
app.UseAuthorization();
app.UseGrpcWeb();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGrpcService<ProfileGrpc>().EnableGrpcWeb().RequireCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowedToAllowWildcardSubdomains());
});
app.UsePolicies();

ConfigureEventBus(app);

app.Run();

void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<UserSignUpIntegrationEvent, UserIntegrationEventHandler>();

}