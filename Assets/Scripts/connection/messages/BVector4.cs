using System.Collections.Generic;
public class BVector4 : MSMessageBase
{
    public float x;
    public float y;
    public float z;
    public float w;

    public BVector4()
    {
        id = 104;
    }

    public override void write(OutputStream output)
    {
        output.write<float>(x);
        output.write<float>(y);
        output.write<float>(z);
        output.write<float>(w);

    }

    public override void read(InputStream input)
    {
        x = input.read<float>();
        y = input.read<float>();
        z = input.read<float>();
        w = input.read<float>();

    }
}