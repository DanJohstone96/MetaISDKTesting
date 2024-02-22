using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointInsideMesh : MonoBehaviour
{
    [SerializeField]
    private MeshFilter _SpawnAreaMeshFilter;

    private List<Vector3> _SpawnAreaMeshVerts = new List<Vector3>();

    private void Awake()
    {
        _SpawnAreaMeshFilter.sharedMesh.GetVertices(_SpawnAreaMeshVerts);

    }

    public Vector3 ReturnPointInsideMesh()
    {
        if (_SpawnAreaMeshVerts.Count == 0) 
        {
            return Vector3.zero;
        }

        Transform spawnAreaTransform = _SpawnAreaMeshFilter.transform;

        Vector3 startVert = spawnAreaTransform.TransformPoint(_SpawnAreaMeshVerts[Random.Range(0, _SpawnAreaMeshVerts.Count)]);
        Vector3 meshCenter = spawnAreaTransform.TransformPoint(_SpawnAreaMeshFilter.sharedMesh.bounds.center);
        Vector3 randomPoint = new Vector3(Random.Range(startVert.x, meshCenter.x), Random.Range(startVert.y, meshCenter.y), Random.Range(startVert.z, meshCenter.z));

        return randomPoint;
    }

}
