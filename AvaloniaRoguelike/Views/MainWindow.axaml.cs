using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

using AvaloniaRoguelike.Model;
using AvaloniaRoguelike.ViewModels;

namespace AvaloniaRoguelike.Views;

public partial class MainWindow : Window
{
    public MainWindow(
        Game game,
        GameField gameField)
    {
        AvaloniaXamlLoader.Load(this);
        this.AttachDevTools();
        DataContext = new MainWindowViewModel(game, gameField);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        Keyboard.Keys.Add(e.Key);
        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        Keyboard.Keys.Remove(e.Key);
        base.OnKeyUp(e);
    }
}
