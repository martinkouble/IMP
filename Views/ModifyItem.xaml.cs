namespace IMP_reseni.Views;
using IMP_reseni.ViewModels;
public partial class ModifyItem : ContentPage
{
	public ModifyItem(ModifyItemViewModel modifyItemViewModel)
	{
		InitializeComponent();
		BindingContext = modifyItemViewModel;
	}
}