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

    public virtual void StartOverride() {}

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

    public virtual void UpdateOverride() {}

    public void Stop()
    {
        StopOverride();

        foreach (var p in subMods)
        {
            p.Value.Stop();
        }
    }

    public virtual void StopOverride() {}

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

    protected virtual void OnEventGameInit(params object[] args) {}

    public void FireGameInit(params object[] args)
    {
        OnEventGameInit(args);

        foreach (var p in subMods)
        {
            p.Value.OnEventGameInit(args);
        }
    }

    protected virtual void OnEventGameJoin(params object[] args) {}

    public void FireGameJoin(params object[] args) 
    {
        OnEventGameJoin(args);

        foreach (var p in subMods)
        {
            p.Value.OnEventGameJoin(args);
        }
    }

    protected virtual void OnEventGameEnd(params object[] args) {}

    public void FireGameEnd(params object[] args) 
    {
        OnEventGameEnd(args);

        foreach (var p in subMods)
        {
            p.Value.OnEventGameEnd(args);
        }
    }

    protected virtual void OnEventGameQuit(params object[] args) {}

    public void FireGameQuit(params object[] args) 
    {
        OnEventGameQuit(args);
        
        foreach (var p in subMods)
        {
            p.Value.OnEventGameQuit(args);
        }
    }
}