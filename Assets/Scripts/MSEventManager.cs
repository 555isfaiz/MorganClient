using System.Collections.Generic;
using System;

public class MSEventManager
{
    Dictionary<string, List<MSEventListener>> listeners = new Dictionary<string, List<MSEventListener>>();

    public void AddListeners(Dictionary<string, MSEventListener> toAdd)
    {
        foreach (var pair in toAdd)
        {
            List<MSEventListener> list;
            if (listeners.TryGetValue(pair.Key, out list))
            {
                list.Add(pair.Value);
            }
            else 
            {
                list = new List<MSEventListener>();
                list.Add(pair.Value);
                listeners.Add(pair.Key, list);
            }
        }
    }

    public void AddListeners(Dictionary<string, List<MSEventListener>> toAdd)
    {
        foreach (var pair in toAdd)
        {
            List<MSEventListener> list;
            if (listeners.TryGetValue(pair.Key, out list))
            {
                list.AddRange(pair.Value);
            }
            else 
            {
                list = new List<MSEventListener>();
                list.AddRange(pair.Value);
                listeners.Add(pair.Key, list);
            }
        }
    }

    public void FireEvent(string EventName, string ModName, Param param)
    {
        List<MSEventListener> list;
        if (listeners.TryGetValue(EventName, out list))
        {
            foreach (var l in list)
            {
                if (ModName != null && !ModName.Equals(l.ModName_)) continue;
                l.Invoke(param);
            }
        }
    }

    public void FireEvent(string EventName, Param param)
    {
        FireEvent(EventName, null, param);
    }
}