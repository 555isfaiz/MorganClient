public abstract class SubModBase
{
    ModBase owner;

    public SubModBase(ModBase owner)
    {
        this.owner = owner;
    }

    public abstract void Start();

    public abstract void Update();

    public virtual void FixedUpdate() {}

    public abstract void Stop();

    public ModBase GetOwner()
    {
        return owner;
    }

    public virtual void OnEventGameInit() {}
    public virtual void OnEventGameJoin() {}
    public virtual void OnEventGameEnd() {}
    public virtual void OnEventGameQuit() {}
}