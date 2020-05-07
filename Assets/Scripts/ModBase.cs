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
}