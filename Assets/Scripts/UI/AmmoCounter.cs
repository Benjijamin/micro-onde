using TMPro;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoCount;

    public void TrackWeapon(Weapon newWeapon, Weapon oldWeapon)
    {
        if (oldWeapon is Gun)
        {
            ((Gun)oldWeapon).OnAttack -= UpdateAmmoCount;
        }
        if(newWeapon is not Gun)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            Gun gun = (Gun)newWeapon;
            gun.OnAttack += UpdateAmmoCount;
            UpdateAmmoCount(gun.GetAmmoCount(), gun.GetMaxAmmoCount());
        }
    }

    private void UpdateAmmoCount(int ammoLeft, int maxAmmo)
    {
        ammoCount.text = ammoLeft.ToString() + " / " + maxAmmo.ToString();
    }
}
