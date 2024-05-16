using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaRoguelike.Model
{
    public abstract class AliveGameObject : MovingGameObject, IAlive
    {
        protected AliveGameObject(GameField field, CellLocation location, Facing facing) : base(field, location, facing)
        {

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
        protected double Speed
        {
            get;
            set;
        }

        public bool IsAlive()
        {
            if (HP > 0) return true;
            return false;
        }

        public void GetDamage(int attack)
        {
            HP -= attack;
        }
    }
}