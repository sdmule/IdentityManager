
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Resend;

//namespace IdentityManager.Services
//{
//    public class EmailSender : IEmailSender
//    {
//        public string ResendKey { get; set; }
//        public EmailSender(IConfiguration _congfig)
//        {
//            ResendKey = _congfig.GetValue<string>("Resend:ApiKey");
//        }
//        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
//        {

//            var message = new EmailMessage();

//            message.From = "Identity Manager <onboarding@resend.dev>";

//            message.To.Add("mulesaurabh45@gmail.com");

//            message.Subject = "Reset Password";

//            message.HtmlBody = htmlMessage;

//            await _resend.EmailSendAsync(message);
//        }
//    }
//}

using Microsoft.AspNetCore.Identity.UI.Services;
using Resend;

namespace IdentityManager.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IResend _resend;

        public EmailSender(IResend resend)
        {
            _resend = resend;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new EmailMessage();

            message.From = "Identity Manager <onboarding@resend.dev>";

            message.To.Add(email);

            message.Subject = subject;

            message.HtmlBody = htmlMessage;

            await _resend.EmailSendAsync(message);
        }
    }
}