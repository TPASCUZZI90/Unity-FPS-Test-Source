using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new weapon", menuName = "Items / Weapons")]
public class Weapon : Item
{
    public GameObject prefab;
    public GameObject particle;
    public int magSize;
    public int storedAmmo;
    public float range;
    public float fireRate;
    public WeaponType type;
    public WeaponSlot slot;
}

public enum WeaponType { FS, MP, Macuahuitl, Epee, Arcabuz}
public enum WeaponSlot { Primary, Secondary, Melee };
