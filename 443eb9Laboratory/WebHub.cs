using _443eb9Laboratory.Database;
using _443eb9Laboratory.DataModels.ETCC;
using _443eb9Laboratory.Utils;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Net.Mail;

namespace _443eb9Laboratory;

public class WebHub : Hub
{
    public Random random = new Random();
    public static Dictionary<string, string> verifCodes = new Dictionary<string, string>();

    public override Task OnConnectedAsync()
    {
        Clients.Caller.SendAsync("createMessage", "连接成功", "成功连接到服务端");
        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task Login(string username, string password, string ipAddress, string connectionId)
    {
        if (!UserDatabase.HasUser(username))
        {
            Console.WriteLine(username);
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "用户不存在");
            return;
        }
        if (UserDatabase.GetUser(username).password != MathExt.MD5Encrypt64(password))
        {
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "密码错误");
            return;
        }

        string page;
        if (ChamberDatabase.IsUserOwnedChamber(username))
        {
            page = "../Pages/ETCC.html";
        }
        else
        {
            page = "../Pages/ETCC-New.html";
        }

        Console.WriteLine($"[{DateTime.Now}][{ipAddress}][{username}] Logged in");
        ClientDatabase.AddKeyValuePair(ipAddress, username);
        await Clients.Client(connectionId).SendAsync("loginAnim", page);
    }

    public async Task Register(string username, string password, string email, string verifCode, string connectionId)
    {
        if (!verifCodes.ContainsKey(connectionId) || verifCode != verifCodes[connectionId])
        {
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "验证码错误");
            return;
        }
        if (UserDatabase.HasUser(username))
        {
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "用户已存在");
            return;
        }

        UserDatabase.AddUser(username, MathExt.MD5Encrypt64(password), email);
        UserDatabase.SaveDatabase();
        await Clients.Client(connectionId).SendAsync("registerAnim");
    }

    public async Task NewChamber(string chamberName, string chamberDescription, string connectionId, string ipAddress)
    {
        if (chamberDescription.Length > 22)
        {
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "培养仓描述超过22个字");
            return;
        }
        if (chamberName.Length > 6)
        {
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "培养仓名字超过6个字");
            return;
        }

        ChamberDatabase.AddChamber(chamberName, chamberDescription, ipAddress);
        await Clients.Client(connectionId).SendAsync("onChamberCreated");
    }

    public async Task SendVerifCode(string email, string connectionId)
    {
        verifCodes[connectionId] = random.Next(100000, 999999).ToString();
        MailMessage verifCode = new MailMessage()
        {
            From = new MailAddress("3166943013@qq.com", "443eb9Laboratory", System.Text.Encoding.UTF8),
            Subject = "443eb9Laboratory - 验证码",
            Body = verifCodes[connectionId],
            IsBodyHtml = false
        };
        try
        {
            verifCode.To.Add(email);
        }
        catch
        {
            await Clients.Client(connectionId).SendAsync("createErrorMessage", "无效参数", "邮箱格式错误");
            return;
        }
        SmtpClient smtpClient = new SmtpClient("smtp.qq.com", 25)
        {
            Credentials = new NetworkCredential("3166943013@qq.com", "pyvuzkzphaabdgif"),
            EnableSsl = true
        };
        smtpClient.Send(verifCode);
    }

    public void ETCC_SendInformation(InformationType informationType, string connectionId, string ipAddress)
    {
        switch (informationType)
        {
            case InformationType.ETCC_DashBoard:
                ETCC_InformationIndexer.SendDashBoardInfo(connectionId, ipAddress, Clients.Client(connectionId));
                break;
            case InformationType.ETCC_Chunks:
                ETCC_InformationIndexer.SendChunksInfo(connectionId, ipAddress, Clients.Client(connectionId));
                break;
        }
    }
}
