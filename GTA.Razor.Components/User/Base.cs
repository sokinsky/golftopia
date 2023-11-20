using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Razor.Components.User {
    public class Base : Components.Base {
        protected async override Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            
            if (this.User == null) {
                this.NavManager.NavigateTo("/login");
            }
        }
    }
}
