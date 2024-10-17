using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolfBallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject golfBallPrefab;
    [SerializeField, Min(0)] private AbstractGolfBallInitializer initializer;
    [SerializeField, Min(0)] private int numberOfGolfBalls;

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

        int spawnedGolfBalls = 0;
        while (spawnedGolfBalls < numberOfGolfBalls)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                bounds.max.y,
                Random.Range(bounds.min.z, bounds.max.z)
            );

            if (Physics.Raycast(spawnPosition + Vector3.up, Vector3.down, out RaycastHit hit, bounds.max.y + 1f, LayerMask.GetMask("Ground")))
            {
                spawnPosition = hit.point + Vector3.up;

                if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit navMeshHit, 2f, NavMesh.AllAreas))
                {
                    GameObject golfBall = Instantiate(golfBallPrefab, navMeshHit.position + Vector3.up * 0.2f, Quaternion.identity, transform);
                    initializer.Initialize(golfBall);
                    spawnedGolfBalls++;
                }
            }

            yield return null;
        }
    }
}
