using System;
using System.Collections.Generic;
public class MSSimpleExecutor
{
    List<MSTask> tasks = new List<MSTask>();
    public void Update(long now) 
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            var t = tasks[i];
            if (now > t.delay)
            {
                t.func();
                tasks.Remove(t);
                i--;
            }
        }
    }

    public void Add(MSTask t)
    {
        tasks.Add(t);
    }
}

public class MSTask 
{
    public long delay;
    public Action func;
}