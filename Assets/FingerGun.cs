using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class FingerGun : MonoBehaviour
{
    private enum Hand { Left, Right};

    [SerializeField]
    private Hand _Hand = Hand.Left;

    [SerializeField]
    private Transform _LaserOrigin;

    [SerializeField]
    private float _LaserLength;

    [SerializeField]
    private LineRenderer _LineRenderer;

    [SerializeField]
    private AudioClip _Fire;
    [SerializeField]
    private AudioClip _Empty;
    [SerializeField]
    private AudioClip _Sipn;

    [SerializeField]
    private GameObject _HitEffect;

    private AudioSource _AudioSource;

    private bool _IsAiming;

    private Vector3 _LaserDirection = Vector3.zero;

    private void Awake()
    {
        if (!_LineRenderer) 
        {
            _LineRenderer = GetComponent<LineRenderer>();
        }
        _AudioSource = GetComponent<AudioSource>();

        switch (_Hand) 
        {
            case Hand.Left:
                _LaserDirection = Vector3.left;
                break;
            case Hand.Right:
                _LaserDirection = Vector3.right;
                break;
            default:break;
        }
    }

    public void Aim(bool isAiming) 
    {
        _IsAiming = isAiming;
        _LineRenderer.enabled = _IsAiming;
        //_AudioSource.PlayOneShot(_Sipn);
    }

    public void Fire() 
    {
        _AudioSource.PlayOneShot(_Fire);
        RaycastHit hit;
        if (Physics.Raycast(_LaserOrigin.position, _LaserOrigin.TransformDirection(_LaserDirection) * _LaserLength,out hit)) 
        {
            Hittable hittable = hit.transform.GetComponent<Hittable>();
            if (hittable) 
            {
                hittable.OnHit.Invoke();
            }
            Transform SpawnedHitEffect = Instantiate(_HitEffect).transform;

            SpawnedHitEffect.position = hit.point;

            SpawnedHitEffect.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
            Destroy(SpawnedHitEffect,2);
            Debug.Log($"Hit {hit.collider.name}",hit.collider.gameObject);
        }
    }

    private void LateUpdate()
    {
        if (_IsAiming)
        {
            _LineRenderer.SetPosition(0, _LaserOrigin.position);
            _LineRenderer.SetPosition(1, _LaserOrigin.position += _LaserOrigin.TransformDirection(_LaserDirection) * _LaserLength);
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(_LaserOrigin.position, _LaserOrigin.TransformDirection(_LaserDirection) * _LaserLength);
    }
}
