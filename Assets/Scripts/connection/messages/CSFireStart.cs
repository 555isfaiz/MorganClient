using System.Collections.Generic;
public class CSFireStart : MSMessageBase
{
    public bool isFire1;
    public BVector3 direction;
    public long timeStamp;

    public CSFireStart()
    {
        id = 3001;
    }

    public override void write(OutputStream output)
    {
        output.write<bool>(isFire1);
        output.write<BVector3>(direction);
        output.write<long>(timeStamp);

    }

    public override void read(InputStream input)
    {
        isFire1 = input.read<bool>();
        direction = input.read<BVector3>();
        timeStamp = input.read<long>();

    }
}