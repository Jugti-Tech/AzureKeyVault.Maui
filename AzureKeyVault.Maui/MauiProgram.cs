using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureKeyVault.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                 .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMarkup()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage, MainViewModel>();

            string? environment = builder.Configuration.GetSection("Environment").Value; 
            Uri? keyVaultUri = new Uri( builder.Configuration["keyVault:url"]!);
            string? clientSecret = builder.Configuration["keyVault:clientSecret"];
            string? clientId = builder.Configuration["keyVault:clientId"];
            string? tenantId = builder.Configuration["keyVault:tenantId"];

            builder.Configuration.AddAzureKeyVault(
                               keyVaultUri,
                                new ClientSecretCredential(tenantId, clientId, clientSecret),
                               // new DefaultAzureCredential(),
                                new AzureKeyVaultConfigurationOptions()
                                {
                                    Manager = new CustomSecretManager(prefix:"TownMilk"),
                                    ReloadInterval = System.TimeSpan.FromMinutes(5)
                                }
                                );


            

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
