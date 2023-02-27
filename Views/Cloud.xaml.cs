using CommunityToolkit.Maui.Views;
using IMP_reseni.Services;
using IMP_reseni.ViewModels;
using System.Windows.Input;

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