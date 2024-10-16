using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomGolfBallInitializer : AbstractGolfBallInitializer
{
    public override void Initialize(GameObject golfBall)
    {
        GolfBall golfBallComponent = golfBall.GetComponent<GolfBall>();
        golfBallComponent.SetLevel(Utils.GetRandomEnumValue<GolfBallLevel>());
    }
}
