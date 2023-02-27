namespace IMP_reseni.Views;
using IMP_reseni.ViewModels;
public partial class NewSupplier : ContentPage
{
	public NewSupplier(NewSupplierViewModel newSupplierViewModel)
	{
		InitializeComponent();
		BindingContext= newSupplierViewModel;
	}
}