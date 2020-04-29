public abstract class MSMessageBase
{
    protected int id;

    public abstract void write(OutputStream output);
    public abstract void read(InputStream input);

    public static int GetMessageId(MSMessageBase msg)
    {
        if  (msg is BPlayer)
        {
            return 101;
        }
        else if (msg is BVector2)
        {
            return 102;
        }
        else if (msg is BVector3)
        {
            return 103;
        }
        else if (msg is CSLogin)
        {
            return 1001;
        }
        else if (msg is SCJoinGame)
        {
            return 1003;
        }
        else if (msg is SCLogin)
        {
            return 1004;
        }

        return 0;
    }

    public static MSMessageBase GetEmptyMessageById(int id)
    {
        MSMessageBase msg = null;
        switch (id)
        {
            case 101:
                msg = new BPlayer();
                break;

            case 102:
                msg = new BVector2();
                break;

            case 103:
                msg = new BVector3();
                break;

            case 1001:
                msg = new CSLogin();
                break;

            case 1003:
                msg = new SCJoinGame();
                break;

            case 1004:
                msg = new SCLogin();
                break;


            default:
                break;
        }
        return msg;
    }
}