namespace AvaloniaRoguelike.Model;

public abstract class AliveGameObject : MovingGameObject, IAlive
{
    protected AliveGameObject(
        GameField field,
        CellLocation location,
        Facing facing) 
        : base(field, location, facing) 
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

    public double Speed
    {
        get;
        protected set;
    }

    public bool IsAlive()
    {
        return HP > 0;
    }
}