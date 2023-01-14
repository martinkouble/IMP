using IMP_reseni.ViewModels;
using IMP_reseni.Models;
namespace IMP_reseni.Views;

public partial class AddItemToBasket : ContentPage
{
	public AddItemToBasket(Items item)
	{
		InitializeComponent();
        BindingContext = new AddItemToBasketViewModel(item);
    }
}