using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class WeaponHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentClipText, maxClipSizeText, ammoInventoriedText;
    [SerializeField] Image weaponSprite;
    Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        player.OnWeaponEquipped.AddListener(OnPlayerEquippedWeapon);
        player.OnWeaponUnequipped.AddListener(OnPlayerUnequippedWeapon);
        OnPlayerEquippedWeapon(player.activeWeapon);
    }

    private void OnDestroy()
    {
        if(player == null)
        {
            return;
        }

        player.OnWeaponEquipped.RemoveListener(OnPlayerEquippedWeapon);
        player.OnWeaponUnequipped.RemoveListener(OnPlayerUnequippedWeapon);
        OnPlayerUnequippedWeapon(player.activeWeapon);
    }


    // callback method evoked when the player equips an active weapon
    private void OnPlayerEquippedWeapon(Weapon equippedWeapon)
    {
        // if the active weapon is null, then disable the weapon UI
        if(equippedWeapon == null)
        {
            ToggleWeaponUI(false);
        }
        else
        {
            // Otherwise, update the weapon UI HUD
            // subscribe to the clip size changed event and the inventoried ammo changed event
            equippedWeapon.OnClipSizeChanged.AddListener(UpdateClipCountUI);
            equippedWeapon.OnInventoriedAmmoChanged.AddListener(UpdateInventoriedAmmoCountUI);
            // Update the current clip count text
            UpdateClipCountUI(equippedWeapon.CurrentClipAmmo);
            // update the inventoried ammo count text
            UpdateInventoriedAmmoCountUI(equippedWeapon.InventoriedAmmo);
            // update the max clip capacity count text
            UpdateMaxClipCountUI(equippedWeapon.WeaponConfig.ClipCapacity);
            // update the weapon HUD sprite
            UpdateWeaponSpriteUI(equippedWeapon.WeaponConfig.WeaponSprite);
        }
    }

    // callback method evoked when the player unequips an active weapon
    private void OnPlayerUnequippedWeapon(Weapon unequippedWeapon)
    {
        // If the unequipped weapon is not null, then unsubscribe to ammo change events
        if(unequippedWeapon != null)
        {
            unequippedWeapon.OnClipSizeChanged.RemoveListener(UpdateClipCountUI);
            unequippedWeapon.OnInventoriedAmmoChanged.RemoveListener(UpdateInventoriedAmmoCountUI);
        }
        // toggle off the weapon HUD
        ToggleWeaponUI(false);
    }

    private void UpdateClipCountUI(int clipCount)
    {
        currentClipText.text = clipCount.ToString();
    }

    private void UpdateMaxClipCountUI(int clipCapacity)
    {
        maxClipSizeText.text = clipCapacity.ToString();
    }

    private void UpdateInventoriedAmmoCountUI(int inventoriedAmmo)
    {
        ammoInventoriedText.text = inventoriedAmmo.ToString();
    }

    private void UpdateWeaponSpriteUI(Sprite sprite)
    {
        weaponSprite.sprite = sprite;
    }



    private void ToggleWeaponUI(bool toggle)
    {
        currentClipText.enabled = toggle;
        maxClipSizeText.enabled = toggle;
        weaponSprite.enabled = toggle;
    }

}
