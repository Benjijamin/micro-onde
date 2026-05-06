using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    [SerializeField] private AmmoCounter ammoCounter;

    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private DefaultWeapon defaultWeapon;
    [SerializeField] private Transform hand;

    private void Start()
    {
        Weapon.OnPickUp += SwapWeapon;
    }

    private void Update()
    {
        if (currentWeapon is Gun)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)mousePos - (Vector2)currentWeapon.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if (currentWeapon.CanAttack(transform) && Input.GetKey(KeyCode.Mouse0))
        {
            currentWeapon.Attack(true);
        }
    }

    public void SwapWeapon(Weapon newWeapon)
    {
        if(currentWeapon is not DefaultWeapon)
        {
            currentWeapon.transform.SetParent(newWeapon.transform.parent);
            currentWeapon.transform.position = newWeapon.transform.position;
            currentWeapon.Drop();
        }
        if(currentWeapon is Gun)
        {
            ((Gun)currentWeapon).OnAttack -= SwapToDefaultOnAmmoEmpty;
        }
        ammoCounter.TrackWeapon(newWeapon, currentWeapon);
        currentWeapon = newWeapon;
        if (currentWeapon is Gun)
        {
            ((Gun)currentWeapon).OnAttack += SwapToDefaultOnAmmoEmpty;
        }

        currentWeapon.transform.SetParent(hand);
        currentWeapon.transform.position = hand.position;
        currentWeapon.transform.rotation = hand.rotation;
    }

    private void SwapToDefaultOnAmmoEmpty(int ammoCount, int arg2)
    {
        if(ammoCount == 0)
        {
            ammoCounter.TrackWeapon(defaultWeapon, currentWeapon);
            Destroy(currentWeapon.gameObject);
            currentWeapon = defaultWeapon;
        }
    }
}
