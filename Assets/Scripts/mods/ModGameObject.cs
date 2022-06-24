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
        if (!MSMain.single) return;

        var collide = collision.gameObject;
        // if collide is bullet
        // do damage

        if (collide.tag.Equals("Bullet")) 
        {
            MSBullet bullet = collide.GetComponent<MSBullet>();
            TakeDamage(bullet.bulletType, bullet.shooter);
        }
    }

    void TakeDamage(int bulletType, MonoBehaviour shooter)
    {
        int damage = 0;
        switch (bulletType)
        {
            case 1:
                damage = 10;
                break;
            default:
                break;
        }

        hp_current -= damage;
        MSMain.FireEvent("TakeDamage", "bulletType", bulletType, "shooter", shooter, "damageTaker", GetOwner(), "damage", damage);

        if (hp_current <= 0)
            MSMain.FireEvent("PlayerDied", "killer", shooter, "Dead", GetOwner());
    }
}