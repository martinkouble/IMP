using IMP_reseni.ViewModels;

namespace IMP_reseni.Views;

public partial class AdminPanel : ContentPage
{
	public AdminPanel()
	{
		InitializeComponent();
        BindingContext = new AdminPanelViewModel(this);
    }
}