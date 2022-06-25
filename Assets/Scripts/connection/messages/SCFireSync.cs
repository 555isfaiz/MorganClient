using System.Collections.Generic;
public class SCFireSync : MSMessageBase
{
    public bool isFire1;
    public bool isFire2;
    public BVector3 direction;
    public int f1Ammo;
    public int f2Ammo;
    public long timeStamp;

    public SCFireSync()
    {
        id = 3002;
    }

    public override void write(OutputStream output)
    {
        output.write<bool>(isFire1);
        output.write<bool>(isFire2);
        output.write<BVector3>(direction);
        output.write<int>(f1Ammo);
        output.write<int>(f2Ammo);
        output.write<long>(timeStamp);

    }

    public override void read(InputStream input)
    {
        isFire1 = input.read<bool>();
        isFire2 = input.read<bool>();
        direction = input.read<BVector3>();
        f1Ammo = input.read<int>();
        f2Ammo = input.read<int>();
        timeStamp = input.read<long>();

    }
}