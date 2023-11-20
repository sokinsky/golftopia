using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Razor.Services {
    public class FacebookService : IAsyncDisposable {
        private IJSRuntime JSRuntime { get; }
        private Lazy<Task<IJSObjectReference>> JSModule { get; }
        public FacebookService(IJSRuntime jsRuntime) {
            this.JSRuntime = jsRuntime;
            this.JSModule = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/GTA.Razor/Services/FacebookService.js").AsTask());
        }

        public async ValueTask<string> checkLoginState() {
            var module = await this.JSModule.Value;
            var result = await module.InvokeAsync<string>("checkLoginState");
            return result;
        }
        public async ValueTask DisposeAsync() {
            if (JSModule.IsValueCreated) {
                var module = await JSModule.Value;
                await module.DisposeAsync();
            }
        }
    }
}
