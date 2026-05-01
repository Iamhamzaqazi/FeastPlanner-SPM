using AppDBContext.General;
using BlazorTypewriter;
using Microsoft.JSInterop;
using MudBlazor;

namespace FeastPlannerUser.Pages
{
    public partial class Index
    {
        #region Variables

        private bool loading = false;

        #endregion

        #region Type Writer Effect

        TypewriterBuilder typewriterName = new TypewriterBuilder(defaultCharacterPause: 6)
            .TypeString("Feast Planner", 150)
            .Pause(1500);

        TypewriterBuilder typewriter = new TypewriterBuilder(defaultCharacterPause: 6)
            .TypeString("Banquets.", 80)
            .Pause(1500)
            .DeleteAll(80)
            .TypeString("Photography.", 80)
            .Pause(1500)
            .DeleteAll(80)
            .TypeString("Catering.", 80)
            .Pause(1500)
            .DeleteAll(80)
            .TypeString("Event Planner.", 80)
            .Pause(1500)
            .DeleteAll(80)
            .TypeString("Collaboration.", 80)
            .Pause(1500)
            .DeleteAll(80)
            .Pause(1500)
            .DeleteAll(80)
            .TypeString("Where Every Celebration Finds Its Perfect Space.", 80)
            .Pause(1500)
            .DeleteAll(80)
            .Pause(1500)
            .DeleteAll(80)
            .Loop();

        #endregion

        #region Events

        private async Task GotoLogin()
        {
            try
            {
                await Task.Delay(1);
                Navigation.NavigateTo("https://localhost:7092/", true);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }

        private async Task GotoBooking()
        {
            try
            {
                await Task.Delay(1);
                Navigation.NavigateTo("https://localhost:7086/Booking", true);
            }
            catch (Exception ex)
            {
                LogsUI.GenerateLogs(ex);
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeAOS();
            }
        }

        private async Task InitializeAOS()
        {
            // Initialize AOS animations
            await Task.Delay(100);
            await JS.InvokeVoidAsync("eval", @"
            const observerOptions = {
                threshold: 0.1,
                rootMargin: '0px 0px -50px 0px'
            };

            const observer = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        entry.target.classList.add('aos-animate');
                        observer.unobserve(entry.target);
                    }
                });
            }, observerOptions);

            document.querySelectorAll('[data-aos]').forEach(el => {
                observer.observe(el);
            });
        ");
        }
        protected async override Task OnInitializedAsync()
        {
            try
            {
                loading = true;
                await Task.Delay(1);
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