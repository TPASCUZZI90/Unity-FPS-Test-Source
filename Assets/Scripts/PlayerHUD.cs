using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private TextMeshProUGUI pickupName;
    [SerializeField] private WeaponUI weaponUI;

    public void Start()
    {
        pickupName.SetText("");
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthBar.SetValues(currentHealth, maxHealth);
    }

    public void UpdatePickUpName(string objectName)
    {
        pickupName.SetText(objectName.ToString());
    }

    public void UpdateWeaponUI(Weapon newWeapon)
    {
        weaponUI.UpdateInfo(newWeapon.icon, newWeapon.magSize, newWeapon.storedAmmo);
    }
}
