namespace IMP_reseni.Views;

public partial class Delete : ContentPage
{
	public Delete()
	{
		InitializeComponent();
		BindingContext = new DeleteViewModel(this);
    }
}