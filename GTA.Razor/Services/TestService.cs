using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Razor.Services {
    public class TestService : IAsyncDisposable {
        private IJSRuntime JSRuntime { get; }
        private Lazy<Task<IJSObjectReference>> JSModule { get; }
        public TestService(IJSRuntime jsRuntime) {
            this.JSRuntime = jsRuntime;
            this.JSModule = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/GTA.Razor/Services/TestService.js").AsTask());        }

        public async ValueTask<bool> Test(string message) {
            var module = await this.JSModule.Value;
            var result = await module.InvokeAsync<bool>("test", message);
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
