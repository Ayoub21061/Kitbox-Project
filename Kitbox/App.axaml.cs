using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Kitbox.ViewModels;
using Kitbox.Views;

using ReactiveUI;
using Avalonia.ReactiveUI;       // <-- Ici, important pour AvaloniaScheduler

namespace Kitbox;

public partial class App : Application
{
    internal static object GetViewModel<T>()
    {
        throw new NotImplementedException();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // Configure ReactiveUI pour utiliser le scheduler Avalonia (thread UI)
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            BindingPlugins.DataValidators.RemoveAt(0);

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
