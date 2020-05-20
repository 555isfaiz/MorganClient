using System.Collections.Generic;
public class SCJump : MSMessageBase
{
    public int playerId;

    public SCJump()
    {
        id = 2004;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(playerId);

    }

    public override void read(InputStream input)
    {
        playerId = input.read<int>();

    }
}