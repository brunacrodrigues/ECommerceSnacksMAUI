using ECommerceSnacksMAUI.Services;

namespace ECommerceSnacksMAUI.Pages;

public partial class MyAccountPage : ContentPage
{
    private readonly ApiService _apiService;

    private const string UserNameKey = "username";
    private const string UserEmailKey = "useremail";
    private const string UserPhoneNumberKey = "userphone";

    public MyAccountPage(ApiService apiService)
	{
		InitializeComponent();
        _apiService = apiService;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadUserInfo();
        ImgBtnProfile.Source = await GetProfileImageAsync();
    }

    private void LoadUserInfo()
    {
        lblUserName.Text = Preferences.Get(UserNameKey, string.Empty);
        EntName.Text = lblUserName.Text;
        EntEmail.Text = Preferences.Get(UserEmailKey, string.Empty);
        EntPhone.Text = Preferences.Get(UserPhoneNumberKey, string.Empty);
    }

    private async Task<string?> GetProfileImageAsync()
    {
        string imagemPadrao = AppConfig.DefaultProfileImage;

        var (response, errorMessage) = await _apiService.GetUserProfileImage();

        if (errorMessage is not null)
        {
            switch (errorMessage)
            {
                case "Unauthorized":
                    await DisplayAlert("Error", "Not authorized", "OK");
                    return imagemPadrao;
                default:
                    await DisplayAlert("Error", errorMessage ?? "Unable to get profile image", "OK");
                    return imagemPadrao;
            }
        }

        if (response?.UrlImage is not null)
        {
            return response.ImagePath;
        }
        return imagemPadrao;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Salva as informa  es alteradas pelo usu rio nas prefer ncias
        Preferences.Set(UserNameKey, EntName.Text);
        Preferences.Set(UserEmailKey, EntEmail.Text);
        Preferences.Set(UserPhoneNumberKey, EntPhone.Text);
        await DisplayAlert("Success", "Your account information was saved successfully", "OK");
    }
}