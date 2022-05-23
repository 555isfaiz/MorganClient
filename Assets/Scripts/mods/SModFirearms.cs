public class SModFirearms : SubModBase
{
    protected int magazine_ammo;
    protected int total_ammo;

    public SModFirearms(ModBase owner, string ModName) : base(owner, ModName) {}

    public virtual void StartFire1() {}
    public virtual void StopFire1() {}
    public virtual void StartFire2() { ((ModWeapon)GetOwner()).Zoom(); }
    public virtual void StopFire2() { ((ModWeapon)GetOwner()).Unzoom(); }
}
