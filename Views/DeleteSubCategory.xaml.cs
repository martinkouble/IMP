namespace IMP_reseni.Views;

public partial class DeleteSubCategory : ContentPage
{
	public DeleteSubCategory(DeleteSubCategoryViewModel deleteSubCategoryViewModel)
	{
		InitializeComponent();
		BindingContext = deleteSubCategoryViewModel;

    }
}