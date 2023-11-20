using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.Smtp {
    public class Gmail {
        public class Configuration {
            public string Host { get; set; } = default!;
            public int Port { get; set; }
            public string Username { get; set; } = default!;
            public string Password { get; set; } = default!;

            public static Configuration Production => new Configuration {
                Host = "smtp.gmail.com",
                Port = 587,
                Username = "sokinsky@gmail.com",
                Password = "Musk4Rat!"
            };
            public static Configuration Default => Configuration.Production;
        }
        public static void Send(MailMessage mailMessage) {
            using var client = new SmtpClient(Configuration.Default.Host, Configuration.Default.Port);
            client.Credentials = new NetworkCredential(Configuration.Default.Username, Configuration.Default.Password);
            client.EnableSsl = true;
            client.Send(mailMessage);
        }
    }
}
