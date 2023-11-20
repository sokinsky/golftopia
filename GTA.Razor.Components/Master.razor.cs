using GTA.Razor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Razor.Components {
    public class MasterBase : LayoutComponentBase {
        [Inject] public ExampleJsInterop ExampleJsInterop { get; set; } = default!;
        [Inject] public TestService TestService { get; set; } = default!;
        [Inject] public FacebookService FacebookService { get; set; } = default!;
        [Inject] public Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; } = default!;
        [Inject] public Client.Context Context { get; set; } = default!;
        protected override async Task OnInitializedAsync() {
            var localLogin = await this.LocalStorage.GetItemAsync<string>("login");
            if (localLogin != null) {
                this.Context.Login = await this.Context.Logins.ByToken(localLogin);
                if (this.Context.Login != null) {
                    var user = await this.Context.Users.FindAsync(this.Context.Login.ID);
                    if (user != null) {
                        this.Context.Login.User = user;
                        var person = await this.Context.People.FindAsync(this.Context.Login.User.ID);
                        if (person != null) {
                            this.Context.Login.User.Person = person;
                        }
                    }
                }
            }

            var result = await this.ExampleJsInterop.Log("Worked");
            var result1 = await this.TestService.Test("Hello World");
            var result3 = await this.FacebookService.checkLoginState();

            //await this.ExampleJsInterop.Alert(result);
            //await this.ExampleJsInterop.Log(result);
        }
    }
}
