using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialGolfBallInitializer : AbstractGolfBallInitializer
{
    [SerializeField] private GolfBallCollector collector;
    [SerializeField] private Vector3 difficultyThresholds;
    [SerializeField, Min(0)] private float jitter;

    public override void Initialize(GameObject golfBall)
    {
        bool reachable = collector.GetPathDistance(golfBall.transform.position, out float distance);

        if (reachable)
        {
            GolfBall golfBallComponent = golfBall.GetComponent<GolfBall>();

            distance += Random.Range(-jitter, jitter);

            if (distance < difficultyThresholds.x)
            {
                golfBallComponent.SetLevel(GolfBallLevel.Easy);
            }
            else if (distance < difficultyThresholds.y)
            {
                golfBallComponent.SetLevel(GolfBallLevel.Medium);
            }
            else if (distance < difficultyThresholds.z)
            {
                golfBallComponent.SetLevel(GolfBallLevel.Hard);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(collector.transform.position, difficultyThresholds.x);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(collector.transform.position, difficultyThresholds.y);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(collector.transform.position, difficultyThresholds.z);
    }
}
