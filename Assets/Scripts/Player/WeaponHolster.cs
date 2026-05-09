using System;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    public static Action<float> OnPlayerInitiatedAttack;

    [SerializeField] private AmmoCounter ammoCounter;

    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private DefaultWeapon defaultWeapon;
    [SerializeField] private Transform hand;

    private float lastSwap = 0f;
    public bool HasSwappedRecently => Time.time - lastSwap < 2f;

    private void Start()
    {
        Weapon.OnPickUp += SwapWeapon;
        defaultWeapon.wielder = transform;
    }

    private void OnDestroy()
    {
        Weapon.OnPickUp -= SwapWeapon;
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (currentWeapon is Gun)
        {
            Vector2 direction = (Vector2)mousePos - (Vector2)currentWeapon.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if (currentWeapon.CanAttack(transform) && Input.GetKey(KeyCode.Mouse0))
        {
            currentWeapon.Attack(true);
            OnPlayerInitiatedAttack?.Invoke(currentWeapon.GetAttackCooldown());
            CameraManager.instance.Shake(0.05f, 0.05f);
            if(currentWeapon is Gun && ((Vector2)mousePos - (Vector2)transform.position).magnitude < GetComponent<CircleCollider2D>().radius)
            {
                GetComponent<PlayerHealth>().Suicide();
            }
        }
    }

    public void SwapWeapon(Weapon newWeapon)
    {
        lastSwap = Time.time;

        defaultWeapon.GetComponent<SpriteRenderer>().enabled = false;
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
        currentWeapon.wielder = transform;
    }

    private void SwapToDefaultOnAmmoEmpty(int ammoCount, int arg2)
    {
        if(ammoCount == 0)
        {
            ScoreManager.Instance.ScoreLastBullet();
            ammoCounter.TrackWeapon(defaultWeapon, currentWeapon);
            Destroy(currentWeapon.gameObject);
            currentWeapon = defaultWeapon;
            defaultWeapon.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
