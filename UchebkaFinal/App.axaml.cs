using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UchebkaFinal.Data;
using UchebkaFinal.ViewModels;
using UchebkaFinal.Views;

namespace UchebkaFinal
{
    public partial class App : Application


    {
        public static AppDbContext DbContext { get; private set; } = new AppDbContext();
        public static UserAccount? CurrentUser { get; set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            DbContext.Students.ToList();
            DbContext.Courses.ToList();
            DbContext.Staff.ToList();
            DbContext.Exams.ToList();
            DbContext.Programs.ToList();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}