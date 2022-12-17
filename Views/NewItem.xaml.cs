namespace IMP_reseni.Views;
using IMP_reseni.ViewModels;

public partial class NewItem : ContentPage
{
	public NewItem()
	{
		InitializeComponent();
		BindingContext = new NewItemViewModel();
	}
}