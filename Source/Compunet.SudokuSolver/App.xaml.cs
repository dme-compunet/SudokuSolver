using Compunet.SudokuSolver.Container;
using Compunet.SudokuSolver.Mvvm;
using Compunet.SudokuSolver.Services;
using Compunet.SudokuSolver.Theming.Themes;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Compunet.SudokuSolver
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            IoC.CreateSimple(services);

            await IoC.Simple.GetRequiredService<ISettingsService>().Load();
            await IoC.Simple.GetRequiredService<IThemeService>().Load();

            MainWindow = new AppWindow();
            MainWindow.Show();
        }

        private IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services.AddTransient<CreatePuzzleDialogViewModel>()
                           .AddTransient<SudokuViewModel>()
                           .AddTransient<AppWindowViewModel>()
                           .AddTransient<AppSidebarViewModel>()
                           .AddSingleton<ISudokuStoreService, SudokuStoreService>()
                           .AddSingleton<ISudokuInputService, SudokuInputService>()
                           .AddSingleton<ISettingsService, SettingsService>()
                           .AddSingleton<ICreatePuzzleDialogService, CreatePuzzleDialogService>()
                           .AddSingleton<IExitService, ExitService>()
                           .AddSingleton<IThemeService, ThemeManager>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            IoC.Simple.GetRequiredService<IExitService>().OnExit();
            IoC.Simple.GetRequiredService<ISettingsService>().Save().Wait();
        }
    }
}
