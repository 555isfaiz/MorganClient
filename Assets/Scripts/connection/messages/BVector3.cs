public class BVector3 : MSMessageBase
{
    public float x;
    public float y;
    public float z;

    public BVector3()
    {
        id = 103;
    }

    public override void write(OutputStream output)
    {
        output.write<float>(x);
        output.write<float>(y);
        output.write<float>(z);
    }

    public override void read(InputStream input)
    {
        x = input.read<float>();
        y = input.read<float>();
        z = input.read<float>();
    }
}