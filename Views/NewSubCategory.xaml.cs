using IMP_reseni.ViewModels;

namespace IMP_reseni.Views;

public partial class NewSubCategory : ContentPage
{
	public NewSubCategory()
	{
		InitializeComponent();
		BindingContext = new NewSubCategoryViewModel();
	}


}