using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STA.Razor.Controls.Models {
	public class ToDo {
		public string Item { get; set; } = default!;
		public bool Complete { get; set; } = default!;
	}
}
