using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartGolfCollectorDecisionMaker : AbstractGolfCollectorDecisionMaker
{
    [SerializeField] private Transform golfCart;
    [SerializeField, Min(1)] private int n = 1;
    private GolfBallCollector collector;

    public override void DecideFor(GolfBallCollector collector)
    {
        this.collector = collector;
    }

    public override GameObject GetBestGolfBall(GameObject[] golfBalls)
    {
        var difficultyGroups = golfBalls.GroupBy(ball => ball.GetComponent<GolfBall>().GetLevel());
        var closestBalls = new List<GameObject>();

        foreach (var group in difficultyGroups)
        {
            // NOTE: Use QuickSelect algorithm to find the n closest balls in the future 
            // for better performance
            var sortedBalls = group.OrderBy(ball => Vector3.Distance(collector.transform.position, ball.transform.position) + Vector3.Distance(ball.transform.position, golfCart.position)).Take(n);
            closestBalls.AddRange(sortedBalls);
        }

        return closestBalls.OrderBy(ball =>
        {
            bool reachable = collector.GetPathDistance(ball.transform.position, out float distance);
            return reachable ? ((int)ball.GetComponent<GolfBall>().GetLevel()) / distance : float.MinValue;
        }).LastOrDefault();
    }
}
