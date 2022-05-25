using System.Collections.Generic;
using UnityEngine;

public class ModWeapon : ModBase
{
    GameObject player;
    bool fire_1 = false;
    bool fire_2 = false;
    SModFirearms firearm_main;
    SModFirearms firearm_1;
    SModFirearms firearm_2;

    public ModWeapon(MonoBehaviour owner, GameObject go) : base(owner, "ModWeapon") 
    {
        player = go; 

        SModFirearms autoRifle = new FMAutoRifle(this);
        SModFirearms bazooka = new FMBazooka(this);
        AddSubMod("autoRifle", autoRifle);
        AddSubMod("bazooka", bazooka);

        firearm_main = firearm_1 = autoRifle;
        firearm_2 = bazooka;
    }

    public override void UpdateOverride()
    {
        if (GetOwner() is MSOtherPlayer) return;
        if (firearm_main == null) return;

        bool do_fire_1 = MSMain.modControl.TryExecuteCommand(ModControl.Command.FIRE_1);
        bool do_fire_2 = MSMain.modControl.TryExecuteCommand(ModControl.Command.FIRE_2);
        if (do_fire_1 && !fire_1)
        {
            fire_1 = true;
            firearm_main.StartFire1();
        }
        else if (!do_fire_1 && fire_1)
        {
            fire_1 = false;
            firearm_main.StopFire1();
        }

        if (do_fire_2 && !fire_2)
        {
            fire_2 = true;
            firearm_main.StartFire2();
        }
        else if (!do_fire_2 && fire_2)
        {
            fire_2 = false;
            firearm_main.StopFire2();
        }
    }

    public void Zoom()
    {
        MSMain.modControl.SetCameraZoom(true);
        MSMain.FireEvent("CameraZoom");
    }

    public void Unzoom()
    {
        MSMain.modControl.SetCameraZoom(false);
        MSMain.FireEvent("CameraUnzoom");
    }
}