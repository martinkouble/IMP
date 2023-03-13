namespace IMP_reseni.Views;
using IMP_reseni.ViewModels;

public partial class NewItem : ContentPage
{
	public NewItem(NewItemViewModel newItemViewModel)
	{
		InitializeComponent();
		BindingContext = newItemViewModel;
		newItemViewModel.Page = this;

    }
}