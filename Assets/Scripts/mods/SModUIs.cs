using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SModUIs : SubModBase
{
    GameObject canvas;

    GameObject aimPoint;

    Dictionary<int, GameObject> dogTags = new Dictionary<int, GameObject>();

    Dictionary<int, GameObject> hpBars = new Dictionary<int, GameObject>();

    Dictionary<int, Transform> dotTagPos = new Dictionary<int, Transform>();

    public static string modName = "SModUIs";

    public SModUIs(ModBase owner, GameObject canvas) : base(owner, "SModUIs") { this.canvas = canvas; }

    public override void Start() 
    {
        aimPoint = GameObject.Find("AimPoint");
        aimPoint.SetActive(false);
    }

    public override void Update() 
    {
        FollowUIs();
    }

    public override void Stop() {}

    public void WaitJoin() 
    {
        GameObject.Find("Start").SetActive(false);
        GameObject.Find("StartSingle").SetActive(false);
        GameObject.Find("Quit").SetActive(false);
        var waitingText = GameObject.Find("WaitingText");
        var text = waitingText.GetComponent<Text>();
        text.text = string.Format("waiting for players to join...");
    }

    [AttrModEvent("GameJoin")]
    public void OnEventGameJoin(Param args)
    {
        GameObject.Find("WaitingText").SetActive(false);
        var players = ((ModGameMaster)GetOwner()).GetAllPlayer();
        GameObject dogTag =  Resources.Load("DogTag") as GameObject;
        GameObject HPBar =  Resources.Load("HPBar") as GameObject;
        int id = MSMain.mainPlayerId;
        int teamMate = ((ModGameMaster)GetOwner()).teamMateId;
        foreach (var pair in players)
        {
            string dogTagText = "";
            if (pair.Value is MSHero)
            {
                MSHero msh = (MSHero)pair.Value;
                dogTagText += msh.playerName + " ID:" + msh.id;
            }
            else
            {
                MSOtherPlayer mso = (MSOtherPlayer)pair.Value;
                dogTagText += mso.playerName + " ID:" + mso.playerId;
            }
            var dt = GameObject.Instantiate(dogTag);
            var hp = GameObject.Instantiate(HPBar);
            dt.transform.parent = canvas.transform;
            hp.transform.parent = canvas.transform;
            var text = dt.GetComponent<Text>();
            text.text = dogTagText;
            if (pair.Key == id)
            {
                text.color = Color.green;
            }
            else if (pair.Key == teamMate)
            {
                text.color = Color.blue;
            }
            else
            {
                text.color = Color.red;
            }
            dogTags.Add(pair.Key, dt);
            hpBars.Add(pair.Key, hp);
            var dtPos = pair.Value.GetComponentInChildren<Transform>();
            dotTagPos.Add(pair.Key, dtPos);
        }
    }

    public void FollowUIs()
    {
        // dog tags and HP bar
        foreach (var pair in dotTagPos)
        {
            Transform dtPos = pair.Value;
            GameObject dogTag;
            dogTags.TryGetValue(pair.Key, out dogTag);
            GameObject hp;
            hpBars.TryGetValue(pair.Key, out hp);
            var v3 = dtPos.position;

            // dog tag position
            v3.y += 0.3f;
            Vector2 pt = Camera.main.WorldToScreenPoint(v3);
            RectTransform rect = dogTag.transform as RectTransform;
            rect.pivot = new Vector2(0, 0);
            rect.position = pt;

            // hp bar position
            v3.y += 0.2f;
            Vector2 ppt = Camera.main.WorldToScreenPoint(v3);
            RectTransform rrect = hp.transform as RectTransform;
            rrect.pivot = new Vector2(0, 0);
            rrect.position = pt;
        }
    }

    [AttrModEvent("CameraZoom")]
    public void OnCameraZoom(Param param)
    {
        GameObject myDogTag;
        dogTags.TryGetValue(MSMain.mainPlayerId, out myDogTag);
        myDogTag.SetActive(false);
        aimPoint.SetActive(true);
    }

    [AttrModEvent("CameraUnzoom")]
    public void OnCameraUnzoom(Param param)
    {
        GameObject myDogTag;
        dogTags.TryGetValue(MSMain.mainPlayerId, out myDogTag);
        myDogTag.SetActive(true);
        aimPoint.SetActive(false);
    }
}