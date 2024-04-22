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
    public abstract class Enemy : AliveGameObject, IEnemy
    {
        public Enemy(GameField field, CellLocation location, Facing facing, int hp, int attack, double speed) : base(field, location, facing, hp, attack, speed) { }

        protected override double SpeedFactor => _speed * base.SpeedFactor;
    }
}
