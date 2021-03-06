using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModUIs : SubModBase
{
    GameObject canvas;

    Dictionary<int, GameObject> dogTags = new Dictionary<int, GameObject>();

    Dictionary<int, Transform> dotTagPos = new Dictionary<int, Transform>();

    public static string modName = "ModUIs";

    public ModUIs(ModBase owner, GameObject canvas) : base(owner) { this.canvas = canvas; }

    public override void Start() {}

    public override void Update() 
    {
        FollowDogTag();
    }

    public override void Stop() {}

    public void PreInit() 
    {
        GameObject.Find("Target").SetActive(false);
        GameObject.Find("Shooter").SetActive(false);
        var waitingText = GameObject.Find("WaitingText");
        var text = waitingText.GetComponent<Text>();
        text.text = string.Format("waiting for {0} to join...", MSShare.isShooter ? "target" : "shooter");
    }

    public void OnJoinedGame()
    {
        GameObject.Find("WaitingText").SetActive(false);
        var players = ((ModGameMaster)GetOwner()).GetAllPlayer();
        GameObject dogTag =  Resources.Load("DogTag") as GameObject;
        int id = MSShare.mainPlayerId;
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
            dt.transform.parent = canvas.transform;
            var text = dt.GetComponent<Text>();
            text.text = dogTagText;
            if (pair.Key == id || pair.Key == teamMate)
            {
                text.color = Color.green;
            }
            else
            {
                text.color = Color.red;
            }
            dogTags.Add(pair.Key, dt);
            var dtPos = pair.Value.GetComponentInChildren<Transform>();
            dotTagPos.Add(pair.Key, dtPos);
        }
    }

    public void FollowDogTag()
    {
        foreach (var pair in dotTagPos)
        {
            GameObject dogTag;
            dogTags.TryGetValue(pair.Key, out dogTag);
            var v3 = pair.Value.position;
            Vector2 pt = Camera.main.WorldToScreenPoint(v3);
            RectTransform rect = dogTag.transform as RectTransform;
            rect.pivot = new Vector2(0, 0);
            rect.position = pt;
        }
    }
}