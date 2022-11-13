using IMP_reseni.ViewModels;

namespace IMP_reseni.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
        this.BindingContext = new LoginViewModel(this);
	}
    //private async void GoToAdminPanel(object sender, EventArgs e)
    //{
    //     await Navigation.PushAsync(new Views.AdminPanel());
        
    //}
}