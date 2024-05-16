using AvaloniaRoguelike.Model;
using ReactiveUI;

using System;
using System.Collections.Generic;
using System.Reactive;

namespace AvaloniaRoguelike.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase content;
    private ViewModelBase[] gameViewModels;
    
   

        public MainWindowViewModel()
        {
            gameViewModels = new ViewModelBase[3];
            gameViewModels[0] = new MainMenuViewModel();
            gameViewModels[1] = new MainViewModel();
            gameViewModels[2] = new InventoryViewModel();
            Content = gameViewModels[0];
            ButtonPlayCommand = ReactiveCommand.Create(ButtonPlayClick);
            ButtonOptionsCommand = ReactiveCommand.Create(ButtonOptionsClick);
            ButtonQuitCommand = ReactiveCommand.Create(ButtonQuitClick);
        }

        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public void ButtonPlayClick()
        {
            // 1 - сама игра
            Content = gameViewModels[1];
            ((MainViewModel)gameViewModels[1]).StartGame();
        }

        public void ButtonOptionsClick()
        {
            Content = gameViewModels[2];
        }

        public void ButtonQuitClick()
        {
            Environment.Exit(0);
        }

        public ReactiveCommand<Unit, Unit> ButtonPlayCommand { get; }
        public ReactiveCommand<Unit, Unit> ButtonOptionsCommand { get; }
        public ReactiveCommand<Unit, Unit> ButtonQuitCommand { get; }
    }
}