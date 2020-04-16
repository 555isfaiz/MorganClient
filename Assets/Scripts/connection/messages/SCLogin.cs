public class SCLogin : MSMessageBase
{
    public int playerId;

    public SCLogin()
    {
        id = 1004;
    }

    public override void write(OutputStream output)
    {
        output.write<int>(id);
    }

    public override void read(InputStream input)
    {
        id = input.read<int>();
    }
}