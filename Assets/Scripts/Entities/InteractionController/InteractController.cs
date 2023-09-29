using Components.Action;
using System;
using UnityEngine;
using Utils;

namespace Entities
{
    public class InteractController : MonoBehaviour
    {
        public string interactableTag;
        public event Action<bool> OnInteractionChangeEvent;

        private void Start()
        {
            if (interactableTag == null) { interactableTag = Constants.PLAYER_TAG; }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(interactableTag))
            {
                Debug.LogWarning("interactableTag is null or empty.");
                return;
            }

            if (other.gameObject.CompareTag(interactableTag))
            {
                Interaction interaction = other.GetComponent<Interaction>();
                if (interaction == null) { Debug.LogWarning("other doesn't have Interaction component."); }

                interaction.currentInteract = gameObject;
                OnInteractionChangeEvent?.Invoke(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(interactableTag))
            {
                Debug.LogWarning("interactableTag is null or empty.");
                return;
            }

            if (other.gameObject.CompareTag(interactableTag))
            {
                Interaction interaction = other.GetComponent<Interaction>();
                if (interaction == null) { Debug.LogWarning("other doesn't have Interaction component."); }

                interaction.currentInteract = null;
                OnInteractionChangeEvent?.Invoke(false);
            }
        }
    }
}