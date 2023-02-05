using IMP_reseni.Models;
using IMP_reseni.ViewModels;
namespace IMP_reseni.Views;

public partial class ItemStockUp : ContentPage
{
	public ItemStockUp(Items item)
	{
        InitializeComponent();
        BindingContext = new ItemStockUpViewModel(item);
    }
}