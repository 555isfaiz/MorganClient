using System.Collections.Generic;
public class SCJoinGame : MSMessageBase
{
    public int sessionId;
    public int mySide;
    public List<MSMessageBase> players = new List<MSMessageBase>();  //BPlayer

    public SCJoinGame()
    {
        id = 1003;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(sessionId);
        output.write<int>(mySide);
        output.write<MSMessageBase>(players);

    }

    public override void read(InputStream input)
    {
        sessionId = input.read<int>();
        mySide = input.read<int>();
        players = input.read<List<MSMessageBase>>();

    }
}