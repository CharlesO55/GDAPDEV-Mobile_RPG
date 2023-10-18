using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class InteractableDetector : MonoBehaviour
{
    public static InteractableDetector Instance;

    List<GameObject> _interactableObjects;

    [SerializeField] private Vector3 _colliderOffset = new Vector3(0, 1, 0);

    SphereCollider _collider;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        this._interactableObjects = new List<GameObject>();
    }

    private void OnEnable()
    {
        this._interactableObjects.Clear();
    }

    private void OnDisable()
    {
        this._interactableObjects.Clear();
    }

    private void Update()
    {
        this.transform.position = PartyManager.Instance.ActivePlayer.transform.position + _colliderOffset;
    } 

    public bool TryGetClosestInteractableObject(out GameObject closestObject, bool bExcludingActivePlayer = true)
    {
        closestObject = null;
        float closestDistance = 100;

        foreach (GameObject interactableObj in _interactableObjects)
        {
            //Exclude active player from list of collisions
            if(bExcludingActivePlayer && interactableObj == PartyManager.Instance.ActivePlayer)
            {
                //Skip to the next obj
            }
            
            else
            {
                float distanceApart = Vector3.Distance(interactableObj.transform.position, this.transform.position);
                if (distanceApart < closestDistance)
                {
                    closestObject = interactableObj;
                    closestDistance = distanceApart;
                }
            }
            
        }


        return (closestObject != null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<IInteractable>(out IInteractable interactableInterface))
        {
            //Highlight it
            interactableInterface.HighlightInteractable(true);
            
            this._interactableObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this._interactableObjects.Contains(other.gameObject))
        {
            //Remove highlight
            other.gameObject.GetComponent<IInteractable>().HighlightInteractable(false);
            
            this._interactableObjects.Remove(other.gameObject);
        }
    }

    public void RemoveFromDetectedList(GameObject toRemove)
    {
        this._interactableObjects.Remove(toRemove);
    }
}