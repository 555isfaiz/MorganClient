using System.Collections.Generic;
using UnityEngine;

public abstract class ModBase
{
    MonoBehaviour owner;

    protected List<SubModBase> subMods = new List<SubModBase>();

    public ModBase(MonoBehaviour owner)
    {
        this.owner = owner;
    }

    public void Start()
    {
        foreach (var m in subMods)
        {
            m.Start();
        }

        StartOverride();
    }

    public abstract void StartOverride();

    public virtual void Update()
    {
        foreach (var m in subMods)
        {
            m.Update();
        }

        UpdateOverride();
    }

    public abstract void UpdateOverride();

    public void Stop()
    {
        foreach (var m in subMods)
        {
            m.Stop();
        }

        StopOverride();
    }

    public abstract void StopOverride();

    public MonoBehaviour GetOwner()
    {
        return owner;
    }

    public void AddSubMod(SubModBase m)
    {
        if (m == null)
        {
            return;
        }

        subMods.Add(m);
    }
}