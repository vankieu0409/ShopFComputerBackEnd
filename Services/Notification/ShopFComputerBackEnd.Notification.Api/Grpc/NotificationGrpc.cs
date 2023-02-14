using AutoMapper;
using Grpc.Core;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using ShopFComputerBackEnd.Notification.Infrastructure.Commands.Histories;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.Notifications;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.NotificationTemplates;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Api.Grpc
{
    public class NotificationGrpc : NotificationTemplateGrpcService.NotificationTemplateGrpcServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public NotificationGrpc(IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public override async Task<NotificationGrpcDto> SendNotification(NotificationGrpcRequest request, ServerCallContext context)
        {
            var result = new NotificationGrpcDto();
            var notificationQuery = new GetNotificationDetailByContextAndNameAndTypeQuery(request.Context, request.Name, _mapper.Map<NotificationType>(request.Type));
            var notification = await _mediator.Send(notificationQuery);
            if (notification.IsNullOrDefault())
                throw new ArgumentNullException("notifications");
            Dictionary<string, string> lstVariables = new Dictionary<string, string>();
            if (NotificationType.Email.Equals(notification.Type))
            {
                var emailClient = request.Variables.FirstOrDefault(variable => string.Equals(variable.Variable, "Email")).Value;
                var templateQuery = new GetNotificationTemplateDetailByLanguageCodeAndNotificationIdQuery(request.LanguageCode, notification.Id);
                var template = await _mediator.Send(templateQuery);
                if (template.IsNullOrDefault())
                    throw new ArgumentNullException("template not found!");
                foreach (var variable in request.Variables)
                {
                    lstVariables.Add(variable.Variable, variable.Value);
                }
                string content = lstVariables.Aggregate(template.Content, (args, pair) => args.Replace($"{{{pair.Key}}}", pair.Value));
                var message = new NotificationRespone()
                {
                    Respone = content
                };
                var hostMail = _configuration["MailSmtp:Mail"];
                var hostMailPassword = _configuration["MailSmtp:Password"];
                var stmpResult = await SendMailGoogleSmtp(hostMail, emailClient, template.Subject, message.Respone, template.Attachments, hostMail, hostMailPassword);
                result = _mapper.Map<NotificationGrpcDto>(notification);

                var id = Guid.NewGuid();
                var rawData = _mapper.Map<NotificationBuiltValueObject>(request);
                if (bool.Equals(stmpResult, true))
                {
                    var command = new CreateHistoryCommand(id, template.Id, content, notification.Type, JsonSerializer.Serialize(_configuration["MailSmtp"]), Domain.Enums.NotificationStatus.Success, "Success", JsonSerializer.Serialize(emailClient), rawData);
                    await _mediator.Send(command);
                }
                else
                {
                    var command = new CreateHistoryCommand(id, template.Id, content, notification.Type, JsonSerializer.Serialize(_configuration["MailSmtp"]), Domain.Enums.NotificationStatus.Fail, "Fail", JsonSerializer.Serialize(emailClient), rawData);
                    await _mediator.Send(command);
                }
            }
            if (NotificationType.Sms.Equals(notification.Type))
            {
                var smsClient = request.Variables.FirstOrDefault(variable => string.Equals(variable.Variable, "PhoneNumber")).Value;
                var otp = request.Variables.FirstOrDefault(variable => string.Equals(variable.Variable, "Otp")).Value;
                var templateQuery = new GetNotificationTemplateDetailByLanguageCodeAndNotificationIdQuery(request.LanguageCode, notification.Id);
                var template = await _mediator.Send(templateQuery);
                foreach (var variable in request.Variables)
                {
                    lstVariables.Add(variable.Variable, variable.Value);
                }
                string content = lstVariables.Aggregate(template.Content, (args, pair) => args.Replace($"{{{pair.Key}}}", pair.Value));
                var message = new NotificationRespone()
                {
                    Respone = content
                };
                string URL = $"http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_get?Phone={smsClient}&Content={message.Respone}&ApiKey={_configuration["SmsOtp:ApiKey"]}&SecretKey={_configuration["SmsOtp:SecretKey"]}&SmsType=2&Brandname=Baotrixemay";
                var smsResult = SendGetRequest(URL);
                JObject ojb = JObject.Parse(smsResult.Result);
                int CodeResult = (int)ojb["CodeResult"];
                var id = Guid.NewGuid();
                var rawData = _mapper.Map<NotificationBuiltValueObject>(request);
                if (!int.Equals(CodeResult, 100))
                {
                    var command = new CreateHistoryCommand(id, template.Id, content, notification.Type, JsonSerializer.Serialize(_configuration["SmsOtp"]), Domain.Enums.NotificationStatus.Fail, "Fail", JsonSerializer.Serialize(smsClient), rawData);
                    await _mediator.Send(command);
                }
                else
                {
                    var command = new CreateHistoryCommand(id, template.Id, content, notification.Type, JsonSerializer.Serialize(_configuration["SmsOtp"]), Domain.Enums.NotificationStatus.Success, "Success", JsonSerializer.Serialize(smsClient), rawData);
                    await _mediator.Send(command);
                }
                Console.WriteLine(message.Respone);
            }
            if (NotificationType.Mobile.Equals(notification.Type))
            {
                var templateQuery = new GetNotificationTemplateDetailByLanguageCodeAndNotificationIdQuery(request.LanguageCode, notification.Id);
                var template = await _mediator.Send(templateQuery);
                foreach (var variable in request.Variables)
                {
                    lstVariables.Add(variable.Variable, variable.Value);
                }
                string content = lstVariables.Aggregate(template.Content, (args, pair) => args.Replace($"{{{pair.Key}}}", pair.Value));
                var message = new NotificationRespone()
                {
                    Respone = content
                };
                Console.WriteLine(message.Respone);
            }

            return result;
        }
        public static async Task<bool> SendMailGoogleSmtp(string emailSend, string emailReceive, string subject,
                                                    string body, IEnumerable<NotificationTemplateAttachmentValueObject> attachments, string emailAccount, string password)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailSend);
            mail.To.Add(emailReceive);
            mail.Subject = subject;
            mail.Body = body;

            if (!attachments.IsNullOrDefault())
            {
                foreach (var item in attachments)
                {
                    mail.Attachments.Add(new Attachment(item.FileName, item.ContentType));
                }
            }

            using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;
                client.Credentials = new NetworkCredential(emailAccount, password);
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                try
                {
                    await client.SendMailAsync(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        private async Task<string> SendGetRequest(string RequestUrl)
        {
            Uri address = new Uri(RequestUrl);
            var httpClient = new HttpClient();
            if (address.IsNullOrDefault())
                throw new ArgumentNullException("address");
            try
            {

                httpClient.BaseAddress = address;
                var response = await httpClient.GetAsync(RequestUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        Console.WriteLine(
                            "The server returned '{0}' with the status code {1} ({2:d}).",
                            errorResponse.StatusDescription, errorResponse.StatusCode,
                            errorResponse.StatusCode);
                    }
                }
            }
            return null;
        }
    }
}
