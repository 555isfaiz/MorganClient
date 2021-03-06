using System.Collections.Generic;
public class CSLogin : MSMessageBase
{
    public bool isShooter;

    public CSLogin()
    {
        id = 1001;
    }

    public override void write(OutputStream output)
    {
        output.write<bool>(isShooter);

    }

    public override void read(InputStream input)
    {
        isShooter = input.read<bool>();
    }
}