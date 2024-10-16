using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolfBallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject golfBallPrefab;
    [SerializeField, Min(0)] private AbstractGolfBallInitializer initializer;
    [SerializeField, Min(0)] private int numberOfGolfBalls;
    [SerializeField, Min(0f)] private float spawnInterval = 0f;

    private void Start()
    {
        StartCoroutine(SpawnGolfBalls());
    }

    private IEnumerator SpawnGolfBalls()
    {
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        Bounds bounds = new Bounds(triangulation.vertices[0], Vector3.zero);
        foreach (Vector3 vertex in triangulation.vertices)
        {
            bounds.Encapsulate(vertex);
        }

        for (int i = 0; i < numberOfGolfBalls; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                bounds.max.y,
                Random.Range(bounds.min.z, bounds.max.z)
            );

            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit))
            {
                spawnPosition.y = hit.point.y + golfBallPrefab.transform.localScale.y / 2;
                initializer.Initialize(Instantiate(golfBallPrefab, spawnPosition, Quaternion.identity, transform));
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
