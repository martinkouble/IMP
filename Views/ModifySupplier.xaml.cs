namespace IMP_reseni.Views;
using IMP_reseni.ViewModels;
public partial class ModifySupplier : ContentPage
{
	public ModifySupplier(ModifySupplierViewModel modifySupplierViewModel)
	{
		InitializeComponent();
		BindingContext = modifySupplierViewModel;
	}
}