using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour
{
    public UnityEvent OnHit;

    [SerializeField]
    private Rigidbody _RigidBody;
    [SerializeField]
    private AudioSource _AudioSource;
    [SerializeField]
    private AudioClip _AudioClip;

    [SerializeField]
    private GameObject _DetroyOnHit;

    [SerializeField]
    private float _DestroyDelay = 5;

    [SerializeField]
    private float _Value = 5;

    [HideInInspector]
    public float Value => _Value;

    public void Hit(Vector3 hitPosition, Vector3 force,ForceMode forceMode) 
    {
        OnHit?.Invoke();
        Debug.Log("I was hit ");
        if (_RigidBody) 
        {
            Debug.Log("I Changed physics stuff");

            _RigidBody.isKinematic = false;
            _RigidBody.useGravity = true;
            _RigidBody.AddForceAtPosition(force, hitPosition,forceMode);
        }
        if (_AudioSource) 
        {
            Debug.Log("I played a sound ");

            _AudioSource.PlayOneShot(_AudioClip);
        }

        Destroy(_DetroyOnHit, _DestroyDelay);
    }
}
