[AttributeUsage(AttributeTargets.method)]
public class AttrModEvent : Attribute
{
    private string EventName;

    public AttrModEvent(string e) 
    {
        EventName = e;
    }

    public string GetEventName()
    {
        return EventName;
    }
}