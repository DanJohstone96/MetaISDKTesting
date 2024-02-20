using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _Hittable;

    [SerializeField]
    private ScoreBoardController _ScoreboardController;

    [SerializeField]
    private float _SpawnRadius = 1;

    private void Awake()
    {
        SpawnHittable();    
    }

    public void SpawnHittable() 
    {
        Vector3 origin = transform.position;
        Vector3 position = origin += Random.insideUnitSphere * _SpawnRadius;

        Transform hittable = Instantiate(_Hittable).transform;
        hittable.position = position;

        List<Hittable> hittables = new List<Hittable>();

        hittables.AddRange(hittable.GetComponentsInChildren<Hittable>());

        foreach (Hittable hit in hittables) 
        {
            hit.OnHit.AddListener(() => { _ScoreboardController.AddScore(hit.Value); });
            hit.OnHit.AddListener(() => { SpawnHittable(); });
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _SpawnRadius);
    }
}
