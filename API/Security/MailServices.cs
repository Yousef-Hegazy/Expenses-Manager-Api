using API.Models.Enums;
using MailKit.Net.Smtp;
using Microsoft.OpenApi.Extensions;
using MimeKit;

namespace API.Security;

public class MailServices
{
    public static async Task<bool> SendEmail(string email, string name, string link,
        EmailType type = EmailType.ConfirmEmail)
    {
        try
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(name: "Expenses Manager", address: "yousefashraf14725@gmail.com"));

            message.To.Add(new MailboxAddress(name: name, address: email));

            message.Subject = type switch
            {
                EmailType.ConfirmEmail => "Confirm Email Address",
                EmailType.ChangePassword => "Reset Your Password",
                _ => "Expense Manager"
            };

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var directory = Directory.GetCurrentDirectory();

            var fileName = type.GetDisplayName();

            // var path = env == "Development"
            //     ? Path.Combine(directory, "Static", $"{fileName}.html")
            //     : Path.Combine(directory, "API", "Static", $"{fileName}.html");

            var path = Path.Combine(directory, "Static", $"{fileName}.html");
            
            var htmlText = await File.ReadAllTextAsync(path);

            htmlText = htmlText.Replace("{name}", name);

            htmlText = htmlText.Replace("{link}", link);

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlText,
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync("smtp.gmail.com", 465, true);

            await client.AuthenticateAsync(userName: "yousefashraf14725@gmail.com", password: "tvkxdyblzoblfbxq");

            await client.SendAsync(message);

            await client.DisconnectAsync(true);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}