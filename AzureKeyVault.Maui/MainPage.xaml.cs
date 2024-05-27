
namespace AzureKeyVault.Maui
{
    public partial class MainPage : ContentPage
    {
       
        readonly MainViewModel viewModel;
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = viewModel;
            Loaded += MainPage_Loaded;  
        }

        private async void MainPage_Loaded(object? sender, EventArgs e)
        {
            await viewModel.LoadKeyVaultSecret();
        }
    }

}
