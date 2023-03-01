using CommunityToolkit.Maui;
using IMP_reseni.Services;
using IMP_reseni.ViewModels;
using Microsoft.Maui.Controls.Hosting;
using Mopups.Hosting;

namespace IMP_reseni;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureMopups()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<SaveHolder>();

        builder.Services.AddSingleton<BasketHolder>();

        builder.Services.AddSingleton<CloudService>();

        //View,ViewModels
        builder.Services.AddTransient<Cloud,CloudViewModel>();
        builder.Services.AddTransient<Basket, BasketViewModel>();
        builder.Services.AddTransient<Accounting, AccountingViewModel>();

        builder.Services.AddTransient<StockUp>();
        builder.Services.AddTransient<MainPage>();

        builder.Services.AddTransient<Create>();
        builder.Services.AddTransient<NewCategory, NewCategoryViewModel>();
        builder.Services.AddTransient<NewSubCategory, NewSubCategoryViewModel>();
        builder.Services.AddTransient<NewItem, NewItemViewModel>();
        builder.Services.AddTransient<NewSupplier, NewSupplierViewModel>();

        builder.Services.AddTransient<Modify>();
        builder.Services.AddTransient<ModifyCategory, ModifyCategoryViewModel>();
        builder.Services.AddTransient<ModifySubCategory, ModifySubCategoryViewModel>();
        builder.Services.AddTransient<ModifyItem, ModifyItemViewModel>();
        builder.Services.AddTransient<ModifySupplier, ModifySupplierViewModel>();

        builder.Services.AddTransient<Delete>();
        builder.Services.AddTransient<DeleteCategory, DeleteCategoryViewModel>();
        builder.Services.AddTransient<DeleteSubCategory, DeleteSubCategoryViewModel>();
        builder.Services.AddTransient<DeleteItem, DeleteItemViewModel>();
        builder.Services.AddTransient<DeleteSupplier, DeleteSupplierViewModel>();
        builder.Services.UsePageResolver();
        return builder.Build();
	}
    
}
