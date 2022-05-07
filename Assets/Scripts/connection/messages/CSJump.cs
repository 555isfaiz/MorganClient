using System.Collections.Generic;
public class CSJump : MSMessageBase
{
    public int playerId;
    public long jumpStart;

    public CSJump()
    {
        id = 2003;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(playerId);
        output.write<long>(jumpStart);

    }

    public override void read(InputStream input)
    {
        playerId = input.read<int>();
        jumpStart = input.read<long>();

    }
}