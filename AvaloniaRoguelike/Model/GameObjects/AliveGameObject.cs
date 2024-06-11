using ReactiveUI;

using System;

namespace AvaloniaRoguelike.Model;

public abstract class AliveGameObject : MovingGameObject, IAlive
{
    private int _health;
    private int _experience;
    private int _level;
    private int _ac;
    private int _toHit;
    private int _minDamage;
    private int _maxDamage;
    private int _attackRadius = 1;
    protected AliveGameObject _currentTarget;

    protected AliveGameObject(
        GameField field,
        CellLocation location,
        Facing facing) 
        : base(field, location, facing) 
    { }

    protected AliveGameObject(
        CellLocation location,
        Facing facing)
        : base(location, facing)
    { }

    public double Speed
    {
        get;
        protected set;
    }

    public int Level
    {
        get { return _level; }
        set
        {
            this.RaiseAndSetIfChanged(ref _level, value);
        }
    }

    public int Experience
    {
        get { return _experience; }
        set
        {
            this.RaiseAndSetIfChanged(ref _experience, value);
        }
    }

    public int Health
    {
        get { return _health; }
        set
        {
            this.RaiseAndSetIfChanged(ref _health, value);
        }
    }

    public int MinDamage
    {
        get { return _minDamage; }
        set
        {
            this.RaiseAndSetIfChanged(ref _minDamage, value);
        }
    }

    public int MaxDamage
    {
        get { return _maxDamage; }
        set
        {
            this.RaiseAndSetIfChanged(ref _maxDamage, value);
        }
    }

    public int ArmorClass
    {
        get { return _ac; }
        set
        {
            this.RaiseAndSetIfChanged(ref _ac, value);
        }
    }

    public int ToHit
    {
        get { return _toHit; }
        set
        {
            this.RaiseAndSetIfChanged(ref _toHit, value);
        }
    }

    protected virtual void ProcessKillTarget()
    { }

    public override void DoMainLogicEachGameTick()
    {
        // Получить все объекты в поле зрения
        var tiles = GetTilesAtSight();
        // Если среди них есть противник (игрок) - идти к игроку
        var isEnemyInRange = CheckIfEnemyInRange(tiles);
        // Если игрок на соседней клетке или достижим атакой с текущей - атаковать с заданной частотой
        var canAttackEnemy = CheckCanAttackEnemy();

        // противник в зоне видимости, но нельзя атаковать - пытаться идти к нему
        // противник в зоне видимости и можно его атаковать - атаковать
        if (isEnemyInRange && !canAttackEnemy)
        {
            // идти к цели
            if (!IsMoving)
            {
                SetTarget(_field.Player.CellLocation);
            }
            //TargetCellLocation = _field.Player.CellLocation;
            _currentTarget = _field.Player;
            return;
        }
        else if (isEnemyInRange && canAttackEnemy)
        {
            _currentTarget = _field.Player;
            // атаковать цель
            if (DateTime.Now.TimeOfDay - _lastAttackTime > _attackCooldown)
            {
                _lastAttackTime = DateTime.Now.TimeOfDay;
                AttackSelectedTarget();
            }
            return;
        }
        else
        {
            _currentTarget = null;
            _targetMovingGameObject = null;
            // Иначе случайно бродить
            if (!IsMoving)
            {
                SetTarget(GetRandomFacing());
            }
        }
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    protected bool CheckCanAttackEnemy()
    {
        if (_currentTarget is null)
        {
            return false;
        }
        return IsInRange(_currentTarget.CellLocation, _attackRadius);
    }

    protected virtual void AttackSelectedTarget()
    {
        if (_currentTarget.Health > 0)
        {
            var damage = Random.Shared.Next(MinDamage, MaxDamage + 1);
            _currentTarget.Health = Math.Max(_currentTarget.Health - damage, 0);
            if (_currentTarget.Health == 0)
            {
                ProcessKillTarget();
            }
        }
    }
}