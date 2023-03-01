namespace IMP_reseni.Views;

public partial class DeleteCategory : ContentPage
{
	public DeleteCategory(DeleteCategoryViewModel deleteCategoryViewModel)
	{
		InitializeComponent();
		BindingContext = deleteCategoryViewModel;
    }
}