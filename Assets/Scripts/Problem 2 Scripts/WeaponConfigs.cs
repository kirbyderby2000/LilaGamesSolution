using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Weapon configuration flyweights for weapons
/// </summary>
[CreateAssetMenu(fileName ="Untitled Weapon Config",menuName ="Weapon Config")]
public class WeaponConfigs : ScriptableObject
{
    /// <summary>
    /// Name of this weapon
    /// </summary>
    [field: SerializeField, Tooltip("Weapon name")]
    public string WeaponName
    {
        private set;
        get;
    }

    /// <summary>
    /// Weapon UI Sprite
    /// </summary>
    [field: SerializeField, Tooltip("Weapon UI Sprite")]
    public Sprite WeaponSprite
    {
        private set;
        get;
    }

    /// <summary>
    /// The clip capacity of this weapon
    /// </summary>
    [field: SerializeField, Tooltip("Weapon's clip capacity")]
    public int ClipCapacity
    {
        private set;
        get;
    }

    /// <summary>
    /// Weapon's fire rate per second
    /// </summary>
    [field: SerializeField, Tooltip("Weapon's fire rate per second")]
    public float FireRate
    {
        private set;
        get;
    } = 8.0f;

    /// <summary>
    /// How much ammo the player can have inventoried (stored) for this weapon
    /// </summary>
    [field: SerializeField, Tooltip("How much ammo the player can have inventoried (stored) for this weapon")]
    public int MaxAmmoInventoried
    {
        private set;
        get;
    } = 90;

    /// <summary>
    /// How much damage this weapon deals per bullet
    /// </summary>
    [field: SerializeField, Tooltip("How much damage this weapon deals per bullet"), Min(1.0f)]
    public float DamagePerBullet
    {
        private set;
        get;
    } = 10.0f;

    /// <summary>
    /// The fire type of this weapon
    /// </summary>
    [field: SerializeField, Tooltip("The fire type of this weapon")]
    public FireType DefaultFireType
    {
        private set;
        get;
    } = FireType.Automatic;



    /// <summary>
    /// How many bullets are fired in a burst if this weapon is semi-automatic
    /// </summary>
    [field: SerializeField, Tooltip("How many bullets are fired in a burst if this weapon is semi-automatic")]
    public int SemiAutoBurstLength
    {
        private set;
        get;
    } = 10;


    [System.Serializable]
    public enum FireType { Single, Automatic, SemiAutomatic}
}
