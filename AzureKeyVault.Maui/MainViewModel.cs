

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace AzureKeyVault.Maui
{
    public partial class MainViewModel : ObservableObject
    {

        [ObservableProperty]
        string label1Text;

        [ObservableProperty]
        string label2Text;

        [ObservableProperty]
        string label3Text;

        string? keyVaultUri ;
        string? clientSecret;
        string? clientId ;
        string? tenantId ;

        string? environment ;

        public MainViewModel()
        {
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("AzureKeyVault.Maui.appsettings.json");
            IConfiguration? config = new ConfigurationBuilder().AddJsonStream(stream).Build();
           keyVaultUri = config["keyVault:url"];
           clientSecret = config["keyVault:clientSecret"];
           clientId = config["keyVault:clientId"];
           tenantId = config["keyVault:tenantId"];

            string? environment = config["Environment"];
        }

        public async Task LoadKeyVaultSecret()
        {
            // Replace with your key vault secret name
            string key1 = "firstSecretKey";
            string key2 = "secondSecretKey";
            string key3 = "thirdSecretKey";

            string secret1 =await GetSecretFromAzure(key1);
            string secret2 = await GetSecretFromAzure(key2);
            string secret3 = await GetSecretFromAzure(key3);


            Label1Text = $"Secret1:  {secret1}";
            Label2Text = $"Secret2:  {secret2}";
            Label3Text = $"Secret3:  {secret3}";
        }

        private async Task<string> GetSecretFromAzure(string key)
        {
            try
            {
               
                ClientSecretCredential clientSecretCredential = new ClientSecretCredential(tenantId, clientId,clientSecret);
                var client = new SecretClient(new Uri(keyVaultUri), clientSecretCredential);
                KeyVaultSecret secret = await client.GetSecretAsync(key);
                return secret.Value;    
               
            }
            catch (Exception ex)
            {

                
            }

            return "dummy secret";
        }   
    }
}
