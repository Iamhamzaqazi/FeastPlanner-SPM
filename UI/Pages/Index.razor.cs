namespace UI.Pages
{
    public partial class Index
    {
        #region Variables

        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.False, CloseButton = true, DisableBackdropClick = true };
        private bool loading = false;
        private int BusinessID = 0;
        private int UserID = 0;
        private string LoggedInUser = "";

        // Labels for X axis
        public string[] XAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep" };

        #endregion

        #region Functions

        // Bar chart series (multiple series)
        public List<ChartSeries> Series = new List<ChartSeries>()
    {
        new ChartSeries() { Name = "Karachi", Data = new double[] { 40, 20, 25, 27, 46, 60, 48, 80, 15 } },
        new ChartSeries() { Name = "Lahore", Data = new double[] { 19, 24, 35, 13, 28, 15, 13, 16, 31 } },
        new ChartSeries() { Name = "Islamabad", Data = new double[] { 8, 6, 11, 13, 4, 16, 10, 16, 18 } },
    };

        // Line chart example (single series)
        public List<ChartSeries> SeriesLine = new List<ChartSeries>()
    {
        new ChartSeries() { Name = "Bookings", Data = new double[] { 5, 12, 9, 15, 20, 18, 25, 30, 28 } }
    };
        private async Task CallAPI()
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

        private async Task Refresh()
        {
            try
            {
                await Task.Delay(1);
                Navigation.NavigateTo(Navigation.Uri, true);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }

        #endregion

        #region Events

        protected async override Task OnInitializedAsync()
        {
            try
            {
                loading = true;
                var authState = await _authState.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity.IsAuthenticated)
                {
                    UserID = Convert.ToInt32(user.Claims.Where(x => x.Type == "UserID").Select(x => x.Value).FirstOrDefault());
                    BusinessID = Convert.ToInt32(user.Claims.Where(x => x.Type == "BusinessID").Select(x => x.Value).FirstOrDefault());
                    LoggedInUser = user.Claims.Where(x => x.Type == "Username").Select(x => x.Value).FirstOrDefault();
                    Snackbar.Add("Welcome back " + LoggedInUser, Severity.Success);
                    await CallAPI();
                }
                loading = false;
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
                loading = false;
            }
        }

        #endregion
    }
}