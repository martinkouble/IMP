using IMP_reseni.ViewModels;
namespace IMP_reseni.Views;

public partial class Cloud : ContentPage
{
	//public Cloud()
	//{
	//	InitializeComponent();
	//	BindingContext= new CloudViewModel();
	//}
	public Cloud(CloudViewModel cloudViewModel)
	{
		InitializeComponent();
		BindingContext = cloudViewModel;
	}
}