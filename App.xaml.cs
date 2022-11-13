
using IMP_reseni.Views;

namespace IMP_reseni;

public partial class App : Application
{
    //private string Passwd;
    public  App()
    {

        SecureStorage.Default.RemoveAll();

        Task get = new Task(GetPassword);
        get.Start();
        get.Wait();

        InitializeComponent();

        //Task<string> oauthToken = SecureStorage.Default.GetAsync("oauth_token").Wait();

        //MainPage = new ContentPage(new MainPage());

        //MainPage = new AppShell();
    }
    private async void SetPassword() 
    {
        await SecureStorage.SetAsync("token", "SUS");
    }

    private async void GetPassword()
    {
        string Passwd = await SecureStorage.Default.GetAsync("token");
        if (Passwd != null)
        {
            MainPage = new NavigationPage(new MainPage());
        }

        else
        {
            MainPage = new NavigationPage(new SetPassword());
        }
    }

    

}
