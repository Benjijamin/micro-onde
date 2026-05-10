using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private Weapon weapon;
    [SerializeField] private float dropRadius;
    [SerializeField] private LayerMask rayMask;
    [SerializeField] private float sightAngle;
    [SerializeField] private float sightRange;

 
    [SerializeField] private Light2D revealLight;
    [SerializeField] private float revealDuration;

    private float revealTimer;
    private bool isRevealed;
    private Pingable pingable;

    public bool hasBeenAlerted = false;
    public bool hasBeenPinged = false;

    private void Awake()
    {
        pingable = GetComponent<Pingable>();
        pingable.OnPigned += Reveal;
    }

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
        Vector2 dropPos = transform.position + Quaternion.Euler(0, 0, Random.Range(0, 360f)) * transform.right * dropRadius;
        weapon.transform.position = dropPos;
        weapon.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        weapon.Drop();
    }

    public void Explode() 
    {
        Gibs g = GetComponentInChildren<Gibs>();
        if (g != null) 
        {
            g.Explode();
        }
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
        if(hit[0].collider?.transform == player)
        {
            stateMachine.SetState(typeof(AlertState), false);
        }
        else
        {
            stateMachine.SetState(typeof(PatrolState), false);
        }

        if (revealTimer > 0)
        {
            revealTimer -= Time.deltaTime;
        }
        else if (isRevealed)
        {
            isRevealed = false;
            StopAllCoroutines();
            StartCoroutine(HideCoroutine());
        }
    }

    private void Reveal()
    {
        hasBeenPinged = true;

        if (!isRevealed)
        {
            isRevealed = true;

            StopAllCoroutines();
            StartCoroutine(RevealCoroutine());
        }
        
        revealTimer = revealDuration;
    }

    private IEnumerator RevealCoroutine()
    {
        while (revealLight.pointLightOuterRadius < 1)
        {
            revealLight.pointLightOuterRadius += Time.deltaTime * 2;

            yield return null;
        }
        revealLight.pointLightOuterRadius = 1;
    }

    private IEnumerator HideCoroutine()
    {
        while (revealLight.pointLightOuterRadius > 0)
        {
            revealLight.pointLightOuterRadius -= Time.deltaTime * 2;

            yield return null;
        }
        revealLight.pointLightOuterRadius = 0;
    }

}