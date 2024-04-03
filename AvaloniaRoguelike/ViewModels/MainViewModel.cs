using AvaloniaRoguelike.Model;
using System.Collections.Generic;

namespace AvaloniaRoguelike.ViewModels
{
public class MainViewModel : ViewModelBase
{
    public MainViewModel(GameField field)
    {
        ViewModelBase content = new GameViewModel(field.GameObjects);
    } 
    
}}