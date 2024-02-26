using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class HittableSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _Hittable;

    [SerializeField]
    private ScoreBoardController _ScoreboardController;

    [SerializeField]
    private RandomPointInsideMesh _PointInMesh;

    [SerializeField]
    private float _MinimumSpawnDistance = 1.5f;

    [SerializeField]
    private int _InitialTargetCount = 3;

    [SerializeField]
    private List<SpawnedHittable> _SpawnedHittableList = new List<SpawnedHittable>();

    private void Start()
    {
        SpawnHittable(_InitialTargetCount);
    }

    public void SpawnHittable(int NumberToSpawn) 
    {
        StartCoroutine(FindValidSpawnPoint(NumberToSpawn));
    }

    private IEnumerator FindValidSpawnPoint(int NumberToFind)
    {
        for (int i = 0; i < NumberToFind; i++)
        {
            bool found = false;
            Vector3 position = _PointInMesh.ReturnPointInsideMesh();

            while (!found)
            {
                position = _PointInMesh.ReturnPointInsideMesh();
                found = IsPositionValid(position);
                yield return null;
            }

            Transform hittable = Instantiate(_Hittable).transform;
            hittable.position = position;
            hittable.rotation = Quaternion.LookRotation(hittable.position - new Vector3(0, 1, 0));
            List<Hittable> hittables = new List<Hittable>();
            hittables.AddRange(hittable.GetComponentsInChildren<Hittable>());

            SpawnedHittable spawnedHittable = new SpawnedHittable(position, hittables);

            foreach (Hittable hit in hittables)
            {
                hit.OnHit.AddListener(() => { RemoveHittableFromList(spawnedHittable); });
                hit.OnHit.AddListener(() => { _ScoreboardController.AddScore(hit.Value); });
                hit.OnHit.AddListener(() => { SpawnHittable(1); });
            }

            _SpawnedHittableList.Add(spawnedHittable);
            yield return null;
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (SpawnedHittable existingSpawnedHittable in _SpawnedHittableList)
        {
            if (Vector2.Distance(position, existingSpawnedHittable.Position) < _MinimumSpawnDistance)
            {
                return false; 
            }
        }
        return true; 
    }

    private void RemoveHittableFromList(SpawnedHittable hittable) 
    {
        _SpawnedHittableList.Remove(hittable);
    }

    [System.Serializable]
    private class SpawnedHittable 
    {
        public SpawnedHittable(Vector3 pos, List<Hittable> hittable) 
        {
            _Position = pos;
            _Hittables = hittable;
        }

        public Vector3 Position => _Position;
        public List<Hittable> Hittables => _Hittables;
        [SerializeField]
        private Vector3 _Position;
        [SerializeField]
        private List<Hittable> _Hittables;
    }
}


