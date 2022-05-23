using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public abstract class ModBase
{
    MonoBehaviour owner;
    string ModName;

    protected Dictionary<string, SubModBase> subMods = new Dictionary<string, SubModBase>();

    public ModBase(MonoBehaviour owner, string modName)
    {
        this.owner = owner;
        this.ModName = modName;
    }

    // Remember to call Start() for every Mods!!!!!!
    public void Start()
    {
        StartOverride();

        MSMain.AddListeners(this.CollectEventListeners());

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

    public Dictionary<string, List<MSEventListener>> CollectEventListeners()
    {
        Dictionary<string, List<MSEventListener>> collection = new Dictionary<string, List<MSEventListener>>();
        Type t = this.GetType();
        foreach (MethodInfo m in t.GetMethods())
        {
            AttrModEvent attr = m.GetCustomAttribute<AttrModEvent>();
            if (attr == null) continue;

            string eventName = attr.GetEventName();
            Action<Param> func = (Action<Param>)Delegate.CreateDelegate(typeof(Action<Param>), this, m);

            MSEventListener listener = new MSEventListener(eventName, ModName, func);

            List<MSEventListener> list;
            if (collection.TryGetValue(eventName, out list))
            {
                list.Add(listener);
            }
            else 
            {
                list = new List<MSEventListener>();
                list.Add(listener);
                collection.Add(eventName, list);
            }
        }

        foreach (var pair in subMods)
        {
            var dict = pair.Value.CollectEventListeners();
            foreach (var ppair in dict)
            {
                List<MSEventListener> list;
                if (collection.TryGetValue(ppair.Key, out list))
                {
                    list.Add(ppair.Value);
                }
                else 
                {
                    list = new List<MSEventListener>();
                    list.Add(ppair.Value);
                    collection.Add(ppair.Key, list);
                }
            }
        }
        return collection;
    }
}