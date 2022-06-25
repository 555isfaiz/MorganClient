using System.Collections.Generic;
public class CSFireStop : MSMessageBase
{
    public bool isFire1;
    public long timeStamp;

    public CSFireStop()
    {
        id = 3003;
    }

    public override void write(OutputStream output)
    {
        output.write<bool>(isFire1);
        output.write<long>(timeStamp);

    }

    public override void read(InputStream input)
    {
        isFire1 = input.read<bool>();
        timeStamp = input.read<long>();

    }
}