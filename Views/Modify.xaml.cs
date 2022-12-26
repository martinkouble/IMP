using IMP_reseni.ViewModels;

namespace IMP_reseni.Views;

public partial class Modify : ContentPage
{
	public Modify()
	{
		InitializeComponent();
        BindingContext = new ModifyViewModel(this);
    }
}