using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

namespace AzureKeyVault.Maui
{
    public partial class MainViewModel : ObservableObject
    {

        [ObservableProperty]
        string label1Text;

        [ObservableProperty]
        string label2Text;

        [ObservableProperty]
        string functionUrl;

        [ObservableProperty]
        string secretNames;


        static HttpClient httpClient = new HttpClient();

        private IConfiguration configuration;

        public MainViewModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            // Load the FunctionURL and SecretNames from the appsettings.json file. The appsettings.json file is embedded in the project and is ignored by Git. 

            FunctionUrl = configuration["url"];
            SecretNames = configuration["secretNames"];
            
        }

        public async Task LoadKeyVaultSecret()
        {
            
            string secrets =await GetSecretFromAzure();

           // Split the secrets into an array
            string[] allSecrets = secrets.Split(",");

            // the loop is running twice because we are getting two secrets from the function.   
            for(int i=0;i<allSecrets.Length;i++)
            {
                string secret = allSecrets[i];
                string secretKey = secret.Split(":")[0].Replace("{", string.Empty).Replace("\"", string.Empty);
                string secretValue = secret.Split(":")[1].Replace("}", string.Empty).Replace("\"", string.Empty);
                if (i==0)
                {
                    // set the value to the json property in appsettings.json and use it anywhere in the project
                    configuration["secret1"] = secretValue;

               
                    // set the label text to the secret key and value
                    Label1Text = $"{secretKey}:  {configuration["secret1"]}";

                 

                }
                else if (i==1)
                {
                    // set the value to the json property in appsettings.json and use it anywhere in the project
                    configuration["secret2"] = secretValue;

                   // set the label text to the secret key and value
                    Label2Text = $"{secretKey}:  {configuration["secret2"]}";

                    
                }
            }   
           
           
        }

        private async Task<string> GetSecretFromAzure()
        {
            try
            {
                Uri uri = new Uri($"{FunctionUrl}&SecretNames={SecretNames}");

                var getRequest = await httpClient.GetAsync(uri);
                var secretJson = await getRequest.Content.ReadAsStringAsync();
                return secretJson;


            }
            catch (Exception ex)
            {

                
            }
            return string.Empty;
        }   
    }
}
