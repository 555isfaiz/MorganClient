using UnityEngine; 

public class FMAutoRifle : SModFirearms
{

    public FMAutoRifle(ModBase owner) : base(owner, "FMAutoRifle") 
    {
        total_ammo = 100;
        magazine_ammo = 20;
        fire_frequency = MSGlobalParams.Rifle_fire_frequency;
        reload_duration = MSGlobalParams.Rifle_reload_duration;
    }

    public override void StartFire1()
    {
        fire_start = true;
    }

    public override void StopFire1()
    {
        fire_start = false;
    }

    public override void Update()
    {
        if (!canShoot()) return;

        var shootPoint = GameObject.FindGameObjectWithTag("ShootPoint");
        var bullet = GameObject.Instantiate(Rifle_Bullet_Prefab);
        bullet.SendMessage("SetShooter", GetOwner().GetOwner());
        bullet.transform.position = shootPoint.transform.position;
        bullet.transform.rotation = shootPoint.transform.rotation;
    }
}