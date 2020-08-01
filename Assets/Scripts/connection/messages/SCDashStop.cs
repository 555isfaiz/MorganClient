using System.Collections.Generic;
public class SCDashStop : MSMessageBase
{
    public int playerId;
    public BVector3 finalPos;

    public SCDashStop()
    {
        id = 2007;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(playerId);
        output.write<BVector3>(finalPos);

    }

    public override void read(InputStream input)
    {
        playerId = input.read<int>();
        finalPos = input.read<BVector3>();

    }
}