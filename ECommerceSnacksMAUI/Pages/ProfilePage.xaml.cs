using ECommerceSnacksMAUI.Models.Validators;
using ECommerceSnacksMAUI.Services;

namespace ECommerceSnacksMAUI.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;

    public ProfilePage(ApiService apiService, IValidator validator)
	{
		InitializeComponent();
        LblUserName.Text = Preferences.Get("username", string.Empty);
        _apiService = apiService;
        _validator = validator;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ImgBtnProfile.Source = await GetProfileImage();
    }

    private async Task<string?> GetProfileImage()
    {
        // Obtenha a imagem padr o do AppConfig
        string defaultImage = AppConfig.DefaultProfileImage;

        var (response, errorMessage) = await _apiService.GetUserProfileImage();

        // Lida com casos de erro
        if (errorMessage is not null)
        {
            switch (errorMessage)
            {
                case "Unauthorized":
                    if (!_loginPageDisplayed)
                    {
                        await DisplayLoginPage();
                        return null;
                    }
                    break;
                default:
                    await DisplayAlert("Error", errorMessage ?? "Unable to get image.", "OK");
                    return defaultImage;
            }
        }

        if (response?.UrlImage is not null)
        {
            return response.ImagePath;
        }

        return defaultImage;
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;
        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }


    private async void ImgBtnProfile_Clicked(object sender, EventArgs e)
    {
        try
        {
            var imagemArray = await SelectImageAsync();
            if (imagemArray is null)
            {
                await DisplayAlert("Error", "Unable to load image.", "Ok");
                return;
            }
            ImgBtnProfile.Source = ImageSource.FromStream(() => new MemoryStream(imagemArray));

            var response = await _apiService.UploadUserImage(imagemArray);
            if (response.Data)
            {
                await DisplayAlert("", "Image sent successfully", "Ok");
            }
            else
            {
                await DisplayAlert("Error", response.ErrorMessage ?? "An unknown error occurred", "Cancel");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An expected error occurred: {ex.Message}", "Ok");
        }
    }

    private async Task<byte[]> SelectImageAsync()
    {
        try
        {
            var arquivo = await MediaPicker.PickPhotoAsync();

            if (arquivo is null) return null;

            using (var stream = await arquivo.OpenReadAsync())
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Error", "This funcionality is not supported by this device.", "Ok");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Error", "Permissions not granted to access camera or gallery", "Ok");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error selecting image: {ex.Message}", "Ok");
        }
        return null;
    }

    private void TapOrders_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new OrdersPage(_apiService, _validator));
    }

    private void MyAccount_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void FAQ_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void BtnLogout_Clicked(object sender, EventArgs e)
    {

    }
}