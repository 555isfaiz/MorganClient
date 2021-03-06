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
    
    public ModGameMaster(MonoBehaviour owner, GameObject mainPlayer) : base(owner)
    {
        this.mainPlayer = mainPlayer;
        msHero = mainPlayer.GetComponent<MSHero>();
    }

    public override void StartOverride()
    {
        GameObject canvas = GameObject.Find("Canvas");
        var modUI = new ModUIs(this, canvas);
        subMods.Add(ModUIs.modName, modUI);
    }

    public override void UpdateOverride()
    {
        // var controller = GetOwner()
        if (MSShare.modControl.TryExecuteCommand(ModControl.Command.SWITCH_LOCK))
        {
            switchLock();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
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
                mainPlayer.transform.position = new Vector3(p.curPos.x, 5f, p.curPos.z);
                msHero.id = mainPlayerId;
                msHero.playerName = p.playerName;
            }
            else
            {
                var tmp = GameObject.Instantiate(go);
                tmp.transform.position = new Vector3(p.curPos.x, 5f, p.curPos.z);
                this.otherPlayer.Add(p.playerId, tmp);
                var msother = tmp.GetComponent<MSOtherPlayer>();
                msother.playerId = p.playerId;
                msother.playerName = p.playerName;
                this.msOther.Add(p.playerId, msother);
            }
        }
        switchLock();
        ((ModUIs)GetSubMod(ModUIs.modName)).OnJoinedGame();
        Debug.Log("game inited!!");
    }

    public void SessionSync(int sessionId, List<BPlayer> players)
    {
        if (sessionId != MSShare.currentSessionId)
        {
            Debug.LogError("incorrect sessionId, received:" + sessionId + ", currentId:" + MSShare.currentSessionId);
            return;
        }

        foreach (var p in players)
        {
            if (p.playerId != MSShare.mainPlayerId)
            {
                var other = GetPlayerObject(p.playerId) as MSOtherPlayer;
                if (!other.acceptSync)
                {
                    continue;
                }
            }
            var go = GetPlayer(p.playerId);
            Vector3 pos = new Vector3(p.curPos.x, p.curPos.y, p.curPos.z);
            if (p.playerId != mainPlayerId && Vector3.Distance(pos, go.transform.position) > 0)
            {
                go.transform.position = pos;
            }

            // ...
        }
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