using Microsoft.Extensions.Logging;
using TaskManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using TaskManagementApp.Services;
using TaskManagementApp.ViewModels;
using TaskManagementApp.Views;

namespace TaskManagementApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "tasks.db");
        builder.Services.AddDbContext<TaskDbContext>(options =>
            options.UseSqlite($"Filename={dbPath}"));

        builder.Services.AddSingleton<ITaskService, TaskService>();

        // Register ViewModels and Views for Dependency Injection
        builder.Services.AddTransient<TaskCreationViewModel>(); // Or AddSingleton if appropriate
        builder.Services.AddTransient<TaskCreationPage>();      // Or AddSingleton if appropriate

		return builder.Build();
	}
}
