using IMP_reseni.ViewModels;
namespace IMP_reseni.Views;

public partial class StockUp : ContentPage
{
	public StockUp()
	{
		InitializeComponent();
		BindingContext = new StockUpViewModel(this);
	}
}