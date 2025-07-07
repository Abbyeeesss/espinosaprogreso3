using espinosaprogreso3.Services;
using espinosaprogreso3.ViewModels;
using espinosaprogreso3.Views;

namespace espinosaprogreso3;

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
            });

        // Registrar servicios
        builder.Services.AddSingleton<DatabaseService>();

        // Registrar ViewModels
        builder.Services.AddSingleton<ListaPrendasViewModel>();
        builder.Services.AddTransient<FormularioViewModel>();
        builder.Services.AddTransient<LogsViewModel>();

        // Registrar Views
        builder.Services.AddSingleton<ListaPrendaPage>();
        builder.Services.AddTransient<FormularioPage>();
        builder.Services.AddTransient<LogsPage>();

        return builder.Build();
    }
}