using AvaloniaRoguelike.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaRoguelike.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public GameViewModel(IEnumerable<GameObject> items)
        {
            Items = new ObservableCollection<GameObject>(items);
        }

        public ObservableCollection<GameObject> Items { get; }
    }
}
