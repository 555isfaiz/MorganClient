public class SCLogin : MSMessageBase
{
    public int playerId;

    public SCLogin()
    {
        id = 1004;
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