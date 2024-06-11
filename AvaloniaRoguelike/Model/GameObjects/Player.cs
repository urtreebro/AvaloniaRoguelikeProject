using ReactiveUI;

using System;
using System.Linq;

namespace AvaloniaRoguelike.Model;

public class Player : AliveGameObject, IPlayer
{
    public const int DEFAULT_HP = 50;
    public const int DEFAULT_MANA = 10;

    public const int DEFAULT_STRENGTH = 10;
    public const int DEFAULT_DEXTERITY = 10;
    public const int DEFAULT_MAGIC = 5;
    public const int DEFAULT_VITALITY = 15;

    public const int DEFAULT_MIN_DAMAGE = 1;
    public const int DEFAULT_MAX_DAMAGE = 2;
    public const int DEFAULT_SPEED = 16;
    public const int DEFAULT_ARMORCLASS = 5;
    public const int DEFAULT_TOHIT = 50;

    private int _mana;
    private int _strength;
    private int _dexterity;
    private int _magic;
    private int _vitality;
    private int _experienceToNextLevel;

    public int[] ExperienceToLevel = [0, 8, 16, 32, 64, 128, 256, 512, 1024, int.MaxValue];

    public Player(
        CellLocation cellLocation) 
        : base(cellLocation, Facing.East)
    {
        Speed = DEFAULT_SPEED;
        Experience = 0;
        Level = 1;
        Luck = 1;
        Strength = DEFAULT_STRENGTH;
        Dexterity = DEFAULT_DEXTERITY;
        Magic = DEFAULT_MAGIC;
        Vitality = DEFAULT_VITALITY;
        ArmorClass = DEFAULT_ARMORCLASS;
        ToHit = DEFAULT_TOHIT;
        Name = "Adventurer";
        RecalculateStats();
    }

    public Player()
        : this(default)
    {
    }

    public int Luck { get; private set; }
    public string Name { get; set; }

    public int ExperienceToNextLevel
    {
        get { return _experienceToNextLevel; }
        set
        {
            this.RaiseAndSetIfChanged(ref _experienceToNextLevel, value);
        }
    }

    public int Strength
    {
        get { return _strength; }
        set
        {
            this.RaiseAndSetIfChanged(ref _strength, value);
        }
    }

    public int Dexterity
    {
        get { return _dexterity; }
        set
        {
            this.RaiseAndSetIfChanged(ref _dexterity, value);
        }
    }

    public int Magic
    {
        get { return _magic; }
        set
        {
            this.RaiseAndSetIfChanged(ref _magic, value);
        }
    }

    public int Vitality
    {
        get { return _vitality; }
        set
        {
            this.RaiseAndSetIfChanged(ref _vitality, value);
        }
    }

    public int Mana
    {
        get { return _mana; }
        set
        {
            this.RaiseAndSetIfChanged(ref _mana, value);
        }
    }

    public void SetField(GameField field) => SetFieldInner(field);

    public bool IsNewLvl()
    {
        if (Experience != 0 && Experience == 1000 * Math.Pow(Level, 3 / 2)) 
            return true;
        return false;
    }

    public void LvlUp()
    {
        Level++;
        Strength++;
        Dexterity++;
        Vitality++;
        Magic++;
        RecalculateStats();
    }

    public void Attack()
    {
        AttackSelectedTarget();
    }

    protected override void ProcessKillTarget()
    {
        base.ProcessKillTarget();
        Experience += ((Enemy)_currentTarget).Experience;
        if (Experience >= ExperienceToLevel[Level])
        {
            LvlUp();
        }
    }

    protected override void AttackSelectedTarget()
    {
        var tiles = GetTilesAtSight();
        
        foreach (var tile in tiles)
        {
            foreach (var enemy in _field.GameObjects.OfType<Enemy>())
            {
                if (tile.CellLocation == enemy.CellLocation)
                {
                    _currentTarget = enemy; 
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

    protected void RecalculateStats()
    {
        Health = GetMaxHealth();
        Mana = GetMaxMana();
        MinDamage = GetMinDamage();
        MaxDamage = GetMaxDamage();
        ExperienceToNextLevel = ExperienceToLevel[Level];
    }

    protected int GetMinDamage()
    {
        return DEFAULT_MIN_DAMAGE + Strength * Level / 10;
    }

    protected int GetMaxDamage()
    {
        return DEFAULT_MAX_DAMAGE + Strength * Level / 5;
    }

    private int GetMaxHealth()
    {
        return DEFAULT_HP + Vitality * 2 + Level * 6;
    }

    private int GetMaxMana()
    {
        return DEFAULT_MANA + Magic + Level;
    }
}