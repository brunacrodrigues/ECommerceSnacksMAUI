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
        }
        else
        {
            await DisplayAlert("Error", errorMessage ?? "Unable to get product details.", "OK");
            return null;
        }
        return productDetails;
    }

    private void ImageBtnFavourite_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnRemove_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnAdd_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnAddToCart_Clicked(object sender, EventArgs e)
    {

    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;

        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }
}