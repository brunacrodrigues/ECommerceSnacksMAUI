using ECommerceSnacksMAUI.Models;
using ECommerceSnacksMAUI.Models.Validators;
using ECommerceSnacksMAUI.Services;
using System.ComponentModel.DataAnnotations;

namespace ECommerceSnacksMAUI.Pages;

public partial class ProductDetailsPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private int _productId;
    private bool _loginPageDisplayed = false;
    private readonly FavoritesService _favoritesService = new FavoritesService();
    private string? _urlImage;

    public ProductDetailsPage(int productId,
                                string productName,
                                ApiService apiService,
                                IValidator validator)
	{
		InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        _productId = productId;
        Title = productName ?? "Product Details";

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetProductDetails(_productId);
        UpdateFavoriteButton();
    }

    private async Task<Product?> GetProductDetails(int productId)
    {
        var (productDetails, errorMessage) = await _apiService.GetProductDetails(productId);

        if (errorMessage == "Unauthorized" && !_loginPageDisplayed)
        {
            await DisplayLoginPage();
            return null;
        }

        // Verificar se houve algum erro na obtenção das produtos
        if (productDetails == null)
        {
            // Lidar com o erro, exibir mensagem ou logar
            await DisplayAlert("Error", errorMessage ?? "Unable to get product.", "OK");
            return null;
        }

        if (productDetails != null)
        {
            // Atualizar as propriedades dos controles com os dados do produto
            ProductImage.Source = productDetails.ImagePath;
            LblProductName.Text = productDetails.Name;
            LblProductPrice.Text = productDetails.Price.ToString();
            LblProductDescription.Text = productDetails.Details;
            LblTotalPrice.Text = productDetails.Price.ToString();
            _urlImage = productDetails.ImagePath;
        }
        else
        {
            await DisplayAlert("Error", errorMessage ?? "Unable to get product details.", "OK");
            return null;
        }
        return productDetails;
    }

   

    private async void UpdateFavoriteButton()
    {
        var favoriteExists = await
               _favoritesService.ReadAsync(_productId);

        if (favoriteExists is not null)
            ImageBtnFavorite.Source = "heartfill";
        else
            ImageBtnFavorite.Source = "heart";
    }

    private void BtnRemove_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(LblQuantity.Text, out int quantity) &&
           decimal.TryParse(LblProductPrice.Text, out decimal unitPrice))
        {
            // Decrementa a quantidade, e n o permite que seja menor que 1
            quantity = Math.Max(1, quantity - 1);
            LblQuantity.Text = quantity.ToString();

            // Calcula o pre o total
            var totalPrice = quantity * unitPrice;
            LblTotalPrice.Text = totalPrice.ToString();
        }
        else
        {
            // Tratar caso as convers es falhem
            DisplayAlert("Error", "Invalid values", "OK");
        }
    }

    private void BtnAdd_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(LblQuantity.Text, out int quantity) &&
       decimal.TryParse(LblTotalPrice.Text, out decimal unitPrice))
        {
            // Incrementa a quantidade
            quantity++;
            LblQuantity.Text = quantity.ToString();

            // Calcula o pre o total
            var totalPrice = quantity * unitPrice;
            LblTotalPrice.Text = totalPrice.ToString(); // Formata como moeda
        }
        else
        {
            // Tratar caso as convers es falhem
            DisplayAlert("Error", "Invalid values", "OK");
        }

    }

    private async void BtnAddToCart_Clicked(object sender, EventArgs e)
    {
        try
        {
            var shoppingCart = new ShoppingCart()
            {
                Quantity = Convert.ToInt32(LblQuantity.Text),
                UnitPrice = Convert.ToDecimal(LblProductPrice.Text),
                Total = Convert.ToDecimal(LblTotalPrice.Text),
                ProductId = _productId,
                ClientId = Preferences.Get("userid", 0)
            };
            var response = await _apiService.AddItemToCart(shoppingCart);
            if (response.Data)
            {
                await DisplayAlert("Success", "Item added to cart !", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", $"Error adding item to cart: {response.ErrorMessage}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occured: {ex.Message}", "OK");
        }

    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;

        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }

    private async void ImageBtnFavorite_Clicked(object sender, EventArgs e)
    {
        try
        {
            var favoriteExists = await _favoritesService.ReadAsync(_productId);
            if (favoriteExists is not null)
            {
                await _favoritesService.DeleteAsync(favoriteExists);
            }
            else
            {
                var favoriteProduct = new FavoriteProduct()
                {
                    ProductId = _productId,
                    IsFavorite = true,
                    Details = LblProductDescription.Text,
                    Name = LblProductName.Text,
                    Price = Convert.ToDecimal(LblProductPrice.Text),
                    UrlImage = _urlImage
                };

                await _favoritesService.CreateAsync(favoriteProduct);
            }
            UpdateFavoriteButton();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occured: {ex.Message}", "OK");
        }
    }
   
}