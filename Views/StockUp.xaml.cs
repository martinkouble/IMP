using IMP_reseni.Services;
using IMP_reseni.ViewModels;
namespace IMP_reseni.Views;

public partial class StockUp : ContentPage
{
	public StockUp(SaveHolder saveHolder)
	{
		InitializeComponent();
		BindingContext = new StockUpViewModel(this, saveHolder);
	}
}