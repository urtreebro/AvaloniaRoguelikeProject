using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaRoguelike.ViewModels;
using AvaloniaRoguelike.Views;
using AvaloniaRoguelike.Infrastructure;
using AvaloniaRoguelike.Model;
using System;

namespace AvaloniaRoguelike;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    static void Main(string[] args)
    {
        Func<GameField> func = () =>
        {
            var field = new GameField();
            var game = new Game(field);
            game.Start();
            return field;
        };
        
    }

    public static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App>();
}
