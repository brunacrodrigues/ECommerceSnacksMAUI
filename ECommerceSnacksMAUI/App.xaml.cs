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
            MainPage = new NavigationPage(new RegisterPage(_apiService, _validator));
        }
    }
}
