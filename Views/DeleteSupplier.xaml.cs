namespace IMP_reseni.Views;

public partial class DeleteSupplier : ContentPage
{
	public DeleteSupplier(DeleteSupplierViewModel supplierViewModel)
	{
		InitializeComponent();
		BindingContext = supplierViewModel;

    }
}