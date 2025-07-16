using System.Collections.Generic;
using UnityEngine;

public class InteractorCollider : MonoBehaviour
{
    [SerializeField] private Interactor _interactor;

    [SerializeField] private string _allowedTag;
    [SerializeField] private List<GameObject> _interactionObjects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _allowedTag)
        {
            _interactionObjects.Add(other.gameObject);
            _interactor.AddInterationObject(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (_interactionObjects.Contains(other.gameObject))
        { 
            _interactionObjects.Remove(other.gameObject);
            _interactor.RemoveInteractionObject(other.gameObject);
        }

    }
}
