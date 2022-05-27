using System.Collections.Generic;
public class CSPlayerRotate : MSMessageBase
{
    public int playerId;
    public BVector4 rotation;
    public long timeStamp;

    public CSPlayerRotate()
    {
        id = 2008;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(playerId);
        output.write<BVector4>(rotation);
        output.write<long>(timeStamp);

    }

    public override void read(InputStream input)
    {
        playerId = input.read<int>();
        rotation = input.read<BVector4>();
        timeStamp = input.read<long>();

    }
}