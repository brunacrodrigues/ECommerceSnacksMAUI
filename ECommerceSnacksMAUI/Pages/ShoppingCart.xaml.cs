using ECommerceSnacksMAUI.Models;
using ECommerceSnacksMAUI.Models.Validators;
using ECommerceSnacksMAUI.Services;
using System.Collections.ObjectModel;

namespace ECommerceSnacksMAUI.Pages;

public partial class ShoppingCartPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;

    private ObservableCollection<ShoppingCartItem> ShoppingCartItems = new ObservableCollection<ShoppingCartItem>();

    public ShoppingCartPage(
        ApiService apiService,
        IValidator validator)
	{
		InitializeComponent();
        _apiService = apiService;
        _validator = validator;
    }

    private void BtnDecrease_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnIncrease_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnDelete_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnEditAddress_Clicked(object sender, EventArgs e)
    {

    }

    private void TapConfirmOrder_Tapped(object sender, TappedEventArgs e)
    {

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetShoppingCartItems();
    }

    private async Task<bool> GetShoppingCartItems()
    {
        try
        {
            var userId = Preferences.Get("userid", 0);
            var (shoopingCartItems, errorMessage) = await 
                     _apiService.GetShoppingCartItems(userId);

            if (errorMessage == "Unauthorized" && !_loginPageDisplayed)
            {
                // Redirecionar para a p?gina de login
                await DisplayLoginPage();
                return false;
            }

            if (shoopingCartItems == null)
            {
                await DisplayAlert("Error", errorMessage ?? "Unable to get shopping cart items.", "OK");
                return false;
            }

            ShoppingCartItems.Clear();
            foreach (var item in shoopingCartItems)
            {
                ShoppingCartItems.Add(item);
            }

            CvCart.ItemsSource = shoopingCartItems;
            UpdateTotalPrice(); // Atualizar o preco total ap?s atualizar os itens do carrinho

            if (!ShoppingCartItems.Any())
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            return false;
        }
    }

    private void UpdateTotalPrice()
    {
        try
        {
            //var totalPrice = ShoppingCartItems.Sum(item => item.UnitPrice * item.Quantity); // TODO fix later?
            var totalPrice = ShoppingCartItems.Sum(item => item.Total);
            LblTotalPrice.Text = totalPrice.ToString();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"An error occurred while updating the total price.: {ex.Message}", "OK");
        }
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;

        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }
}