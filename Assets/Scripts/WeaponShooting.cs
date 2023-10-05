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
    private PlayerHUD playerHUD;

    private Animator anim;

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

        if (Input.GetKey(KeyCode.R))
        {
            Reload(manager.currentlyEquippedWeapon);
        }
    }

    private void GetReferences()
    {

        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        manager = GetComponent<EquipmentManager>();
        anim = GetComponentInChildren<Animator>();
        playerHUD = GetComponent<PlayerHUD>();
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
        Instantiate(currentWeapon.muzzleParticle, manager.currentWeaponBarrel);
        GameObject shootParticle = Instantiate(currentWeapon.shootParticle, manager.currentWeaponBarrel);
        shootParticle.transform.parent = null;
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
                playerHUD.UpdateWeaponAmmoUI(primaryAmmoMag, primaryAmmoStorage);
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
                playerHUD.UpdateWeaponAmmoUI(secondaryAmmoMag, secondaryAmmoStorage);
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

    private void AddAmmo(int slot, int currentAmmoAdded, int currentStoredAmmoAdded)
    {
        if (slot == 0)
        {
            primaryAmmoMag += currentAmmoAdded;
            primaryAmmoStorage -= currentStoredAmmoAdded;
            playerHUD.UpdateWeaponAmmoUI(primaryAmmoMag,primaryAmmoStorage);

        }

        if (slot == 1)
        {
            secondaryAmmoMag += currentAmmoAdded;
            secondaryAmmoStorage -= currentStoredAmmoAdded;
            playerHUD.UpdateWeaponAmmoUI(secondaryAmmoMag, secondaryAmmoStorage);
        }
    }

    private void Reload(int slot)
    {
        // primary
        if(slot == 0)
        {
            int ammoToReload = inventory.GetItem(0).magSize - primaryAmmoMag;
            if((primaryAmmoMag != inventory.GetItem(0).magSize) && (primaryAmmoStorage >= ammoToReload)) // Pour remplir le chargeur entièrement
            {
                AddAmmo(slot, ammoToReload, 0); // Pour remplir le chargeur
                UseAmmo(slot, 0, ammoToReload); // Pour Updater l'UI

                primaryMagEmpty = false; // Le chargeur n'est plus vide
            } else
            {
                if((primaryAmmoMag + primaryAmmoStorage) < inventory.GetItem(0).magSize)// Pas assez en stock pour remplir tout le chargeur mais au moins 1 munition en stock
                {
                    Debug.Log("pas assez pour remplir tout le chargeur mais au moins 1 balle");
                    AddAmmo(slot, primaryAmmoStorage, 0); // Pour remplir le chargeur
                    UseAmmo(slot, 0, primaryAmmoStorage); // Pour Updater l'UI
                }
                Debug.Log("pas assez de munitions en stock");
            }
        }
        // secondary
        if(slot == 1)
        {
            int ammoToReload = inventory.GetItem(1).magSize - secondaryAmmoMag;
            if ((secondaryAmmoMag != inventory.GetItem(1).magSize) && (secondaryAmmoStorage >= ammoToReload))
            {
                AddAmmo(slot, ammoToReload, 0);
                UseAmmo(slot, 0, ammoToReload);

                secondaryMagEmpty = false;
            }
            else
            {
                Debug.Log("pas assez de munitions en stock");
            }
        }

        CheckCanShoot(slot);
        anim.SetTrigger("reload");
        manager.currentWeaponAnim.SetTrigger("reload");
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
