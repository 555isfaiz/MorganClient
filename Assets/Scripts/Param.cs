using System.Collections.Generic;
using System;

public class Param
{
    Dictionary<string, object> objs = new Dictionary<string, object>();

    public Param(params object[] obj)
    {
        if (obj.Length % 2 != 0)
        {
            throw new ArgumentException("Use Param constructor with even number of arguements");
        }

        try
        {
            for (int i = 0; i < obj.Length; i += 2)
            {
                string name = (string) obj[i];
                objs.Add(name, obj[i + 1]);
            }
        }
        catch (System.InvalidCastException e)
        {
            throw e;
        }
    }

    public void Set(string name, object obj)
    {
        objs.Add(name, obj);
    }

    public void Get(string name, out object obj)
    {
        objs.TryGetValue(name, out obj);
    }

    public Dictionary<string, object> GetOjbs()
    {
        return objs;
    }
}