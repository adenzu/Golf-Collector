using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialGolfBallInitializer : AbstractGolfBallInitializer
{
    [SerializeField] private GolfBallCollector collector;

    public override void Initialize(GameObject golfBall)
    {
        bool reachable = collector.GetPathDistance(golfBall.transform.position, out float distance);

        if (reachable)
        {
            GolfBall golfBallComponent = golfBall.GetComponent<GolfBall>();

            if (distance < 5)
            {
                golfBallComponent.SetLevel(GolfBallLevel.Easy);
            }
            else if (distance < 10)
            {
                golfBallComponent.SetLevel(GolfBallLevel.Medium);
            }
            else
            {
                golfBallComponent.SetLevel(GolfBallLevel.Hard);
            }
        }
    }
}
