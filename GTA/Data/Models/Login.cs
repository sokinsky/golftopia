using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class Login : Model {
        public int UserID { get; set; }
        public virtual User User { get; set; } = default!;
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime? Expired { get; set; }
    }
}
