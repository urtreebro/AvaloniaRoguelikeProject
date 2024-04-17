using Avalonia.Controls.Shapes;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia;

namespace AvaloniaRoguelike.Model
{
    public abstract class Enemy : MovingGameObject
    {
        public Enemy(GameField field, CellLocation location, Facing facing) : base(field, location, facing) { }
    }
}
