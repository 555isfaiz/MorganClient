using UnityEngine;

public class ModGameObject : ModBase
{
    int hp_current = MSGlobalParams.HP_MAX;

    public ModGameObject(MonoBehaviour owner) : base(owner, "ModGameObject") {}

    public override void StartOverride()
    {

    }

    public override void UpdateOverride()
    {
        
    }

    public override void StopOverride()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        var collide = collision.gameObject;
        // if collide is bullet
        // do damage

        
    }
}