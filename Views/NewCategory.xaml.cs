namespace IMP_reseni.Views;
using IMP_reseni.ViewModels;

public partial class NewCategory : ContentPage
{
	public NewCategory()
	{
		InitializeComponent();
        BindingContext = new NewCategoryViewModel();
    }
}