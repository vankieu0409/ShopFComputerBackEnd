using Iot.Core.AspNetCore.Extensions;
using Iot.Core.AspNetCore.Http;
using Iot.Core.AspNetCore.Middlewares;
using Iot.Core.EventBus.Base.Abstractions;
using Iot.Core.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ShopFComputerBackEnd.Core.Authentication.Extensions.DependencyInjection;
using ShopFComputerBackEnd.Identity.Shared.IntegrationEvents;
using ShopFComputerBackEnd.Notification.Api.Grpc;
using ShopFComputerBackEnd.Notification.Api.Policies;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Infrastructure.Extensions.DependencyInjection;
using ShopFComputerBackEnd.Notification.Infrastructure.Handlers.IntegrationEvents;
using ShopFComputerBackEnd.Notification.Shared.IntegrationEvents;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddControllers().AddOData(opt =>
{
    opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5000)
    .AddRouteComponents("odata", GetEdmModel());
    opt.TimeZone = TimeZoneInfo.Utc;
}).AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddCors(options =>
        options.AddPolicy(
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
    options.AddPolicy(HistoriesPolicies.Create, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == HistoriesPolicies.FullAccess || c.Value == HistoriesPolicies.Create)));
    options.AddPolicy(HistoriesPolicies.View, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == HistoriesPolicies.FullAccess || c.Value == HistoriesPolicies.View)));
    options.AddPolicy(HistoriesPolicies.Update, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == HistoriesPolicies.FullAccess || c.Value == HistoriesPolicies.Update)));
    options.AddPolicy(HistoriesPolicies.Delete, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == HistoriesPolicies.FullAccess || c.Value == HistoriesPolicies.Delete)));

    options.AddPolicy(NotificationsPolicies.Create, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == NotificationsPolicies.FullAccess || c.Value == NotificationsPolicies.Create)));
    options.AddPolicy(NotificationsPolicies.View, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == NotificationsPolicies.FullAccess || c.Value == NotificationsPolicies.View)));
    options.AddPolicy(NotificationsPolicies.Update, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == NotificationsPolicies.FullAccess || c.Value == NotificationsPolicies.Update)));
    options.AddPolicy(NotificationsPolicies.Delete, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Value == NotificationsPolicies.FullAccess || c.Value == NotificationsPolicies.Delete)));
});
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddGrpc();
//builder.Services.AddGrpcReflection();
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
    endpoints.MapGrpcService<NotificationGrpc>().EnableGrpcWeb().RequireCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowedToAllowWildcardSubdomains());
});

ConfigureEventBus(app);

app.Run();

IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();

    odataBuilder.EntityType<NotificationDto>().HasKey(entity => new { entity.Id });
    odataBuilder.EntitySet<NotificationDto>("Notifications");

    odataBuilder.EntityType<HistoryNotificationDtoBase>().HasKey(entity => new { entity.Id });
    odataBuilder.EntitySet<HistoryNotificationDtoBase>("NotificationHistories");
    odataBuilder.ComplexType<PayloadDto>();
    odataBuilder.ComplexType<ProfileDto>();
    odataBuilder.ComplexType<TimelineDto>();
    odataBuilder.ComplexType<CommentDto>();

    odataBuilder.EntityType<NotificationTemplateDto>().HasKey(entity => new { entity.Id, entity.LanguageCode, entity.NotificationId });
    odataBuilder.EntitySet<NotificationTemplateDto>("NotificationTemplates");

    odataBuilder.EntityType<HistoryDto>().HasKey(entity => new { entity.Id });
    odataBuilder.EntitySet<HistoryDto>("Histories");

    var recoverNotificationAction = odataBuilder.EntityType<NotificationDto>().Action("Recover").ReturnsFromEntitySet<NotificationDto>("Notifications");
    var updateTemplateAction = odataBuilder.EntityType<NotificationDto>().Action("UpdateTemplate").ReturnsFromEntitySet<NotificationDto>("Notifications");
    var removeTemplateAction = odataBuilder.EntityType<NotificationDto>().Action("RemoveTemplate").ReturnsFromEntitySet<NotificationDto>("Notifications");

    var getMobileNotificationFunction = odataBuilder.EntityType<HistoryNotificationDtoBase>().Function("GetMobileNotification").ReturnsFromEntitySet<HistoryNotificationDtoBase>("NotificationHistories");
    return odataBuilder.GetEdmModel();
}

void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<UserUpdateDeviceIntegrationEvent, DeviceIntegrationEventHandler>();
    eventBus.Subscribe<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();
}
