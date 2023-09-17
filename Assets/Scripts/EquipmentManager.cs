using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public int currentlyEquippedWeapon = 2;
    public GameObject currentWeaponObject = null;
    public Transform currentWeaponBarrel = null;

    public Transform WeaponHolderR = null;
    private Animator animate;
    private Inventory inventory;
    private PlayerHUD hud;

    [SerializeField] Weapon defaultWeapon = null;

    private void Start()
    {
        GetReferences();
        InitVar();
        //Debug.Log(animate.GetInteger("Type"));
    }

    private void Update()
    {
        //Debug.Log(animate.GetInteger("Type"));
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentlyEquippedWeapon != 0)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(0));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && currentlyEquippedWeapon != 1)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(1));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && currentlyEquippedWeapon != 2)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(2));
        }

    }

    public void EquipWeapon(Weapon weapon)
    {
        currentlyEquippedWeapon = (int)weapon.slot;
        animate.SetInteger("GunType", (int)weapon.type);
        hud.UpdateWeaponUI(weapon);
        
    }

    public void UnequipWeapon()
    {
        animate.SetTrigger("unequipWeapon");
        //Destroy(currentWeaponObject);
    }

    private void InitVar()
    {
        inventory.AddWeapon(defaultWeapon);
        EquipWeapon(inventory.GetItem(2));
    }

    private void GetReferences()
    {
        animate = GetComponentInChildren<Animator>();
        inventory = GetComponent<Inventory>();
        hud = GetComponent<PlayerHUD>();
    }
}
