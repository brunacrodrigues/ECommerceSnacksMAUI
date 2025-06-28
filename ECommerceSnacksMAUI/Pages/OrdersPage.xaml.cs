using ECommerceSnacksMAUI.Models;
using ECommerceSnacksMAUI.Models.Validators;
using ECommerceSnacksMAUI.Services;

namespace ECommerceSnacksMAUI.Pages;

public partial class OrdersPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;


    public OrdersPage(ApiService apiService, IValidator validator)
	{
		InitializeComponent();
        _apiService = apiService;
        _validator = validator;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetOrdersList();
    }

    private async Task GetOrdersList()
    {
        try
        {
            var (orders, errorMessage) = await _apiService.GetOrdersByUser(Preferences.Get("userid", 0));

            if (errorMessage == "Unauthorized" && !_loginPageDisplayed)
            {
                await DisplayLoginPage();
                return;
            }
            if (errorMessage == "NotFound")
            {
                await DisplayAlert("Warning", "You don't have any orders yet.", "OK");
                return;
            }
            if (orders is null)
            {
                await DisplayAlert("Error", errorMessage ?? "Unable to get orders.", "OK");
                return;
            }
            else
            {
                CvOrders.ItemsSource = orders;
            }
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "An error occurred while getting your orders. Please, try again later.", "OK");
        }
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;
        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }

    private void CvOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault() as OrderByUser;

        if (selectedItem == null) return;

        Navigation.PushAsync(new OrderDetailsPage(selectedItem.Id,
                                                    selectedItem.Total,
                                                    _apiService,
                                                    _validator));

        ((CollectionView)sender).SelectedItem = null;
    }
}