using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    private Camera cam;
    private float lastShootTime = 0;
    private Inventory inventory;
    private EquipmentManager manager;
    private AudioSource son;

    [SerializeField] private bool canShoot;

    [SerializeField] private int primaryAmmoMag;
    [SerializeField] private int primaryAmmoStorage;
    [SerializeField] private int secondaryAmmoMag;
    [SerializeField] private int secondaryAmmoStorage;

    [SerializeField] private bool primaryMagEmpty = false;
    [SerializeField] private bool secondaryMagEmpty = false;

    private void Start()
    {
        GetReferences();
        canShoot = true;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            son = GetComponentInChildren<AudioSource>();
            Shoot();
        }
    }

    private void RaycastShoot(Weapon currentWeapon)
    {
        
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        float currentWeaponRange = currentWeapon.range;
        if (Physics.Raycast(ray, out hit, currentWeaponRange))
        {
            Debug.Log(hit.transform.name);
        }
        Instantiate(currentWeapon.particle, manager.currentWeaponBarrel);
        son.Play();
    }

    private void Shoot()
    {
        CheckCanShoot(manager.currentlyEquippedWeapon);

        if (canShoot)
        {
            Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

            if (Time.time > lastShootTime + currentWeapon.fireRate)
            {
                Debug.Log("Shoot");
                lastShootTime = Time.time;

                RaycastShoot(currentWeapon);
                UseAmmo((int)currentWeapon.slot, 1, 0);
            }
        }
        else
        {
            Debug.Log("empty mag");
        }

        
        
    }

    private void UseAmmo(int slot, int currentAmmoUsed, int currentStoredAmmoUsed)
    {
        if(slot == 0)
        {
            if(primaryAmmoMag <= 0)
            {
                primaryMagEmpty = true;
                CheckCanShoot(manager.currentlyEquippedWeapon);
            }
            else {
                primaryAmmoMag -= currentAmmoUsed;
                primaryAmmoStorage -= currentStoredAmmoUsed;
            }
            
        }

        if (slot == 1)
        {
            if (secondaryAmmoMag <= 0)
            {
                secondaryMagEmpty = true;
                CheckCanShoot(manager.currentlyEquippedWeapon);
            }
            else
            {
                secondaryAmmoMag -= currentAmmoUsed;
                secondaryAmmoStorage -= currentStoredAmmoUsed;
            }
        }
    }

    private void CheckCanShoot(int slot)
    {   
        if(slot == 0)
        {
            if (primaryMagEmpty)
            {
                canShoot = false;
            }
            else
            {
                canShoot = true;
            }
        }

        if (slot == 1)
        {
            if (secondaryMagEmpty)
            {
                canShoot = false;
            }
            else
            {
                canShoot = true;
            }
        }
    }

    private void GetReferences()
    {
        
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>(); 
        manager = GetComponent<EquipmentManager>();
    }

    public void InitAmmo(int slot, Weapon weapon)
    {
        if(slot == 0)
        {
            primaryAmmoMag = weapon.magSize;
            primaryAmmoStorage = weapon.storedAmmo;
        }

        if (slot == 1)
        {
            secondaryAmmoMag = weapon.magSize;
            secondaryAmmoStorage = weapon.storedAmmo;
        }
    }
}
