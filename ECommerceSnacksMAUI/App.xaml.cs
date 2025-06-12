using ECommerceSnacksMAUI.Pages;
using ECommerceSnacksMAUI.Services;

namespace ECommerceSnacksMAUI
{
    public partial class App : Application
    {
        private ApiService _apiService;

        public App(ApiService apiService)
        {
            InitializeComponent();          
            _apiService = apiService;
            MainPage = new NavigationPage(new RegisterPage(_apiService));
        }
    }
}
