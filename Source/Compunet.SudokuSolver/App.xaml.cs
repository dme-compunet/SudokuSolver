using Compunet.SudokuSolver.Application;
using Compunet.SudokuSolver.Container;
using Compunet.SudokuSolver.Mvvm;
using Compunet.SudokuSolver.Print;
using Compunet.SudokuSolver.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Compunet.SudokuSolver
{
    public partial class App : System.Windows.Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            IoC.CreateSimple(services);

            await IoC.Simple.GetRequiredService<ISettingsService>().Load();
            await IoC.Simple.GetRequiredService<IApplicationResourceManager>().Load();

            MainWindow = new AppWindow();
            MainWindow.Show();
        }

        private IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services.AddTransient<CreatePuzzleDialogViewModel>()
                           .AddTransient<SudokuViewModel>()
                           .AddTransient<AppSidebarViewModel>()
                           .AddSingleton<ISudokuStoreService, SudokuStoreService>()
                           .AddSingleton<ISudokuInputService, SudokuInputService>()
                           .AddSingleton<IApplicationResourceManager, ApplicationResourceManager>()
                           .AddSingleton<ICreatePuzzleDialogService, CreatePuzzleDialogService>()
                           .AddSingleton<IPrintService, PrintService>()
                           .AddSingleton<ISettingsService, SettingsService>()
                           .AddSingleton<IExitService, ExitService>()
                           ;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            IoC.Simple.GetRequiredService<IExitService>().OnExit();
            IoC.Simple.GetRequiredService<ISettingsService>().Save().Wait();
        }
    }
}
