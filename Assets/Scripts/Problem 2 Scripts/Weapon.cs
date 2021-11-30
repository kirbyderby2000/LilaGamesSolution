using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base abstract class for weapons
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    /// <summary>
    /// How much ammo is available in the current clip
    /// </summary>
    public int CurrentClipAmmo
    {
        private set;
        get;
    } = 0;

    /// <summary>
    /// The configuration settings for this weapon
    /// </summary>
    [field: SerializeField, Tooltip("Weapon configuration settings")]
    public WeaponConfigs WeaponConfig
    {
        private set;
        get;
    }

    /// <summary>
    /// How much ammo the player has inventoried for this weapon
    /// </summary>
    public int InventoriedAmmo
    {
        private set;
        get;
    } = 0;

    /// <summary>
    /// the total duration the player has been firing
    /// </summary>
    protected float _fireTime = 0;

    /// <summary>
    /// how many bullets have been shot from this burst
    /// </summary>
    protected int _burstBulletsFired = 0;

    /// <summary>
    /// Equips this weapon onto the player
    /// </summary>
    public abstract void Equip();

    /// <summary>
    /// Unequips this weapon from the player
    /// </summary>
    public abstract void Unequip();


    /// <summary>
    /// Returns whether or not this weapon can be switched (to ensure that reload animations are not interrupted or fire animations are interrupted)
    /// </summary>
    /// <returns></returns>
    public abstract bool CanSwitchWeapons();

    

    /// <summary>
    /// Weapon handling when the fire button is pressed
    /// </summary>
    public virtual void FirePressed(float deltaTime)
    {
        _fireTime = 0;
        _burstBulletsFired = 0;
        Firing();
    }

    /// <summary>
    /// Weapon handling when the fire button is held
    /// </summary>
    public virtual void FireHeld(float deltaTime)
    {
        _fireTime += deltaTime;

        Firing();
    }

    /// <summary>
    /// Weapon handling when the fire button is released
    /// </summary>
    public virtual void FireReleased(float deltaTime)
    {

    }

    protected virtual void Firing()
    {
        // check if we can fire, if not then check if we can reload, otherwise do nothing
        if(CanFire() == false)
        {
            if(CanReload() == false)
            {
                return;
            }

            Reload();
            return;
        }


        // Handle the weapon firing based on the fire-type
        switch (WeaponConfig.DefaultFireType)
        {
            // if the weapon fires in a single shot, then shoot if it's the first frame of the weapon being fired
            case WeaponConfigs.FireType.Single:
                if(_fireTime == 0)
                {
                    FireBullet();
                    ConsumeBulletsFromClip();
                }
                break;
            default:
                // if the weapon is semi-automatic and the full burst has been completed, then do nothing
                if(WeaponConfig.DefaultFireType == WeaponConfigs.FireType.SemiAutomatic && _burstBulletsFired >= WeaponConfig.SemiAutoBurstLength)
                {
                    return;
                }

                // if the fire time has exceeded the fire rate, or we're firing the first bullet, 
                // then fire a bullet and consume 1 bullet from the clip. reset the fire-time duration
                if (_fireTime > 1 / WeaponConfig.FireRate || _fireTime == 0)
                {
                    _fireTime = 0.0f;
                    FireBullet();
                    ConsumeBulletsFromClip();
                    _burstBulletsFired++;
                }

                break;
        }

    }

    /// <summary>
    /// Method called to fire a bullet
    /// </summary>
    protected abstract void FireBullet();


    /// <summary>
    /// Returns whether or not this weapon can be fired (if ammmo is available in the clip)
    /// </summary>
    /// <returns></returns>
    public virtual bool CanFire()
    {
        return CurrentClipAmmo > 0;
    }


    /// <summary>
    /// Whether or not this weapon can be reloaded
    /// </summary>
    /// <returns></returns>
    public bool CanReload()
    {
        return InventoriedAmmo == 0 ? false : CurrentClipAmmo < WeaponConfig.ClipCapacity;
    }


    /// <summary>
    /// Consumes bullets from the weapon clip (1 bullet if nothing is passed)
    /// </summary>
    /// <param name="bulletsConsumed"></param>
    protected void ConsumeBulletsFromClip(int bulletsConsumed = 1)
    {
        CurrentClipAmmo = Mathf.Max(CurrentClipAmmo - bulletsConsumed, 0);
        OnClipSizeChanged.Invoke(CurrentClipAmmo);
    }

    /// <summary>
    /// Add to the player's inventoried ammo for this weapon
    /// </summary>
    /// <param name="ammoToGive"></param>
    public void AddInventoriedAmmo(int ammoToGive)
    {
        InventoriedAmmo += Mathf.Abs(ammoToGive);
        InventoriedAmmo = Mathf.Min(InventoriedAmmo, WeaponConfig.MaxAmmoInventoried);
        OnInventoriedAmmoChanged.Invoke(InventoriedAmmo);
    }

    /// <summary>
    /// Reloads the weapon clip with inventoried ammo
    /// </summary>
    public void Reload()
    {
        // calculate how much ammo we're going to reload with
        // get how many bullets are needed to refill the clip
        int reloadCount = WeaponConfig.ClipCapacity - CurrentClipAmmo;
        // then cap use the lowest of the inventoried ammo or the reload count
        // this will prevent us from reloading ammo that isn't available in our inventory
        reloadCount = Mathf.Min(InventoriedAmmo, reloadCount);

        // increment the reload count into the current clip and decrement the reload count from the inventory
        CurrentClipAmmo += reloadCount;
        InventoriedAmmo -= reloadCount;
        // call the ammo count changed events
        OnClipSizeChanged.Invoke(CurrentClipAmmo);
        OnInventoriedAmmoChanged.Invoke(InventoriedAmmo);
        // play some reload animation here
    }

    /// <summary>
    /// Event called when this weapon's clip count changes
    /// </summary>
    public UnityEvent<int> OnClipSizeChanged = new UnityEvent<int>();

    /// <summary>
    /// Event called when this weapon's inventoried ammo count changes
    /// </summary>
    public UnityEvent<int> OnInventoriedAmmoChanged = new UnityEvent<int>();
}
