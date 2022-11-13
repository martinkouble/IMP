namespace IMP_reseni.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}
    private async void GoToAdminPanel(object sender, EventArgs e)
    {
         await Navigation.PushAsync(new Views.AdminPanel());
        
    }
}