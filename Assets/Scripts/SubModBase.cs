public abstract class SubModBase
{
    ModBase owner;

    public SubModBase(ModBase owner)
    {
        this.owner = owner;
    }

    public virtual void Start() {}

    public virtual void Update() {}

    public virtual void FixedUpdate() {}

    public virtual void Stop() {}

    public ModBase GetOwner()
    {
        return owner;
    }

    public virtual void OnEventGameInit(params object[] args) {}
    public virtual void OnEventGameJoin(params object[] args) {}
    public virtual void OnEventGameEnd(params object[] args) {}
    public virtual void OnEventGameQuit(params object[] args) {}
}