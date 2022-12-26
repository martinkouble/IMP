namespace IMP_reseni.Views;
using IMP_reseni.ViewModels;
public partial class ModifyCategory : ContentPage
{
	public ModifyCategory()
	{
		InitializeComponent();
        BindingContext = new ModifyCategoryViewModel();

    }
}