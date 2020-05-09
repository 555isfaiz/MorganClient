using System;
using UnityEngine;

public class Utils
{
    public static byte[] IntToBytes(int data)
    {
        byte[] bytes = new byte[4];
        bytes[3] = (byte)(data & 0xff);
        bytes[2] = (byte)((data & 0xff00) >> 8);
        bytes[1] = (byte)((data & 0xff0000) >> 16);
        bytes[0] = (byte)((data & 0xff000000) >> 24);
        return bytes;
    }

    public static byte[] LongToBytes(long data)
    {
        byte[] bytes = new byte[8];
        bytes[7] = (byte)(data & 0xff);
        bytes[6] = (byte)((data & 0xff00) >> 8);
        bytes[5] = (byte)((data & 0xff0000) >> 16);
        bytes[4] = (byte)((data & 0xff000000) >> 24);
        long t = data >> 32;
        bytes[3] = (byte)((t & 0xff));
        bytes[2] = (byte)((t & 0xff00) >> 8);
        bytes[1] = (byte)((t & 0xff0000) >> 16);
        bytes[0] = (byte)((t & 0xff000000) >> 24);
        return bytes;
    }

    public static int BytesToInt(byte[] bytes)
    {
        if (bytes.Length < 4)
        {
            throw new NotSupportedException("bytes length of a int must be 4!");
        }

        return (bytes[0] & 0xFF) << 24 | (bytes[1] & 0xFF) << 16 | (bytes[2] & 0xFF) << 8 | bytes[3] & 0xFF;
    }

    public static long BytesToLong(byte[] bytes)
    {
        if (bytes.Length < 8)
        {
            throw new NotSupportedException("bytes length of a long must be 8!");
        }

        return (((long)bytes[0] & 0xFF) << 56) |
                (((long)bytes[1] & 0xFF) << 48) |
                (((long)bytes[2] & 0xFF) << 40) |
                (((long)bytes[3] & 0xFF) << 32) |
                (((long)bytes[4] & 0xFF) << 24) |
                (((long)bytes[5] & 0xFF) << 16) |
                (((long)bytes[6] & 0xFF) << 8) |
                ((long)bytes[7] & 0xFF);
    }

    public static float K(Vector2 v1, Vector2 v2)
    {
        if (v1.x == v2.x)
        {
            return float.MaxValue;
        }
        return (v2.y - v1.y) / (v2.x - v1.x);
    }

    public static Vector3 Unitlize(Vector3 v)
    {
        float l = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2));
        return new Vector3(v.x / l, v.y / l, v.z / l);
    }

    public static long GetTimeMilli()
    {
        long currentTicks = DateTime.Now.Ticks;
        DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return (currentTicks - dtFrom.Ticks) / 10000;
    }
}