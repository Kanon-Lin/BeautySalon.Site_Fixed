using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BeautySalon.FrontEnd.Site.Models.Services
{
    public class EmailService
    {
        public async Task SendVerificationEmail(string recipientEmail, string confirmationLink)
        {
            try
            {
                var fromAddress = new MailAddress("kanonlin2024@gmail.com", "BeautySalon");
                var toAddress = new MailAddress(recipientEmail);
                const string fromPassword = "lrrwzitupmknmhnk";
                const string subject = "請確認您的電子郵件地址";
                // 使用 HTML 格式編寫郵件內容
                string body = $@"
            <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333333;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                            background-color: #f7f7f7;
                        }}
                        h1 {{
                            color: #4a4a4a;
                            text-align: center;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            margin: 20px 0;
                            background-color: #BCED91;
                            color: 333333;
                            text-decoration: none;
                            border-radius: 5px;
                            text-align: center;
                        }}
                        .button:hover {{
                            background-color: #A9DF86;
                        }}
                        .footer {{
                            margin-top: 20px;
                            text-align: center;
                            font-size: 0.8em;
                            color: #888888;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>BeautySalon 帳號驗證</h1>
                        <p>親愛的用戶：</p>
                        <p>感謝您註冊 BeautySalon。請點擊下方按鈕以驗證您的電子郵件地址：</p>
                        <a href='{confirmationLink}' class='button'>驗證您的帳號</a>
                        <p>如果按鈕無法顯示，請複製此連結到瀏覽器：<br />{confirmationLink}</p>
                        <p>謝謝您的配合！</p>
                        <div class='footer'>
                            <p>此郵件由系統自動發送，請勿直接回覆。</p>
                            <p>&copy; 2024 BeautySalon. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";



				var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                // 處理錯誤
                Console.WriteLine($"發送郵件失敗: {ex.Message}");
            }
        }
    }
}