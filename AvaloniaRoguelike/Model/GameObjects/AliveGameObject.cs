using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaRoguelike.Model 
{
    public abstract class AliveGameObject : MovingGameObject, IAlive
    {
        protected AliveGameObject(GameField field, CellLocation location, Facing facing, int hp, int attack, double speed) : base(field, location, facing) 
        { 
            HP = hp;
            Attack = attack;
            _speed = speed;
        }
        public int HP
        {
            get;
            protected set;
        }
        public int Attack
        {
            get;
            protected set;
        }
        public double _speed;

        public bool IsAlive()
        {
            if (HP > 0) return true;
            return false;
        }
    }
}
