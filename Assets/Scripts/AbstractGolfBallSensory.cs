using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public abstract class AbstractGolfBallSensory : MonoBehaviour
{
    public abstract GameObject[] GetGolfBalls();
    public abstract void BlackList(GameObject golfBall);
    public abstract bool IsBlackListed(GameObject golfBall);
    public abstract void WhiteList(GameObject golfBall);
}
