using IMP_reseni.ViewModels;

namespace IMP_reseni.Views;

public partial class Create : ContentPage
{
	public Create()
	{
		InitializeComponent();
        BindingContext = new CreateViewModel(this);
    }
}