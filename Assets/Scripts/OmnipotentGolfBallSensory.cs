using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmnipotentGolfBallSensory : AbstractGolfBallSensory
{
    private static readonly string golfBallTag = "GolfBall";
    private static readonly string blackListedTag = "BlackListed";

    public override GameObject[] GetGolfBalls()
    {
        return GameObject.FindGameObjectsWithTag(golfBallTag);
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
}
