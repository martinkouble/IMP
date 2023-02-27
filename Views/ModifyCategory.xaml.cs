namespace IMP_reseni.Views;
using IMP_reseni.ViewModels;
public partial class ModifyCategory : ContentPage
{
	public ModifyCategory(ModifyCategoryViewModel modifyCategoryViewModel)
	{
		InitializeComponent();
        BindingContext = modifyCategoryViewModel;

    }
}