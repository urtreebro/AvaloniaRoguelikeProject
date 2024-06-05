using ReactiveUI;

using System;
using System.Reactive;

namespace AvaloniaRoguelike.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase content;
    private ViewModelBase[] gameViewModels;
   
    public MainWindowViewModel()
    {
        gameViewModels = new ViewModelBase[4];
        gameViewModels[0] = new MainMenuViewModel();
        gameViewModels[1] = new MainViewModel();
        gameViewModels[2] = new InventoryViewModel();
        PlayerViewModel = new PlayerViewModel();
        gameViewModels[3] = PlayerViewModel;
        Content = gameViewModels[0];
        ButtonPlayCommand = ReactiveCommand.Create(ButtonPlayClick);
        ButtonOptionsCommand = ReactiveCommand.Create(ButtonOptionsClick);
        ButtonQuitCommand = ReactiveCommand.Create(ButtonQuitClick);
        ButtonEscapeCommand = ReactiveCommand.Create(ButtonEscapePress);
        OpenCloseInventoryCommand = ReactiveCommand.Create(OpenCloseInventory);
    }

    public ViewModelBase Content
    {
        get => content;
        private set => this.RaiseAndSetIfChanged(ref content, value);
    }

    public PlayerViewModel PlayerViewModel { get; private set; }

    public void ButtonPlayClick()
    {
        // 1 - сама игра
        var mainViewModel = (MainViewModel)gameViewModels[1];
        Content = mainViewModel;
        mainViewModel.StartGame();
        var playerViewModel = (PlayerViewModel)gameViewModels[3];
        playerViewModel.Player = mainViewModel.Game.Player;
    }

    public void ButtonOptionsClick()
    {
        Content = gameViewModels[2];
    }

    public void ButtonQuitClick()
    {
        Environment.Exit(0);
    }

    public void ButtonEscapePress()
    {
        if (Content == gameViewModels[1])
        {
            ((MainViewModel)gameViewModels[1]).StopGame();
            Content = gameViewModels[0];
        }
        else if (Content == gameViewModels[0])
        {
            Environment.Exit(0);
        }
    }

    public void OpenCloseInventory()
    {
        if (Content !=  gameViewModels[1])
        {
            return;
        }
        var game = ((MainViewModel)gameViewModels[1]).Game;
        game.OpenClosePlayerView();
    }

    public ReactiveCommand<Unit, Unit> ButtonPlayCommand { get; }
    public ReactiveCommand<Unit, Unit> ButtonOptionsCommand { get; }
    public ReactiveCommand<Unit, Unit> ButtonQuitCommand { get; }
    public ReactiveCommand<Unit, Unit> ButtonEscapeCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenCloseInventoryCommand { get; }
}
