using UnityEngine;

public class DodgeMechanic : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if(bullet != null && !bullet.usedByPlayer && !bullet.hit) ScoreManager.Instance.ScoreDodge();
    }
}
