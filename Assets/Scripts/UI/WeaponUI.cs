using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI magSizeText;
    [SerializeField] private TextMeshProUGUI storedAmmoText;

    public void UpdateInfo(Sprite weaponIcon, int magSize, int storedAmmo)
    {
        icon.sprite = weaponIcon;
        magSizeText.SetText(magSize.ToString());
        storedAmmoText.SetText(storedAmmo.ToString());
    }
}
