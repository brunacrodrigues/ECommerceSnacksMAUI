using ECommerceSnacksMAUI.Models;
using ECommerceSnacksMAUI.Models.Validators;
using ECommerceSnacksMAUI.Services;

namespace ECommerceSnacksMAUI.Pages;

public partial class ProductsListPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private int _categoriaId;
    private bool _loginPageDisplayed = false;

    public ProductsListPage(int categoriaId, string categoryName, ApiService apiService, IValidator validator)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        _categoriaId = categoriaId;
        Title = categoryName ?? "Products";  // Definindo o título da página
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetProductsList(_categoriaId);
    }

    private async Task<IEnumerable<Product>> GetProductsList(int categoryId)
    {
        try
        {
            var (products, errorMessage) = await _apiService.GetProducts("categoria", categoryId.ToString());

            if (errorMessage == "Unauthorized" && !_loginPageDisplayed)
            {
                await DisplayLoginPage();
                return Enumerable.Empty<Product>();
            }

            if (products is null)
            {
                await DisplayAlert("Error", errorMessage ?? "Unable to get categories.", "OK");
                return Enumerable.Empty<Product>();
            }

            CvProducts.ItemsSource = products;
            return products;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An unexpected error occured: {ex.Message}", "OK");
            return Enumerable.Empty<Product>();
        }
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;
        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }

    private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as Product;

        if (currentSelection is null)
            return;

        Navigation.PushAsync(new ProductDetailsPage(currentSelection.Id,
                                                     currentSelection.Name!,
                                                     _apiService,
                                                     _validator));

        ((CollectionView)sender).SelectedItem = null;

    }
}