using System.Collections.Generic;
public class BVector2 : MSMessageBase
{
    public float x;
    public float y;

    public BVector2()
    {
        id = 102;
    }

    public override void write(OutputStream output)
    {
        output.write<float>(x);
        output.write<float>(y);

    }

    public override void read(InputStream input)
    {
        x = input.read<float>();
        y = input.read<float>();

    }
}