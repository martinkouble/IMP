
using IMP_reseni.ViewModels;
using IMP_reseni.Services;

namespace IMP_reseni;

public partial class MainPage : ContentPage
{
	//int count = 0;

	public MainPage()
	{
		InitializeComponent();
        BindingContext =new MainPageViewModel(this,App.saveholder);
        
    }

    /*
     private async void GoToLogin(object sender, EventArgs e)
     {
         await Navigation.PushAsync(new Views.Login());

     }
    */
    //private void OnCounterClicked(object sender, EventArgs e)
    //{
    //	count++;

    //	if (count == 1)
    //		CounterBtn.Text = $"Clicked {count} time";
    //	else
    //		CounterBtn.Text = $"Clicked {count} times";

    //	SemanticScreenReader.Announce(CounterBtn.Text);
    //}
}

