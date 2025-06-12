using ECommerceSnacksMAUI.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ECommerceSnacksMAUI.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly ApiService _apiService;

    public RegisterPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        //_validator = validator;
    }

    private async void BtnSignup_Clicked(object sender, EventArgs e)
    {
        //if (await _validator.Validar(EntNome.Text, EntEmail.Text, EntPhone.Text, EntPassword.Text))
        //{

            var response = await _apiService.RegisterUser(EntNome.Text, EntEmail.Text,
                                                          EntPhone.Text, EntPassword.Text);

            if (!response.HasError)
            {
                await DisplayAlert("Success", "Account created successfully !!", "OK");
                await Navigation.PushAsync(new LoginPage(_apiService));
            }
            else
            {
                await DisplayAlert("Error", "Something went wrong!!!", "Cancel");
            }
        //}
        //else
        //{
        //    string mensagemErro = "";
        //    mensagemErro += _validator.NomeErro != null ? $"\n- {_validator.NomeErro}" : "";
        //    mensagemErro += _validator.EmailErro != null ? $"\n- {_validator.EmailErro}" : "";
        //    mensagemErro += _validator.TelefoneErro != null ? $"\n- {_validator.TelefoneErro}" : "";
        //    mensagemErro += _validator.SenhaErro != null ? $"\n- {_validator.SenhaErro}" : "";

        //    await DisplayAlert("Erro", mensagemErro, "OK");
        //}
    }

    private async void TapLogin_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(_apiService));
    }
}