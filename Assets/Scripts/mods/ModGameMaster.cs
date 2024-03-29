using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    public ModGameMaster(MonoBehaviour owner, GameObject mainPlayer) : base(owner, "ModGameMaster")
    {
        this.mainPlayer = mainPlayer;
        msHero = mainPlayer.GetComponent<MSHero>();
    }

    public override void StartOverride()
    {
        GameObject canvas = GameObject.Find("Canvas");
        var modUI = new SModUIs(this, canvas);
        subMods.Add(SModUIs.modName, modUI);
    }

    public override void UpdateOverride()
    {
        
    }

    public override void StopOverride(){}

    public void SetMyId(int id)
    {
        mainPlayerId = id;
        MSMain.mainPlayerId = id;
    }

    // using myside to tell if I am shooter or target
    [AttrModEvent("GameJoin")]
    public void OnEventGameJoin(Param args)
    {
        int myside;
        int session;
        List<BPlayer> players;
        args.Get("myside", out myside);
        args.Get("lastGameSession", out session);
        args.Get("players", out players);
        this.lastGameSession = session;
        MSMain.currentSessionId = lastGameSession;
        GameObject go =  Resources.Load("OtherPlayer") as GameObject;
        foreach (var p in players)
        {
            if (p.side == myside && p.playerId != mainPlayerId)
            {
                teamMateId = p.playerId;
            }

            if (p.playerId == mainPlayerId)
            {
                mainPlayer.transform.position = new Vector3(p.curPos.x, p.curPos.y, p.curPos.z);
                msHero.id = mainPlayerId;
                msHero.playerName = p.playerName;
            }
            else
            {
                var tmp = GameObject.Instantiate(go);
                tmp.transform.position = new Vector3(p.curPos.x, p.curPos.y, p.curPos.z);
                this.otherPlayer.Add(p.playerId, tmp);
                var msother = tmp.GetComponent<MSOtherPlayer>();
                msother.playerId = p.playerId;
                msother.playerName = p.playerName;
                this.msOther.Add(p.playerId, msother);
            }
        }
        Debug.Log("game inited!!");
    }

    public void SessionSync(int sessionId, List<BPlayer> players)
    {
        if (sessionId != MSMain.currentSessionId)
        {
            Debug.LogError("incorrect sessionId, received:" + sessionId + ", currentId:" + MSMain.currentSessionId);
            return;
        }

        foreach (var p in players)
        {
            if (p.playerId != MSMain.mainPlayerId)
            {
                var other = GetPlayerObject(p.playerId) as MSOtherPlayer;
                if (!other.acceptSync)
                {
                    continue;
                }
            }
            
            var go = GetPlayer(p.playerId);
            Vector3 pos = new Vector3(p.curPos.x, p.curPos.y, p.curPos.z);
            Quaternion rot = new Quaternion(p.rotation.x, p.rotation.y, p.rotation.z, p.rotation.w);
            if (p.playerId != mainPlayerId)
            {
                go.transform.rotation = rot;
                go.transform.eulerAngles = new Vector3(go.transform.eulerAngles.x, go.transform.eulerAngles.y, 0);
                if (Vector3.Distance(pos, go.transform.position) > 0) 
                {
                    go.transform.position = pos;
                }
            }

            // ...
        }
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

    public GameObject GetMainPlayer()
    {
        return mainPlayer;
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

    public MonoBehaviour GetCameraObject()
    {
        return GetOwner();
    }
}