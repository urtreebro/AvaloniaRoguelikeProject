using System;

namespace AvaloniaRoguelike.Model;

public class Player : AliveGameObject, IPlayer
{
    public const int DEFAULTHP = 20;
    public const int DEFAULTATTACK = 4;
    public const int DEFAULTSPEED = 8;

    public Player(
        GameField field,
        CellLocation location,
        Facing facing) 
        : base(field, location, facing)
    {
        HP = DEFAULTHP;
        Attack = DEFAULTATTACK;
        Speed = DEFAULTSPEED;
        Exp = 0;
        PlayerLvl = 0;
        Luck = 1;
    }

    public int Exp { get; private set; }
    public int PlayerLvl { get; private set; }
    public int Luck { get; private set; }

    public bool IsNewLvl()
    {
        if (Exp != 0 && Exp == 1000 * Math.Pow(PlayerLvl, 3 / 2)) 
            return true;
        return false;
    }

    public void LvlUp()
    {
        PlayerLvl++;
    }
}