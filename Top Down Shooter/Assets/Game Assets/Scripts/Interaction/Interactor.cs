using UnityEngine;
using System.Collections.Generic;

public class Interactor : MonoBehaviour
{
    [SerializeField] private List<InteractionObject> _stashes;
    [SerializeField] private List<InteractionObject> _items;
    [SerializeField] private List<InteractionObject> _doors;

    public void AddInterationObject(GameObject obj)
    {
        InteractionObject intercationObject = obj.GetComponent<InteractionObject>();
        List<InteractionObject> list = GetList(intercationObject);

        if (list == null)
            return;

        if (list.Contains(intercationObject))
            return;

        if (intercationObject.IsAbleToInteract())
            list.Add(intercationObject);
    }

    public void RemoveInteractionObject(GameObject obj)
    {
        InteractionObject intercationObject = obj.GetComponent<InteractionObject>();
        List<InteractionObject> list = GetList(intercationObject);

        if (list == null)
            return;

        if (!list.Contains(intercationObject))
            return;

        list.Remove(intercationObject);
    }

    public void UpdateInteraction(List<GameObject> objects)
    {
        
    }

    private List<InteractionObject> GetList(InteractionObject obj)
    {
        switch (obj.GetInteractionType())
        {
            case InteractionType.Item:
                return _items;
            case InteractionType.Stash:
                return _stashes;
            case InteractionType.Door:
                return _doors;
            default:
                return null;
        }
    }
}
