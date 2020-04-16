public class CSLogin : MSMessageBase
{
    public string playerName;

    public CSLogin()
    {
        id = 1001;
    }

    public override void write(OutputStream output)
    {
        output.write<string>(playerName);
    }

    public override void read(InputStream input)
    {
        playerName = input.read<string>();
    }
}