using System.Collections.Generic;
public class SCGameSync : MSMessageBase
{
    public int sessionId;
    public List<MSMessageBase> players = new List<MSMessageBase>();  //BPlayer

    public SCGameSync()
    {
        id = 1005;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(sessionId);
        output.write<MSMessageBase>(players);

    }

    public override void read(InputStream input)
    {
        sessionId = input.read<int>();
        players = input.read<List<MSMessageBase>>();

    }
}