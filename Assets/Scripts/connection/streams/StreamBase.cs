using System;
public abstract class StreamBase
{
    protected const int TYPE_OBJECT = 0;
    protected const int TYPE_INT = 1;      //int32
    protected const int TYPE_LONG = 2;      //int64
    protected const int TYPE_FLOAT = 3;
    protected const int TYPE_DOUBLE = 4;
    protected const int TYPE_BOOL = 5;
    protected const int TYPE_BYTE = 6;
    protected const int TYPE_STRING = 7;
    protected const int TYPE_LIST = 8;
    protected const int TYPE_MESSAGE = 11;
    protected const int TYPE_NULL = 13;
    protected const int TYPE_ARRAY = 14;

    protected static int LENGTH_INT = 4;
    protected static int LENGTH_LONG = 8;

    public static int DEFAULT_BUFFER_SIZE = 512;
    public static int MAXIMUM_BUFFER_SIZE = 50 * 1024;

    protected byte[] buffer{set; get;}
    protected int actualLen;

    public byte[] getBuffer()
    {
        byte[] toReturn = new byte[actualLen];
        Array.Copy(buffer, 0, toReturn, 0, actualLen);
        return toReturn;
    }

    public void setBuffer(byte[] buf)
    {
        this.buffer = buf;
        actualLen = buf.Length;
    }

    public int getBufferSize()
    {
        return actualLen;
    }

    public void setBufferSize(int size)
    {
        if (size < 0)
            throw new NotSupportedException("size can't be negative: " + size);
        if (size > MAXIMUM_BUFFER_SIZE)
            throw new NotSupportedException("buffer overflowed! size:" + size);

        if (buffer == null)
        {
            buffer = new byte[size];
            return;
        }

        if (size < getBufferSize())
        {
            throw new NotSupportedException("can't set a smaller buffer, if you want to reset buffer, call reset()");
        }
        else if (size > getBufferSize())
        {
            byte[] tmp = new byte[size];
            Array.Copy(buffer, 0, tmp, 0, buffer.Length);
            buffer = tmp;
        }
    }

    public void reset()
    {
        buffer = new byte[DEFAULT_BUFFER_SIZE];
        actualLen = 0;
    }
}