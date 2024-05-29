

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CommunityToolkit.Mvvm.ComponentModel;

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

        public MainViewModel()
        {
           
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
               
            }
            catch (Exception ex)
            {

                
            }

            return "dummy secret";
        }   
    }
}
