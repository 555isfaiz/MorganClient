using UnityEngine;

public class SModFirearms : SubModBase
{
    public static GameObject Rifle_Bullet_Prefab;
    public static GameObject shootPoint;
    protected int magazine_ammo;
    protected int total_ammo;
    protected bool fire_start = false;
    protected float fire_frequency;
    protected float nextFireTime;
    protected float reload_duration;

    public SModFirearms(ModBase owner, string ModName) : base(owner, ModName) 
    {
        Rifle_Bullet_Prefab = Resources.Load("RifleBullet") as GameObject;
        shootPoint = GameObject.FindGameObjectWithTag("ShootPoint");
    }

    public bool canShoot()
    {
        if (!fire_start) return false;
        if (magazine_ammo == 0) return false;
        if (Time.time < nextFireTime) return false;
        nextFireTime = Time.time + 1 / fire_frequency;
        return true;
    }

    public void StartFire1() 
    { 
        StartFire1Override(); 
    }

    public virtual void StartFire1Override() {}

    public void StopFire1() 
    {
        StopFire1Override();
    }

    public virtual void StopFire1Override() {}

    public void StartFire2() 
    {
        StartFire2Override();
    }

    public virtual void StartFire2Override() { ((ModWeapon)GetOwner()).Zoom(); }

    public void StopFire2() 
    { 
        StopFire2Override(); 
    }

    public virtual void StopFire2Override() { ((ModWeapon)GetOwner()).Unzoom(); }
}
