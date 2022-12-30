namespace IMP_reseni.Views;
using ViewModels;
public partial class ModifySupplier : ContentPage
{
	public ModifySupplier()
	{
		InitializeComponent();
		BindingContext =new ModifySupplierViewModel();
	}
}