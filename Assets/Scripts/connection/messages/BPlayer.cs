public class BPlayer : MSMessageBase
{
    public int playerId;
    public string playerName;
    public int side;
    public BVector2 initPos;

    public BPlayer()
    {
        id = 101;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(playerId);
        output.write<string>(playerName);
        output.write<int>(side);
        output.write<BVector2>(initPos);
    }

    public override void read(InputStream input)
    {
        playerId = input.read<int>();
        playerName = input.read<string>();
        side = input.read<int>();
        initPos = input.read<BVector2>();
    }
}