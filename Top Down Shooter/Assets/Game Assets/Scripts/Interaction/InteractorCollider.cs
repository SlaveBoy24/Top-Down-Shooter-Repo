using System.Collections.Generic;
using UnityEngine;

public class InteractorCollider : MonoBehaviour
{
    [SerializeField] private string _allowedTag;
    [SerializeField] private List<GameObject> _interactionObjects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _allowedTag)
        {
            _interactionObjects.Add(other.gameObject);
            Interactor.Instance.AddInterationObject(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (_interactionObjects.Contains(other.gameObject))
        { 
            _interactionObjects.Remove(other.gameObject);
            Interactor.Instance.RemoveInteractionObject(other.gameObject);
        }

    }
}
