using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;

    private WeaponShooting shooting;

    public Transform dropPoint = null;

    private EquipmentManager manager;


    private void Start()
    {
        GetReferences();
        InitVar();
    }

    public void AddWeapon(Weapon newWeapon)
    {
        int newWeaponIndex = (int)newWeapon.slot;

        if (weapons[newWeaponIndex] != null)
        {
            RemoveItem(newWeaponIndex);
        }
        weapons[newWeaponIndex] = newWeapon;

        shooting.InitAmmo((int)newWeapon.slot, newWeapon);
    }

    private void InitVar()
    {
        weapons = new Weapon[3];
    }

    public Weapon GetItem(int index)
    {
        return weapons[index];
    }

    public void RemoveItem(int index)
    {
        DropItem(index);
        weapons[index] = null;
    }

    public void DropItem(int index)
    {
        GameObject dropedWeapon = Instantiate(GetItem(index).prefab, dropPoint);
        Rigidbody rb = dropedWeapon.AddComponent<Rigidbody>();
        dropedWeapon.transform.parent = null;
    }

    private void GetReferences()
    {
        shooting = GetComponent<WeaponShooting>();
    }
}
