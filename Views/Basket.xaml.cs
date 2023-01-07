using IMP_reseni.ViewModels;
namespace IMP_reseni.Views;

public partial class Basket : ContentPage
{

    public Basket()
	{
		InitializeComponent();
        BindingContext = new BasketViewModel();
    }

}