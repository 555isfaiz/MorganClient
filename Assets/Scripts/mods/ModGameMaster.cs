using System.Collections.Generic;
using UnityEngine;

public class ModGameMaster : ModBase
{
    int id;
    int teamMateId;
    int lastGameSession;
    int currentLock;
    GameObject mainPlayer;
    Dictionary<int, GameObject> otherPlayer = new Dictionary<int, GameObject>();

    public ModGameMaster(MonoBehaviour owner, GameObject mainPlayer) : base(owner)
    {
        this.mainPlayer = mainPlayer;
    }

    public override void StartOverride(){}

    public override void UpdateOverride()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            switchLock();
        }
    }

    public override void StopOverride(){}

    public void SetMyId(int id)
    {
        this.id = id;
    }

    public void NewGame(int myside, int lastGameSession, List<BPlayer> otherPlayers)
    {
        this.lastGameSession = lastGameSession;
        GameObject go =  Resources.Load("OtherPlayer") as GameObject;
        foreach (var p in otherPlayers)
        {
            if (p.side == myside)
            {
                teamMateId = p.playerId;
            }
            var tmp = GameObject.Instantiate(go);
            tmp.transform.position = new Vector3(p.initPos.x, 5f, p.initPos.z);
            this.otherPlayer.Add(p.playerId, tmp);
        }
        mainPlayer.transform.position = new Vector3(6, 5f, -6);
        switchLock();
    }

    public void switchLock()
    {
        int toLock = 0;
        foreach (var e in otherPlayer)
        {
            if (e.Key == currentLock)
            {
                continue;
            }

            if (e.Key == teamMateId)
            {
                continue;
            }

            toLock = e.Key;
        }

        MSCamera c = (MSCamera)GetOwner();
        if (toLock == 0)
        {
            c.locked = false;
        } 
        else
        {
            c.locked = true;
        }
        currentLock = toLock;
        c.QuickSwitch(GetTargetPos());
    }

    public Vector3 GetTargetPos()
    {
        if (currentLock == 0)
        {
            return new Vector3();
        }

        GameObject tmp;
        if (!otherPlayer.TryGetValue(currentLock, out tmp))
        {
            return new Vector3();
        }

        return tmp.transform.position;
    }

    public GameObject GetTargetObject()
    {
        GameObject tmp;
        if (!otherPlayer.TryGetValue(currentLock, out tmp))
        {
            return null;
        }

        return tmp;
    }
}