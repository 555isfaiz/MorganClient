public class BVector2 : MSMessageBase
{
    public float x;
    public float z;

    public BVector2()
    {
        id = 102;
    }

    public override void write(OutputStream output)
    {
        output.write<float>(x);
        output.write<float>(z);
    }

    public override void read(InputStream input)
    {
        x = input.read<float>();
        z = input.read<float>();
    }
}