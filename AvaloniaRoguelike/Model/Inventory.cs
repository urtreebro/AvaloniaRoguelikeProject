using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaRoguelike.Model;

public class Inventory
{
    private const int MAXITEMS = 32;

    public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

    public Item Weapon { get; }

    public Item Helmet { get; }

    public Item Armor { get; }

}
