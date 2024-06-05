using System;
using System.Linq;

namespace AvaloniaRoguelike.Model;

public class Player : AliveGameObject, IPlayer
{
    public const int DEFAULTHP = 100;
    public const int DEFAULTATTACK = 4;
    public const int DEFAULTSPEED = 16;

    public Player(
        CellLocation cellLocation) 
        : base(cellLocation, Facing.East)
    {
        Health = DEFAULTHP;
        Damage = DEFAULTATTACK;
        Speed = DEFAULTSPEED;
        Exp = 0;
        Level = 0;
        Luck = 1;
    }

    public int Exp { get; private set; }
    public int Level { get; private set; }
    public int Luck { get; private set; }
    public string Name { get; set; }

    public void SetField(GameField field) => SetFieldInner(field);

    public bool IsNewLvl()
    {
        if (Exp != 0 && Exp == 1000 * Math.Pow(Level, 3 / 2)) 
            return true;
        return false;
    }

    public void LvlUp()
    {
        Level++;
    }

    public void Attack()
    {
        AttackSelectedTarget();
    }

    protected override void AttackSelectedTarget()
    {
        var tiles = GetTilesAtSight();
        
        foreach (var tile in tiles)
        {
            foreach (MovingGameObject enemy in _field.GameObjects.OfType<Enemy>())
            {
                if (tile.CellLocation == enemy.CellLocation)
                {
                    _targetMovingGameObject = enemy; 
                    break;
                }
            }
        }
        var canAttackEnemy = CheckCanAttackEnemy();
        if (canAttackEnemy && DateTime.Now.TimeOfDay - _lastAttackTime > _attackCooldown)
        {
            _lastAttackTime = DateTime.Now.TimeOfDay;
            base.AttackSelectedTarget();
        }
    }
}