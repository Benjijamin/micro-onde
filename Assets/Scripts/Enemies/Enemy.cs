using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private Weapon weapon;
    [SerializeField] private float dropRadius;
    [SerializeField] private LayerMask rayMask;
    [SerializeField] private float sightAngle;
    [SerializeField] private float sightRange;

    public bool hasBeenAlerted = false;
    public bool hasBeenPinged = false;

    private void Start()
    {
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        weapon.GetComponent<CircleCollider2D>().enabled = false;
        weapon.wielder = transform;
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }

    public Weapon GetWeapon() 
    {
        return weapon; 
    }

    public void DropWeapon()
    {
        weapon.transform.SetParent(null);
        Vector3 dropPos = Quaternion.Euler(0, 0, Random.Range(0, 360f)) * transform.right * dropRadius;
        weapon.transform.position += dropPos;
        weapon.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        weapon.Drop();
    }

    private void Update()
    {
        Transform player = PlayerMovement.Instance.transform;
        Vector2 playerDirection = player.position - transform.position;
        float angle = Vector2.SignedAngle(transform.right, playerDirection);
        Vector2 rayDirection = Quaternion.Euler(0, 0, Mathf.Clamp(angle, -sightAngle / 2, sightAngle / 2)) * transform.right;
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = rayMask;
        RaycastHit2D[] hit = new RaycastHit2D[1];
        Physics2D.Raycast(transform.position, rayDirection, contactFilter, hit, sightRange);
        Debug.DrawRay(transform.position, rayDirection * sightRange, Color.black);
        if (hit[0].collider != null)
        {
            print(hit[0].collider.name);
        }
        if(hit[0].collider?.transform == player)
        {
            stateMachine.SetState(typeof(AlertState), false);
        }
        else
        {
            stateMachine.SetState(typeof(PatrolState), false);
        }
    }
}
