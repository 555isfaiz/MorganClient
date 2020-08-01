using System.Collections.Generic;
public class SCDashStart : MSMessageBase
{
    public int playerId;
    public BVector3 direction;

    public SCDashStart()
    {
        id = 2006;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(playerId);
        output.write<BVector3>(direction);

    }

    public override void read(InputStream input)
    {
        playerId = input.read<int>();
        direction = input.read<BVector3>();

    }
}