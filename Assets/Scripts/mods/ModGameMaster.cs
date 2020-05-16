using System.Collections.Generic;
using UnityEngine;

public class ModGameMaster : ModBase
{
    public int mainPlayerId;
    public int teamMateId;
    int lastGameSession;
    int currentLock;
    GameObject mainPlayer;
    Dictionary<int, GameObject> otherPlayer = new Dictionary<int, GameObject>();
    MSHero msHero;
    Dictionary<int, MSOtherPlayer> msOther = new Dictionary<int, MSOtherPlayer>();
    
    public ModGameMaster(MonoBehaviour owner, GameObject mainPlayer) : base(owner)
    {
        this.mainPlayer = mainPlayer;
        msHero = mainPlayer.GetComponent<MSHero>();
    }

    public override void StartOverride()
    {
        GameObject canvas = GameObject.Find("Canvas");
        var modUI = new ModUIs(this, canvas);
        subMods.Add("ModUIs", modUI);
    }

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
        mainPlayerId = id;
        MSShare.mainPlayerId = id;
    }

    public void NewGame(int myside, int lastGameSession, List<BPlayer> players)
    {
        this.lastGameSession = lastGameSession;
        MSShare.currentSessionId = lastGameSession;
        GameObject go =  Resources.Load("OtherPlayer") as GameObject;
        foreach (var p in players)
        {
            if (p.side == myside && p.playerId != mainPlayerId)
            {
                teamMateId = p.playerId;
            }

            if (p.playerId == mainPlayerId)
            {
                mainPlayer.transform.position = new Vector3(p.initPos.x, 5f, p.initPos.y);
                msHero.id = mainPlayerId;
                msHero.playerName = p.playerName;
            }
            else
            {
                var tmp = GameObject.Instantiate(go);
                tmp.transform.position = new Vector3(p.initPos.x, 5f, p.initPos.y);
                this.otherPlayer.Add(p.playerId, tmp);
                var msother = tmp.GetComponent<MSOtherPlayer>();
                msother.playerId = p.playerId;
                msother.playerName = p.playerName;
                this.msOther.Add(p.playerId, msother);
            }
        }
        switchLock();
        SubModBase modUI;
        subMods.TryGetValue("ModUIs", out modUI);
        ((ModUIs)modUI).OnJoinedGame();
        Debug.Log("game inited!!");
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

    public GameObject GetPlayer(int id)
    {
        if (id == mainPlayerId)
        {
            return mainPlayer;
        }
        else
        {
            GameObject ret;
            otherPlayer.TryGetValue(id, out ret);
            return ret;
        }
    }

    public MonoBehaviour GetPlayerObject(int id)
    {
        if (id == mainPlayerId)
        {
            return msHero;
        }
        else
        {
            MSOtherPlayer ret;
            msOther.TryGetValue(id, out ret);
            return ret;
        }
    }

    public Dictionary<int, GameObject> GetAllPlayerObjects()
    {
        Dictionary<int, GameObject> all = new Dictionary<int, GameObject>();
        foreach (var p in otherPlayer)
        {
            all.Add(p.Key, p.Value);
        }

        all.Add(mainPlayerId, mainPlayer);
        return all;
    }

    public Dictionary<int, MonoBehaviour> GetAllPlayer()
    {
        Dictionary<int, MonoBehaviour> all = new Dictionary<int, MonoBehaviour>();
        foreach (var p in msOther)
        {
            all.Add(p.Key, p.Value);
        }

        all.Add(mainPlayerId, msHero);
        return all;
    }
}