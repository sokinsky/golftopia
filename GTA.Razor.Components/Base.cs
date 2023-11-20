using GTA.Client.Controllers;
using GTA.Client.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GTA.Razor.Components {
    public class Base : ComponentBase {
        [Inject] public NavigationManager NavManager { get; set; } = default!;
        [Inject] public Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; } = default!;
        [Inject] public Client.Context Context { get; set; } = default!;
        public UserController? User => this.Context.User;


        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();


        }
    }
}
