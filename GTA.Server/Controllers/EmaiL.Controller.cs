using GTA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Server.Controllers {
    public class EmailController : Controller<Email> {
        public EmailController(Context context, Email email) : base(context, email) { }
        public void Send(MailMessage mailMessage) {
        }
    }
}
