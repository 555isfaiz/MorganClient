public abstract class SubModBase
{
    ModBase owner;

    public SubModBase(ModBase owner)
    {
        this.owner = owner;
    }

    public abstract void Start();

    public abstract void Update();

    public abstract void Stop();

    public ModBase GetOwner()
    {
        return owner;
    }
}