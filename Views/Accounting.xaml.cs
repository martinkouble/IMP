using IMP_reseni.ViewModels;
namespace IMP_reseni.Views;

public partial class Accounting : ContentPage
{
	public Accounting(AccountingViewModel accountingViewModel)
	{
        InitializeComponent();
        BindingContext = accountingViewModel;
    }
}