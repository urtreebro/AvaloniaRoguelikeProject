using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaRoguelike.ViewModels;
using AvaloniaRoguelike.Views;
using AvaloniaRoguelike.Infrastructure;
using AvaloniaRoguelike.Model;
using AvaloniaRoguelike.ViewModels;
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
        base.OnFrameworkInitializationCompleted();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var field = new GameField();
            var game = new Game(field);
            game.Start();

            desktop.MainWindow = new MainWindow(game, field);
        }
    }
}