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

        builder.Services.AddTransient<Cloud>();
        builder.Services.UsePageResolver();
        return builder.Build();
	}
    
}
