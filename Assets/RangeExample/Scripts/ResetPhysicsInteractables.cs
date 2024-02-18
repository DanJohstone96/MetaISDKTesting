using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class ResetPhysicsInteractables : MonoBehaviour
{
    [SerializeField]
    private List<PhysInteractabl> Interactables = new List<PhysInteractabl>();

    private void Awake()
    {
        foreach (PhysInteractabl interactable in Interactables) 
        {
            interactable._StartPosition = interactable._Transform.position;
            interactable._StartScale = interactable._Transform.localScale;
            interactable._StartRotation = interactable._Transform.rotation;
        }
    }

    public void ResetInteractables()
    {
        Debug.Log("Calling reset");
        foreach (PhysInteractabl interactable in Interactables)
        {
            bool KinematicState = interactable._Rigidbody.isKinematic;
            Transform currentInteractableTransform = interactable._Transform;
            if (!KinematicState) 
            {
                interactable._Rigidbody.isKinematic = true;
            }
            currentInteractableTransform.position = interactable._StartPosition;
            currentInteractableTransform.rotation = interactable._StartRotation;
            currentInteractableTransform.localScale = interactable._StartScale;
            interactable._Rigidbody.isKinematic = KinematicState;
        }
    }
}

[System.Serializable]
public class PhysInteractabl 
{
    public Rigidbody _Rigidbody;
    public Transform _Transform;

    [HideInInspector]
    public Vector3 _StartPosition;
    [HideInInspector]
    public Quaternion _StartRotation;
    [HideInInspector]
    public Vector3 _StartScale;
}
