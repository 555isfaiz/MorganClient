using System.Collections.Generic;
public class CSDash : MSMessageBase
{
    public BVector3 direction;
    public long duration;

    public CSDash()
    {
        id = 2005;
    }

    public override void write(OutputStream output)
    {
        output.write<BVector3>(direction);
        output.write<long>(duration);

    }

    public override void read(InputStream input)
    {
        direction = input.read<BVector3>();
        duration = input.read<long>();

    }
}