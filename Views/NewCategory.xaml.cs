namespace IMP_reseni.Views;
using IMP_reseni.ViewModels;

public partial class NewCategory : ContentPage
{
	public NewCategory(NewCategoryViewModel newCategoryViewModel)
	{
		InitializeComponent();
        BindingContext = newCategoryViewModel;
        newCategoryViewModel.Page = this;
    }
}