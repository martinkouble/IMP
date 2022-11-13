
using IMP_reseni.Views;

namespace IMP_reseni;

public partial class App : Application
{
    private bool Passwd;
    public App()
    {
        //SecureStorage.Default.SetAsync("token", "SUS");
        InitializeComponent();
        //SecureStorage.Default.RemoveAll();
        //Task<string> oauthToken = SecureStorage.Default.GetAsync("oauth_token").Wait();
        //GetPassword();
      
            MainPage = new NavigationPage(new MainPage());
        //MainPage = new ContentPage(new MainPage());

        //MainPage = new AppShell();
    }
 
    private async void GetPassword()
    {
        var token = await SecureStorage.Default.GetAsync("token");
        if(token != null)
        {
            Passwd = true;
            MainPage = new NavigationPage(new MainPage());

        }
        else
        {
            Passwd=false;
            MainPage = new NavigationPage(new SetPassword());

        }
        //return token;
    }

    

}
