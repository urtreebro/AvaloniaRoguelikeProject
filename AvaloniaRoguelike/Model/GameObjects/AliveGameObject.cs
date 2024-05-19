using ReactiveUI;
using System;

namespace AvaloniaRoguelike.Model;

public abstract class AliveGameObject : MovingGameObject, IAlive
{
    protected AliveGameObject(
        GameField field,
        CellLocation location,
        Facing facing) 
        : base(field, location, facing) 
    { }

    public int Damage
    {
        get;
        protected set;
    }

    public double Speed
    {
        get;
        protected set;
    }

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
            _targetMovingGameObject = _field.Player;
            //var path = _pathFindingService.FindPath(_field, CellLocation, TargetCellLocation);
            //if (path == null)
            //{
            //    // TODO: MakarovEA, log.Debug?
            //    return;
            //}
            return;
        }
        else if (isEnemyInRange && canAttackEnemy)
        {
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
        if (_targetMovingGameObject is null)
        {
            return false;
        }
        return IsInRange(_targetMovingGameObject.CellLocation, _attackRadius);
    }

    protected virtual void AttackSelectedTarget()
    {
        if (_targetMovingGameObject.Health > 0)
        {
            _targetMovingGameObject.Health -= Damage;
        }
    }
}