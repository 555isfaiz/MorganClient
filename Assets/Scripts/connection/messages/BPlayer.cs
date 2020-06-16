using System.Collections.Generic;
public class BPlayer : MSMessageBase
{
    public int playerId;
    public string playerName;
    public int side;
    public BVector3 curPos;

    public BPlayer()
    {
        id = 101;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(playerId);
        output.write<string>(playerName);
        output.write<int>(side);
        output.write<BVector3>(curPos);

    }

    public override void read(InputStream input)
    {
        playerId = input.read<int>();
        playerName = input.read<string>();
        side = input.read<int>();
        curPos = input.read<BVector3>();

    }
}