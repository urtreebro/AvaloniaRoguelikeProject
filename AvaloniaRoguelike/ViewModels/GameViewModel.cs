using AvaloniaRoguelike.Model;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvaloniaRoguelike.ViewModels;

public class GameViewModel : ViewModelBase
{
    public GameViewModel(IEnumerable<GameObject> items)
    {
        Items = new ObservableCollection<GameObject>(items);
    }

    public ObservableCollection<GameObject> Items { get; }
}