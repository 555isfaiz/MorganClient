using System.Collections.Generic;
public class SCMove : MSMessageBase
{
    public int playerId;
    public BVector3 curPos;
    public BVector3 direction;
    public long timeStamp;

    public SCMove()
    {
        id = 2002;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(playerId);
        output.write<BVector3>(curPos);
        output.write<BVector3>(direction);
        output.write<long>(timeStamp);

    }

    public override void read(InputStream input)
    {
        playerId = input.read<int>();
        curPos = input.read<BVector3>();
        direction = input.read<BVector3>();
        timeStamp = input.read<long>();

    }
}