using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGolfCollectorDecisionMaker : MonoBehaviour
{
    public abstract void DecideFor(GolfBallCollector collector);
    public abstract GameObject GetBestGolfBall(GameObject[] golfBalls);
}
