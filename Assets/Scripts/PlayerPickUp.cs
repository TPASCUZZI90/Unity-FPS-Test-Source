using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{ 
    [SerializeField] private float pickupRange;
    [SerializeField] private LayerMask pickupLayer;
    private Camera cam;
    private Inventory inventory;
    private EquipmentManager manager;

    

    private PlayerHUD hud;

    private void Start()
    {
        GetReferences();
        pickupRange = 5.0f;
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        string objectName = "";
        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            objectName = hit.transform.name.Replace(" pickup", "").Replace("(Clone)", "");
            hit.transform.name = objectName;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Weapon newWeapon = hit.transform.GetComponent<ItemObject>().item as Weapon;
                newWeapon.name = newWeapon.name.Replace(" pickup", "").Replace("(Clone)", "");
                inventory.AddWeapon(newWeapon);
                Destroy(hit.transform.gameObject);
                manager.UnequipWeapon();
                manager.EquipWeapon(inventory.GetItem((int)newWeapon.slot));
            }
        } else
        {
            objectName = "";
        }
        hud.UpdatePickUpName(objectName);
    }

    private void GetReferences()
    {
        hud = GetComponent<PlayerHUD>();
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        manager = GetComponent<EquipmentManager>();
    }
}
