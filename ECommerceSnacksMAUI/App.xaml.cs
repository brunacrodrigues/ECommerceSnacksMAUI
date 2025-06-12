using ECommerceSnacksMAUI.Models.Validators;
using ECommerceSnacksMAUI.Pages;
using ECommerceSnacksMAUI.Services;

namespace ECommerceSnacksMAUI
{
    public partial class App : Application
    {
        private ApiService _apiService;
        private IValidator _validator;

        public App(ApiService apiService, IValidator validator)
        {
            InitializeComponent();
            _apiService = apiService;
            _validator = validator;

            SetMainPage();
        }

        private void SetMainPage()
        {
            var accessToken = Preferences.Get("accesstoken", string.Empty);

            if (string.IsNullOrEmpty(accessToken))
            {
                MainPage = new NavigationPage(new LoginPage(_apiService, _validator));
                return;
            }

            MainPage = new AppShell(_apiService, _validator);
        }

    }
}
