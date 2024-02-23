using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class FingerGun : MonoBehaviour
{
    private enum Hand { Left, Right,Up,Down,Forward,Back};

    [SerializeField]
    private Hand _Hand = Hand.Left;

    [SerializeField]
    private Transform _LaserOrigin;

    [SerializeField]
    private float _LaserLength;

    [SerializeField]
    private LineRenderer _LineRenderer;

    [SerializeField] 
    private Transform _LaserTip;

    [SerializeField]
    private int _MaxShots = 6;

    [SerializeField]
    private AudioClip _Fire;
    [SerializeField]
    private AudioClip _Empty;
    [SerializeField]
    private AudioClip _Spin;

    [SerializeField]
    private GameObject _HitEffect;

    [SerializeField]
    private Vector3 _ForceVector;
    [SerializeField]
    private ForceMode _ForceMode;

    private AudioSource _AudioSource;

    private bool _IsAiming;

    private Vector3 _LaserDirection = Vector3.zero;

    private int _ShotsRemaining;


    private Transform _LerpedLaserOrigin;

    [SerializeField]
    private int _LerpSpeedModifier = 10;

    private RaycastHit _CurrentHit;

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
            case Hand.Up:
                _LaserDirection = Vector3.up;
                break;
            case Hand.Down:
                _LaserDirection = Vector3.down;
                break;
            case Hand.Forward:
                _LaserDirection = Vector3.forward;
                break;
            case Hand.Back:
                _LaserDirection = Vector3.back;
                break;
            default:break;
        }

        _ShotsRemaining = _MaxShots;

        _LerpedLaserOrigin = new GameObject().transform;
    }

    public void Aim(bool isAiming) 
    {
        _IsAiming = isAiming;
        _LineRenderer.enabled = _IsAiming;
        _LaserTip.gameObject.SetActive(_IsAiming);
        //_AudioSource.PlayOneShot(_Sipn);
    }

    public void Fire() 
    {
        if (_ShotsRemaining <= 0)
        {
            _AudioSource.PlayOneShot(_Empty);
        }
        else 
        {
            _AudioSource.PlayOneShot(_Fire);

            if (_CurrentHit.collider != null) 
            {
                Hittable hittable = _CurrentHit.collider.GetComponent<Hittable>();
                if (hittable)
                {
                    Debug.Log("Hittable on hit object", _CurrentHit.collider.gameObject);
                    hittable.Hit(_CurrentHit.point, _ForceVector, _ForceMode);
                }
                else
                {
                    Debug.Log("No Hittable on hit object", _CurrentHit.collider.gameObject);
                }
                Transform SpawnedHitEffect = Instantiate(_HitEffect).transform;

                SpawnedHitEffect.position = _CurrentHit.point;

                SpawnedHitEffect.rotation = Quaternion.LookRotation(_CurrentHit.normal, Vector3.up);
                Destroy(SpawnedHitEffect.gameObject, 2);
                Debug.Log($"Hit {_CurrentHit.collider.name}", _CurrentHit.collider.gameObject);
            }

            
            _ShotsRemaining--;
        }
    }

    public void Reload()
    {
        if (_ShotsRemaining == _MaxShots) 
        {
            return;
        }

        _ShotsRemaining = _MaxShots;
        _AudioSource.PlayOneShot(_Spin);
    }

    private void LateUpdate()
    {
        if (_IsAiming)
        {
            _LerpedLaserOrigin.position =Vector3.Lerp(_LerpedLaserOrigin.position, _LaserOrigin.position,Time.deltaTime * _LerpSpeedModifier) ;
            _LerpedLaserOrigin.rotation = Quaternion.Lerp(_LerpedLaserOrigin.rotation, _LaserOrigin.rotation,Time.deltaTime * _LerpSpeedModifier);

            Vector3 origin = _LerpedLaserOrigin.position;
            Vector3 endPos = origin += _LerpedLaserOrigin.TransformDirection(_LaserDirection) * _LaserLength;

            RaycastHit hit;
            if (Physics.Raycast(_LerpedLaserOrigin.position, _LerpedLaserOrigin.TransformDirection(_LaserDirection) * _LaserLength, out hit))
            {
                if (hit.collider != null) 
                {
                    endPos = hit.point;
                }
                
            }
            _CurrentHit = hit;

            _LaserTip.position = endPos;
            _LineRenderer.SetPosition(0, _LerpedLaserOrigin.position);
            _LineRenderer.SetPosition(1, endPos);

        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(_LaserOrigin.position, _LaserOrigin.TransformDirection(_LaserDirection) * _LaserLength);
    }
}
