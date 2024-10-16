using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleGolfCollectorDecisionMaker : AbstractGolfCollectorDecisionMaker
{
    private GolfBallCollector collector;

    public override void DecideFor(GolfBallCollector collector)
    {
        this.collector = collector;
    }

    public override GameObject GetBestGolfBall(GameObject[] golfBalls)
    {
        GameObject bestGolfBall = null;
        float bestDistance = float.MaxValue;

        foreach (GameObject golfBall in golfBalls)
        {
            bool reachable = collector.GetPathDistance(golfBall.transform.position, out float distance);
            if (!reachable) continue;
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestGolfBall = golfBall;
            }
        }

        return bestGolfBall;
    }
}