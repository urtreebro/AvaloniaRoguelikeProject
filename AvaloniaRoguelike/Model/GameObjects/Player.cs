using System;
using System.Linq;

namespace AvaloniaRoguelike.Model
{
    public class Player : AliveGameObject, IPlayer
    {
        const int DEFAULTHP = 20;
        const int DEFAULTATTACK = 4;
        const int DEFAULTSPEED = 8;
        public int Exp { get; private set; }
        public int PlayerLevel { get; private set; }
        public int Luck { get; private set; }
        public Player(GameField field, CellLocation location, Facing facing) : base(field, location, facing)
        {
            HP = DEFAULTHP;
            Attack = DEFAULTATTACK;
            Speed = DEFAULTSPEED;
            Exp = 0;
            PlayerLevel = 0;
            Luck = 1;
        }

        public bool IsNewLevel()
        {
            if (Exp != 0 && Exp == 1000 * Math.Pow(PlayerLevel, 3 / 2)) return true;
            return false;
        }

        public void LevelUp()
        {
            PlayerLevel++;
        }
    }
}
