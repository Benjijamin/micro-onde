using UnityEngine;

public class AlertState : RootState<Enemy>
{
    [Range(0, 1)][SerializeField] private float turnLerpValue;
    [SerializeField] private float attackDelay;

    private float attackTimer;

    public override void StateEnter()
    {
        base.StateEnter();
        if(parent.GetWeapon() is Gun)
        {
            parent.GetAgent().isStopped = true;
        }
        parent.hasBeenAlerted = true;
        attackTimer = attackDelay;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        Vector2 playerPos = PlayerMovement.Instance.transform.position;
        Vector2 direction = playerPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        parent.transform.rotation = Quaternion.Lerp(parent.transform.rotation, Quaternion.Euler(0, 0, angle), turnLerpValue);
        if (parent.GetWeapon() is Melee)
        {
            parent.GetAgent().SetDestination(playerPos);
        }
        else
        {
            direction = playerPos - (Vector2)parent.GetWeapon().transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            parent.GetWeapon().transform.rotation = Quaternion.Euler(0, 0, angle);
        }


        if (parent.GetWeapon().CanAttack(parent.transform) && attackTimer <= 0)
        {
            parent.GetWeapon().Attack(false);
        }

        attackTimer -= Time.deltaTime;
    }

    public override void StateExit()
    {
        base.StateExit();
        parent.GetAgent().isStopped = false;
    }
}
