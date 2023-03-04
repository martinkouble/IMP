namespace IMP_reseni.Views;

public partial class DeleteItem : ContentPage
{
	public DeleteItem(DeleteItemViewModel deleteItemViewModel)
	{
		InitializeComponent();
		BindingContext = deleteItemViewModel;
    }
}