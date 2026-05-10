using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : RootState<Enemy>
{
    [SerializeField] private float distanceThreshold;
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();
    [MinMaxSlider(0, 10)][SerializeField] private Vector2 pauseTime;
    [Range(0, 1)][SerializeField] private float turnLerpValue;

    private Vector2 lookDirection;
    private int patrolPointIndex;
    private bool isPaused;
    private float distanceRemaining => (transform.position - patrolPoints[patrolPointIndex].position).magnitude;

    public override void StateEnter()
    {
        base.StateEnter();
        MoveNext();
    }

    public override void StateUpdate()
    {
        if (!isPaused) 
        { 
            lookDirection = (Vector2)patrolPoints[patrolPointIndex].position - (Vector2)transform.position; 
        }
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        parent.transform.rotation = Quaternion.Lerp(parent.transform.rotation, Quaternion.Euler(0, 0, 90 + angle), turnLerpValue);
        if (isPaused) { return; }
        if (distanceRemaining < distanceThreshold)
        {
            StartCoroutine(Pause());
        }
    }

    private IEnumerator Pause()
    {
        isPaused = true;
        parent.GetAnimController().CancelAnimation(CharacterAnim.Walk);
        lookDirection = patrolPoints[patrolPointIndex].up;
        yield return new WaitForSeconds(Random.Range(pauseTime.x, pauseTime.y));
        MoveNext();
        parent.GetAnimController().SetAnimation(CharacterAnim.Walk);
        isPaused = false;
    }

    private void MoveNext()
    {
        patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Count;
        parent.GetAgent().SetDestination(patrolPoints[patrolPointIndex].position);
    }
}
