using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Interactable : MonoBehaviour
{
    public string interactText;

    public virtual void Interact()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactor interactor = collision.GetComponent<Interactor>();
        if (interactor != null)
        {
            interactor.AddInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactor interactor = collision.GetComponent<Interactor>();
        if (interactor != null)
        {
            interactor.RemoveInteractable(this);
        }
    }
}
