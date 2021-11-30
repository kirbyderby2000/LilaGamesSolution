using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    /// <summary>
    /// Primary slot weapon 1
    /// </summary>
    public PrimaryWeapon primaryWeapon1;
    /// <summary>
    /// Primary slot weapon 2
    /// </summary>
    public PrimaryWeapon primaryWeapon2;

    /// <summary>
    /// Secondary slot weapon 
    /// </summary>
    public SecondaryWeapon secondaryWeapon;

    /// <summary>
    /// The active weapon on the player has equipped
    /// </summary>
    public Weapon activeWeapon;

    private void Awake()
    {
        SwitchActiveWeapon(WeaponSlot.PrimaryWeapon_1);
    }

    private void Update()
    {
        // if the active weapon is null, then do nothing
        if(activeWeapon == null)
        {
            return;
        }

        // otherwise, pass in the corresponding fire button event state into the weapon (down / held / released)
        if (Input.GetMouseButtonDown(0))
        {
            activeWeapon.FirePressed(Time.deltaTime);
        }
        else if (Input.GetMouseButton(0))
        {
            activeWeapon.FireHeld(Time.deltaTime);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            activeWeapon.FireReleased(Time.deltaTime);
        }
    }

    /// <summary>
    /// Switches the active weapon into the inventoried weapon slot
    /// </summary>
    /// <param name="slot"></param>
    public void SwitchActiveWeapon(WeaponSlot slot)
    {
        Weapon weaponToSwitchInto = GetWeaponInSlot(slot);
        // if the active weapon is the same as the weapon requested, then do nothing
        if (activeWeapon == weaponToSwitchInto)
        {
            return;
        }
        // If the active weapon is not null and it allows us to switch weapons, then proceed to do so by unequiping 
        // the active weapon
        if(activeWeapon != null)
        {
            if(activeWeapon.CanSwitchWeapons() == false)
            {
                return;
            }

            UnequipActiveWeapon();
        }
        // equip the active weapon requested
        EquipActiveWeapon(weaponToSwitchInto);
    }


    /// <summary>
    /// Equips a weapon into the provided weapon slot
    /// </summary>
    /// <param name="weaponToEquip">The weapon to equip</param>
    /// <param name="slot">The slot to equip the weapon into</param>
    public void EquipWeapon(Weapon weaponToEquip, WeaponSlot slot)
    {
        // check for invalid weapon equipping requests
        if(weaponToEquip is PrimaryWeapon && slot == WeaponSlot.SecondaryWeapon)
        {
            Debug.LogError("You cannot equip a primary weapon into the secondary slot");
            return;
        }
        if (weaponToEquip is SecondaryWeapon && (slot == WeaponSlot.PrimaryWeapon_1 || slot == WeaponSlot.PrimaryWeapon_2))
        {
            Debug.LogError("You cannot equip a secondary weapon into a primary slot");
            return;
        }

        // get the weapon about to be swapped out of the inventory
        Weapon weaponToSwap = GetWeaponInSlot(slot);
        // cache whether or not we need to immediately active the weapon being inventoried
        bool immediatelyActivateWeapon = false;
        // if the weapon to swap out is the active weapon, check it if we can unequip it (switch weapons)
        if(weaponToSwap == activeWeapon && activeWeapon != null)
        {
            if(activeWeapon.CanSwitchWeapons() == false)
            {
                return;
            }
            // unequip the active weapon and immediately activate the weapon inventoried
            UnequipActiveWeapon();
            immediatelyActivateWeapon = true;
        }

        // Equip the provided weapon into the requested item slot
        switch (slot)
        {
            case WeaponSlot.PrimaryWeapon_1:
                primaryWeapon1 = weaponToEquip as PrimaryWeapon;
                break;
            case WeaponSlot.PrimaryWeapon_2:
                primaryWeapon2 = weaponToEquip as PrimaryWeapon;
                break;
            case WeaponSlot.SecondaryWeapon:
                secondaryWeapon = weaponToEquip as SecondaryWeapon;
                break;
        }

        // immediately equip the new weapon if the dropped equipment was the active weapon
        if (immediatelyActivateWeapon)
        {
            EquipActiveWeapon(weaponToEquip);
        }

        OnInventoriedWeaponsChanged.Invoke();
    }

    /// <summary>
    /// Unequips the active weapon
    /// </summary>
    private void UnequipActiveWeapon()
    {
        // if the active weapon is not null, unequip it and call the weapon unequipped event
        if(activeWeapon != null)
        {
            activeWeapon.Unequip();
            OnWeaponUnequipped.Invoke(activeWeapon);
        }
    }

    /// <summary>
    /// Equips a given weapon as the active weapon
    /// </summary>
    /// <param name="weaponToEquip"></param>
    private void EquipActiveWeapon(Weapon weaponToEquip)
    {
        // equip the weapon into the active weapon slot, call the weapon equipped event
        activeWeapon = weaponToEquip;
        if(activeWeapon != null)
        {
            activeWeapon.Equip();
        }
        OnWeaponEquipped.Invoke(activeWeapon);
    }

    /// <summary>
    /// returns the weapon in the provided inventory slot
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    private Weapon GetWeaponInSlot(WeaponSlot slot)
    {
        if(slot == WeaponSlot.PrimaryWeapon_1)
        {
            return primaryWeapon1;
        }
        else if(slot == WeaponSlot.PrimaryWeapon_2)
        {
            return primaryWeapon2;
        }
        else
        {
            return secondaryWeapon;
        }
    }


    public enum WeaponSlot { PrimaryWeapon_1, PrimaryWeapon_2, SecondaryWeapon}

    
    /// <summary>
    /// Event called when a weapon is actively equipped
    /// </summary>
    public UnityEvent<Weapon> OnWeaponEquipped = new UnityEvent<Weapon>();

    /// <summary>
    /// Event called when a weapon is unequipped
    /// </summary>
    public UnityEvent<Weapon> OnWeaponUnequipped = new UnityEvent<Weapon>();

    /// <summary>
    /// Event called when the player's inventoried weapons changed
    /// </summary>
    public UnityEvent OnInventoriedWeaponsChanged = new UnityEvent();
}
