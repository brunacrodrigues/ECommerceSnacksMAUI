using ECommerceSnacksMAUI.Models.Validators;
using ECommerceSnacksMAUI.Services;

namespace ECommerceSnacksMAUI.Pages;

public partial class OrderDetailsPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;

    public OrderDetailsPage(int orderId,
                              decimal totalPrice,
                              ApiService apiService,
                              IValidator validator)
	{
		InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        LblTotalPrice.Text = " �" + totalPrice;

        GetOrderDetails(orderId);

    }

    private async void GetOrderDetails(int orderId)
    {
        try
        {
            loadIndicator.IsRunning = true;
            loadIndicator.IsVisible = true;

            var (orderDetails, errorMessage) = await _apiService.GetOrderDetails(orderId);

            if (errorMessage == "Unauthorized" && !_loginPageDisplayed)
            {
                await DisplayLoginPage();
                return;
            }

            if (orderDetails is null)
            {
                await DisplayAlert("Error", errorMessage ?? "Unable to get order details.", "OK");
                return;
            }
            else
            {
                CvOrderDetails.ItemsSource = orderDetails;
            }
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "An error occurred while getting order details. Please, try again later.", "OK");
        }
        finally
        {
            loadIndicator.IsRunning = false;
            loadIndicator.IsVisible = false;
        }
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;
        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }
}