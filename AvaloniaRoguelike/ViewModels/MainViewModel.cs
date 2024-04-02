using AvaloniaRoguelike.Model;
using System.Collections.Generic;

namespace AvaloniaRoguelike.ViewModels;

public class MainViewModel(GameField field) : ViewModelBase
{
    ViewModelBase content = new GameViewModel(field.GameObjects);
}