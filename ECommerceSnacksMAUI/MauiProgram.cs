using CommunityToolkit.Maui;
using ECommerceSnacksMAUI.Models.Validators;
using ECommerceSnacksMAUI.Services;
using Microsoft.Extensions.Logging;

namespace ECommerceSnacksMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<IValidator, Validator>();
            return builder.Build();
        }
    }
}
