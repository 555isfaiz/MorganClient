using System.Collections.Generic;
using System;
using System.Reflection;

public abstract class SubModBase
{
    ModBase owner;
    string ModName;

    public SubModBase(ModBase owner, string modName)
    {
        this.owner = owner;
        ModName = modName;
    }

    public virtual void Start() {}

    public virtual void Update() {}

    public virtual void FixedUpdate() {}

    public virtual void Stop() {}

    public ModBase GetOwner()
    {
        return owner;
    }

    public Dictionary<string, MSEventListener> CollectEventListeners()
    {
        Dictionary<string, MSEventListener> collection = new Dictionary<string, MSEventListener>();
        Type t = this.GetType();
        foreach (MethodInfo m in t.GetMethods())
        {
            AttrModEvent attr = m.GetCustomAttribute<AttrModEvent>();
            if (attr == null) continue;

            string eventName = attr.GetEventName();
            Action<Param> func = (Action<Param>)Delegate.CreateDelegate(typeof(Action<Param>), this, m);

            MSEventListener listener = new MSEventListener(eventName, ModName, func);
            collection.Add(eventName, listener);
        }
        return collection;
    }
}