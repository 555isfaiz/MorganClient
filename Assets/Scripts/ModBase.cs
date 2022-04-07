using System.Collections.Generic;
using UnityEngine;

public abstract class ModBase
{
    MonoBehaviour owner;

    protected Dictionary<string, SubModBase> subMods = new Dictionary<string, SubModBase>();

    public ModBase(MonoBehaviour owner)
    {
        this.owner = owner;
    }

    public void Start()
    {
        StartOverride();

        foreach (var p in subMods)
        {
            p.Value.Start();
        }
    }

    public abstract void StartOverride();

    public virtual void Update()
    {
        UpdateOverride();

        foreach (var p in subMods)
        {
            p.Value.Update();
        }
    }
    
    public virtual void FixedUpdate()
    {
        FixedUpdateOverride();

        foreach (var p in subMods)
        {
            p.Value.FixedUpdate();
        }
    }

    public virtual void FixedUpdateOverride() {}

    public abstract void UpdateOverride();

    public void Stop()
    {
        StopOverride();

        foreach (var p in subMods)
        {
            p.Value.Stop();
        }
    }

    public abstract void StopOverride();

    public MonoBehaviour GetOwner()
    {
        return owner;
    }

    public void AddSubMod(string name, SubModBase m)
    {
        if (m == null)
        {
            return;
        }

        subMods.Add(name, m);
    }

    public SubModBase GetSubMod(string name) 
    {
        SubModBase subMod;
        subMods.TryGetValue(name, out subMod);
        return subMod;
    } 

    protected virtual void OnEventGameInit() {}

    protected void FireGameInit()
    {
        OnEventGameInit();

        foreach (var p in subMods)
        {
            p.Value.OnEventGameInit();
        }
    }

    protected virtual void OnEventGameJoin() {}

    protected void FireGameJoin() 
    {
        OnEventGameJoin();

        foreach (var p in subMods)
        {
            p.Value.OnEventGameJoin();
        }
    }

    protected virtual void OnEventGameEnd() {}

    protected void FireGameEnd() 
    {
        OnEventGameEnd();

        foreach (var p in subMods)
        {
            p.Value.OnEventGameEnd();
        }
    }

    protected virtual void OnEventGameQuit() {}

    protected void FireGameQuit() 
    {
        OnEventGameQuit();
        
        foreach (var p in subMods)
        {
            p.Value.OnEventGameQuit();
        }
    }
}