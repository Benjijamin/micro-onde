using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private InteractText UI;

    private List<Interactable> interactblesInRange = new List<Interactable>();

    public void AddInteractable(Interactable interactable)
    {
        interactblesInRange.Add(interactable);
        UpdateUI();
    }

    public void RemoveInteractable(Interactable interactable)
    {
        interactblesInRange.Remove(interactable);
        UpdateUI();
    }

    private void UpdateUI()
    {
        bool isActive = interactblesInRange.Count > 0;
        UI.gameObject.SetActive(isActive);
        if (isActive)
        {
            UI.UpdateText(interactblesInRange[0].interactText);
        }
    }

    private void Update()
    {
        if (interactblesInRange.Count > 0 && Input.GetKeyDown(KeyCode.E))
        {
            interactblesInRange[0].Interact();
        }
    }
}
