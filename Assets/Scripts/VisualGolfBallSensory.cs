using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualGolfBallSensory : AbstractGolfBallSensory
{
    [SerializeField, Min(0.5f)] private float radius = 10f;
    [SerializeField, Range(0, 360)] private float angle = 180f;

    private static readonly string golfBallTag = "GolfBall";
    private static readonly string blackListedTag = "BlackListed";

    public override GameObject[] GetGolfBalls()
    {
        return GameObject.FindGameObjectsWithTag(golfBallTag).Where(IsGolfBallVisible).ToArray();
    }

    public override void BlackList(GameObject golfBall)
    {
        golfBall.tag = blackListedTag;
    }

    public override bool IsBlackListed(GameObject golfBall)
    {
        return golfBall.CompareTag(blackListedTag);
    }

    public override void WhiteList(GameObject golfBall)
    {
        golfBall.tag = golfBallTag;
    }

    private bool IsGolfBallVisible(GameObject golfBall)
    {
        Vector3 directionToGolfBall = (golfBall.transform.position - transform.position).normalized;
        float distanceToGolfBall = Vector3.Distance(transform.position, golfBall.transform.position);

        if (distanceToGolfBall <= radius)
        {
            float angleToGolfBall = Vector3.Angle(transform.forward, directionToGolfBall);
            if (angleToGolfBall <= angle / 2)
            {
                if (Physics.Raycast(transform.position, directionToGolfBall, out RaycastHit hit, radius))
                {
                    if (hit.collider.gameObject == golfBall)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}