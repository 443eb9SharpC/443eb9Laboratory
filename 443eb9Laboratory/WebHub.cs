using _443eb9Laboratory.Database;
using _443eb9Laboratory.Utils;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Net.Mail;

namespace _443eb9Laboratory;

public class WebHub : Hub
{
    public Random random = new Random();
    public Dictionary<string, string> verifCodes = new Dictionary<string, string>();

    public async Task Login(string username, string password, string connectionId)
    {
        if (!UserDatabase.HasUser(username))
        {
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "用户不存在");
            return;
        }
        if (UserDatabase.GetUser(username).password != MathExt.MD5Encrypt64(password))
        {
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "密码错误");
            return;
        }

        await Clients.Client(connectionId).SendAsync("loginAnim");
    }

    public async Task Register(string username, string password, string email, string verifCode, string connectionId)
    {
        if (verifCode != verifCodes[connectionId])
        {
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "验证码错误");
            return;
        }
        if (UserDatabase.HasUser(username))
        {
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "用户名已存在");
            return;
        }

        UserDatabase.AddUser(username, MathExt.MD5Encrypt64(password), email);
        await Clients.Client(connectionId).SendAsync("registerAnim");
    }

    public void SendVerifCode(string email, string connectionId)
    {
        verifCodes[connectionId] = random.Next(100000, 999999).ToString();
        MailMessage verifCode = new MailMessage()
        {
            From = new MailAddress("3166943013@qq.com", "443eb9Laboratory", System.Text.Encoding.UTF8),
            Subject = "443eb9Laboratory - 验证码",
            Body = verifCodes[connectionId],
            IsBodyHtml = false
        };
        verifCode.To.Add(email);
        SmtpClient smtpClient = new SmtpClient("smtp.qq.com", 25)
        {
            Credentials = new NetworkCredential("3166943013@qq.com", "pyvuzkzphaabdgif"),
            EnableSsl = true
        };
        smtpClient.Send(verifCode);
    }
}
