using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OutputStream : StreamBase
{
    public OutputStream() : this(DEFAULT_BUFFER_SIZE) { }

    public OutputStream(int size)
    {
        setBufferSize(size);
    }

    private byte[] resolveNumber(long i)
    {
        if (i == 0)
            return new byte[1];
        long absI = i < 0 ? -i : i;
        long div = ~long.MaxValue;
        int index = 0;

        while ((absI & div) == 0)
        {
            div = div >> 1;
            if (index == 0)
                div = div ^ (~long.MaxValue);
            index++;
        }

        int len = 8 - (index % 8 == 0 ? (index / 8 - 1) : (index / 8));
        byte[] res = new byte[len];
        for (int j = 0; j < len; j++)
        {
            res[j] = (byte)(absI >> (8 * (len - j - 1)));
        }
        if (i < 0)
        {
            res[0] |= (byte)128;
        }
        return res;
    }

    public void write<T>(object obj)
    {
        try
        {
            writeOut<T>(obj);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private void writeOut<T>(object obj)
    {
        if (obj == null)
        {
            write(new byte[] { (byte)TYPE_NULL }); return;
        }

        if (obj is int)
        {

            byte[] bytes = resolveNumber((int)obj);
            writeCompressedTL(TYPE_INT, bytes.Length);
            write(bytes);

        }
        else if (obj is long)
        {

            byte[] bytes = resolveNumber((long)obj);
            writeCompressedTL(TYPE_LONG, bytes.Length);
            write(bytes);

        }
        else if (obj is byte)
        {

            write(new byte[] { (byte)TYPE_BYTE, (byte)obj });

        }
        else if (obj is bool)
        {

            write(new byte[] { (byte)((((bool)obj ? 1 : 0) << 4) | TYPE_BOOL) });

        }
        else if (obj is float)
        {

            write(new byte[] { (byte)TYPE_FLOAT });
            byte[] b = BitConverter.GetBytes((float)obj);
            // reverse the bytes
            for (int i = b.Length / 2; i < b.Length ; i++)
            {
                byte t = b[i];
                b[i] = b[i - (1 + 2 * (i - b.Length / 2))];
                b[i - (1 + 2 * (i - b.Length / 2))] = t;
            }
            write(b);

        }
        else if (obj is double)
        {

            write(new byte[] { (byte)TYPE_DOUBLE });
            byte[] b = BitConverter.GetBytes((double)obj);
            // reverse the bytes
            for (int i = b.Length / 2; i < b.Length ; i++)
            {
                byte t = b[i];
                b[i] = b[i - (1 + 2 * (i - b.Length / 2))];
                b[i - (1 + 2 * (i - b.Length / 2))] = t;
            }
            write(b);

        }
        else if (obj is string)
        {

            byte[] bytes = Encoding.UTF8.GetBytes((string)obj);
            byte[] bNum = resolveNumber(bytes.Length);
            writeTandVL(TYPE_STRING, bNum.Length, bNum);
            write(bytes);

        }
        else if (obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
        {

            IList<T> l = (IList<T>)obj;
            byte[] lenB = resolveNumber(l.Count);
            writeTandVL(TYPE_LIST, lenB.Length, lenB);
            foreach (var v in l)
            {
                writeOut<T>(v);
            }

        }
        else if (obj is MSMessageBase)
        {

            writeTandId(TYPE_MESSAGE, MSMessageBase.GetMessageId((MSMessageBase)obj));
            ((MSMessageBase)obj).write(this);

        }
        else
        {
            throw new NotSupportedException("Unserializable class: " + obj.GetType());
        }
    }

    private void writeCompressedTL(int tag, int length)
    {
        write(new byte[] { (byte)((length << 4) | tag) });
    }

    /*
    * Message, DistrClass
    */
    private void writeTandId(int tag, int id)
    {
        byte[] bytes = new byte[5];
        bytes[0] = (byte)tag;
        byte[] idB = Utils.IntToBytes(id);

        for (int i = 1; i < 5; i++)
            bytes[i] = idB[i - 1];

        write(bytes);
    }

    /*
    * list, map, string
    * */
    private void writeTandVL(int tag, int length, byte[] lenBytes)
    {
        writeCompressedTL(tag, length);
        write(lenBytes);
    }

    private void write(byte[] bytes)
    {
        write(actualLen, bytes);
    }

    private void write(int begin, byte[] bytes)
    {
        if (begin + bytes.Length >= buffer.Length)
        {
            setBufferSize(buffer.Length << 1);
        }

        Array.Copy(bytes, 0, buffer, begin, bytes.Length);

        if ((begin + bytes.Length) > actualLen)
            actualLen = begin + bytes.Length;
    }
}