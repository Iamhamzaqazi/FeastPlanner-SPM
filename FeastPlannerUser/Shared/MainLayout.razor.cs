using AppDBContext.General;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace FeastPlannerUser.Shared
{
    public partial class MainLayout
    {
        #region Variables

        private MudThemeProvider _mudThemeProvider;
        private bool ThemeMode { get; set; } = false;
        private bool open = false;
        private Anchor anchor = Anchor.Left;
        private MudTheme _theme = new();

        #endregion

        #region Functions

        private void ToggleDrawer(Anchor anchor)
        {
            this.anchor = anchor;
            open = !open;
        }

        #endregion

        #region Events

        protected async override Task OnInitializedAsync()
        {
            try
            {
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        public async void Dispose()
        {
            //if (_hubConnection is not null)
            //{
            //    await _hubConnection.DisposeAsync();
            //}
            //if (_hubConnectionMessage is not null)
            //{
            //    await _hubConnectionMessage.DisposeAsync();
            //}
            // Suppress finalization.
            await Task.Delay(1);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}