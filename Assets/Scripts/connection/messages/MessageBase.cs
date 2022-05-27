public abstract class MSMessageBase
{
    protected int id;

    public abstract void write(OutputStream output);
    public abstract void read(InputStream input);

    public static int GetMessageId(MSMessageBase msg)
    {
        if  (msg is CSMove)
        {
            return 2001;
        }
        else if (msg is SCMove)
        {
            return 2002;
        }
        else if (msg is CSJump)
        {
            return 2003;
        }
        else if (msg is SCJump)
        {
            return 2004;
        }
        else if (msg is CSDash)
        {
            return 2005;
        }
        else if (msg is SCDashStart)
        {
            return 2006;
        }
        else if (msg is SCDashStop)
        {
            return 2007;
        }
        else if (msg is CSPlayerRotate)
        {
            return 2008;
        }
        else if (msg is BVector2)
        {
            return 102;
        }
        else if (msg is BVector3)
        {
            return 103;
        }
        else if (msg is BVector4)
        {
            return 104;
        }
        else if (msg is BPlayer)
        {
            return 101;
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
        else if (msg is SCGameSync)
        {
            return 1005;
        }

        return 0;
    }

    public static MSMessageBase GetEmptyMessageById(int id)
    {
        MSMessageBase msg = null;
        switch (id)
        {
            case 2001:
                msg = new CSMove();
                break;

            case 2002:
                msg = new SCMove();
                break;

            case 2003:
                msg = new CSJump();
                break;

            case 2004:
                msg = new SCJump();
                break;

            case 2005:
                msg = new CSDash();
                break;

            case 2006:
                msg = new SCDashStart();
                break;

            case 2007:
                msg = new SCDashStop();
                break;

            case 2008:
                msg = new CSPlayerRotate();
                break;

            case 102:
                msg = new BVector2();
                break;

            case 103:
                msg = new BVector3();
                break;

            case 104:
                msg = new BVector4();
                break;

            case 101:
                msg = new BPlayer();
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

            case 1005:
                msg = new SCGameSync();
                break;


            default:
                break;
        }
        return msg;
    }
}