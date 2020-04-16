using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class InputStream : StreamBase
{
    private int cursor = 0;

    public InputStream(){}

    public InputStream(byte[] bytes)
    {
        setBuffer(bytes);
    }

    public void resetCursor()
    {
        cursor = 0;
    }

    public T read<T>()
    {
        T result = default(T);
        try
        {
            byte firstByte = read(1)[0];
            int tag = resolveTag(firstByte);

            if (tag == TYPE_NULL)
            {

            }
            else if (tag == TYPE_INT)
            {
                int len = (firstByte & 0xF0) >> 4;
                result = (T)(object)(int)resolveNum(len);
            }
            else if (tag == TYPE_LONG)
            {
                int len = (firstByte & 0xF0) >> 4;
                result = (T)(object)resolveNum(len);
            }
            else if (tag == TYPE_FLOAT)
            {
                byte[] b = read(LENGTH_INT);
                // reverse the bytes
                for (int i = b.Length / 2; i < b.Length ; i++)
                {
                    byte t = b[i];
                    b[i] = b[i - (1 + 2 * (i - b.Length / 2))];
                    b[i - (1 + 2 * (i - b.Length / 2))] = t;
                }

                result = (T)(object)BitConverter.ToSingle(b, 0);
            }
            else if (tag == TYPE_DOUBLE)
            {
                byte[] b = read(LENGTH_LONG);
                // reverse the bytes
                for (int i = b.Length / 2; i < b.Length ; i++)
                {
                    byte t = b[i];
                    b[i] = b[i - (1 + 2 * (i - b.Length / 2))];
                    b[i - (1 + 2 * (i - b.Length / 2))] = t;
                }
                result = (T)(object)BitConverter.ToDouble(b, 0);
            }
            else if (tag == TYPE_BOOL)
            {
                result = (T)(object)((((firstByte & 0xF0) >> 4) & 1) == 1);
            }
            else if (tag == TYPE_BYTE)
            {
                result = (T)(object)read(1)[0];
            }
            else if (tag == TYPE_STRING)
            {
                int lenOflen = (firstByte & 0xF0) >> 4;
                int length = (int)resolveNum(lenOflen);

                byte[] str = read(length);
                result = (T)(object)Encoding.UTF8.GetString(str);
            }
            else if (tag == TYPE_LIST)
            {
                int lenOflen = (firstByte & 0xF0) >> 4;
                int length = (int) resolveNum(lenOflen);
                int subTag = resolveTag(buffer[cursor]);
                switch (subTag)
                {
                    case TYPE_INT:
                        List<int> li = new List<int>();
                        for (int i = 0; i < length; i++)
                        {
                            li.Add(read<int>());
                        }
                        result = (T)(object)li;
                        break;

                    case TYPE_LONG:
                        List<long> ll = new List<long>();
                        for (int i = 0; i < length; i++)
                        {
                            ll.Add(read<long>());
                        }
                        result = (T)(object)ll;
                        break;

                    case TYPE_FLOAT:
                        List<float> lf = new List<float>();
                        for (int i = 0; i < length; i++)
                        {
                            lf.Add(read<float>());
                        }
                        result = (T)(object)lf;
                        break;

                    case TYPE_DOUBLE:
                        List<double> ld = new List<double>();
                        for (int i = 0; i < length; i++)
                        {
                            ld.Add(read<double>());
                        }
                        result = (T)(object)ld;
                        break;

                    case TYPE_MESSAGE:
                        List<MSMessageBase> lm = new List<MSMessageBase>();
                        for (int i = 0; i < length; i++)
                        {
                            lm.Add(read<MSMessageBase>());
                        }
                        result = (T)(object)lm;
                        break;

                    default:
                        throw new NotSupportedException("unreadable subTag while reading a list:" + subTag);
                }
            }
            else if (tag == TYPE_MESSAGE)
            {
                int id = Utils.BytesToInt(read(LENGTH_INT));
                MSMessageBase m = MSMessageBase.GetEmptyMessageById(id);
                m.read(this);
                result = (T)(object)m;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        return (T)result;
    }

    private int resolveTag(byte b)
    {
        return b & 0xF;
    }

    private long resolveNum(int length)
    {
        byte[] bytes = read(length);

        bool isNegative = (bytes[0] & (1 << 7)) == 128;
        long num = 0L;
        for (int i = 0; i < length; i++)
        {
            if (i == 0)
                num |= (long)(bytes[i] & 0x7F) << ((length - i - 1) * 8);
            else
            {
                num |= ((long)bytes[i] & 0xFF) << ((length - i - 1) * 8);
            }
        }
        return isNegative ? -num : num;
    }

    private byte[] read(int length)
    {
        return read(cursor, length);
    }

    private byte[] read(int begin, int length)
    {
        if (begin == actualLen)
            throw new NotSupportedException("end of InputStream!");

        if ((begin + length) > actualLen)
        {
            length = actualLen - begin;
        }

        byte[] toReturn = new byte[length];
        Array.Copy(buffer, begin, toReturn, 0, length);
        if (begin == cursor)
            cursor += length;
        return toReturn;
    }
}