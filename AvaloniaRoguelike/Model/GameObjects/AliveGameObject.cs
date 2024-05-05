namespace AvaloniaRoguelike.Model;

public abstract class AliveGameObject : MovingGameObject, IAlive
{
    protected AliveGameObject(
        GameField field,
        CellLocation location,
        Facing facing,
        int hp,
        int attack,
        double speed) 
        : base(field, location, facing) 
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
    protected double Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public bool IsAlive()
    {
        return HP > 0;
    }
}
