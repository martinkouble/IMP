using IMP_reseni.ViewModels;

namespace IMP_reseni.Views;

public partial class ModifySubCategory : ContentPage
{
	public ModifySubCategory()
	{
		InitializeComponent();
        BindingContext = new ModifySubCategoryViewModel();

    }
}