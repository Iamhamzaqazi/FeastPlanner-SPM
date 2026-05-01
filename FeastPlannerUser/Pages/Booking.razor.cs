using AppDBContext.General;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace FeastPlannerUser.Pages
{
    public partial class Booking
    {
        #region Variables

        private bool loading = false;

        #endregion

        #region Events

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
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeAnimations();
                await AnimateNumbers();
            }
        }

        private async Task InitializeAnimations()
        {
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

        private async Task AnimateNumbers()
        {
            await JS.InvokeVoidAsync("eval", @"
            const animateNumber = (element) => {
                const target = parseInt(element.getAttribute('data-count'));
                let current = 0;
                const increment = target / 50;
                const updateNumber = () => {
                    current += increment;
                    if (current < target) {
                        element.textContent = Math.floor(current);
                        requestAnimationFrame(updateNumber);
                    } else {
                        element.textContent = target;
                    }
                };
                updateNumber();
            };

            const numberObserver = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        animateNumber(entry.target);
                        numberObserver.unobserve(entry.target);
                    }
                });
            });

            document.querySelectorAll('.metro-stat-number').forEach(el => {
                numberObserver.observe(el);
            });
        ");
        }

        #endregion
    }
}