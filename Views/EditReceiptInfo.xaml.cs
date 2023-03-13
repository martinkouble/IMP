namespace IMP_reseni.Views;

public partial class EditReceiptInfo : ContentPage
{
	public EditReceiptInfo(EditReceiptInfoViewModel editReceiptInfoViewModel)
	{
		InitializeComponent();
		BindingContext=editReceiptInfoViewModel;
	}
}