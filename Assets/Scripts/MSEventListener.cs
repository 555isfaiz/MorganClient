using System;

public class MSEventListener
{
    public string EventName_;
    public string ModName_;
    public Action<Param> Func_;

    public MSEventListener(string EventName, string ModName, Action<Param> func)
    {
        EventName_ = EventName;
        ModName_ = ModName;
        Func_ = func;
    }

    public void Invoke(Param param)
    {
        Func_.Invoke(param);
    }
}