using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaRoguelike.Views;
using AvaloniaRoguelike.Model;

namespace AvaloniaRoguelike
{

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
}