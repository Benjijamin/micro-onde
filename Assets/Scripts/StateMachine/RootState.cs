using UnityEngine;

public class RootState<T> : State
{
    [SerializeField] protected T parent;
}
