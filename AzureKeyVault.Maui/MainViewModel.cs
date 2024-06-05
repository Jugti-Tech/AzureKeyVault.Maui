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
        string functionUrl;

        [ObservableProperty]
        string secretNames;


        static HttpClient httpClient = new HttpClient();

        public MainViewModel()
        {
            // Load the FunctionURL and SecretNames from the appsettings.json file. The appsettings.json file is embedded in the project and is ignored by Git. 
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("AzureKeyVault.Maui.appsettings.json");
            IConfiguration? config = new ConfigurationBuilder().AddJsonStream(stream).Build();
            FunctionUrl = config["function:url"];
            SecretNames = "Secret1Name,Secret2Name";

        }

        public async Task LoadKeyVaultSecret()
        {
            
            string secrets =await GetSecretFromAzure();

           // Split the secrets into an array
            string[] secret1 = secrets.Split(",");

            // the loop is running twice because we are getting two secrets from the function. Use the secrets in the project as needed.    
            for(int i=0; i<2;i++)
            {
                string secret = secret1[i];
                string secretKey = secret.Split(":")[0].Replace("{", string.Empty).Replace("\"", string.Empty);
                string secretValue = secret.Split(":")[1].Replace("}", string.Empty).Replace("\"", string.Empty);
                if (i==0)
                {
                    Label1Text = $"{secretKey}:  {secretValue}";
                }
                else if (i==1)
                {
                    Label2Text = $"{secretKey}:  {secretValue}";
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
