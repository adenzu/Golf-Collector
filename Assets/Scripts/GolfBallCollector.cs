using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent), typeof(Collider))]
public class GolfBallCollector : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxGolfBalls = 1;
    [SerializeField] private AbstractGolfBallSensory golfBallSensory;
    [SerializeField] private AbstractGolfCollectorDecisionMaker decisionMaker;
    [SerializeField] private Transform golfCart;
    [SerializeField, Min(0f)] private float startHealth = 100f;
    [SerializeField, Min(0f)] private float damagePerSecond = 1f;
    [SerializeField, Min(0f)] private float abandonAfterSeconds = 1f;

    private Health health;
    [SerializeField] private List<GolfBallLevel> golfBallsOnPerson = new List<GolfBallLevel>();
    private GameObject currentGolfBall;
    private UnityEngine.AI.NavMeshAgent agent;
    private Dictionary<GameObject, float> golfBallTimers = new Dictionary<GameObject, float>();
    private Score score;

    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        health = new Health(startHealth);
        score = new Score();

        health.OnHealthChanged += (newHealth, maxHealth) => UIManager.Instance.UpdateHealthUI((int)newHealth, (int)maxHealth);
        score.OnScoreChanged += newScore => UIManager.Instance.UpdateScoreUI(newScore);

        decisionMaker.DecideFor(this);
    }

    private void Update()
    {
        DecreaseHealth(Time.deltaTime);
        if (IsDead())
        {
            agent.isStopped = true;
            return;
        }

        if (currentGolfBall != null && golfBallTimers.ContainsKey(currentGolfBall))
        {
            golfBallTimers[currentGolfBall] -= Time.deltaTime;
            if (golfBallTimers[currentGolfBall] <= 0)
            {
                BlackList(currentGolfBall);
                SetCurrentGolfBall(null);
            }
        }

        if (CanGoForABetterBall(out GameObject bestGolfBall))
        {
            GoFor(bestGolfBall);
        }
        else if (currentGolfBall == null && golfBallsOnPerson.Count >= maxGolfBalls)
        {
            ReturnToGolfCart();
        }
    }

    private void SetCurrentGolfBall(GameObject golfBall)
    {
        currentGolfBall = golfBall;
        if (golfBall != null && GetPathDistance(currentGolfBall.transform.position, out float distance))
        {
            float timeToReach = distance / agent.speed;
            golfBallTimers[currentGolfBall] = timeToReach + abandonAfterSeconds;
        }
    }

    private void BlackList(GameObject golfBall)
    {
        golfBallSensory.BlackList(golfBall);
    }

    private bool IsBlackListed(GameObject golfBall)
    {
        return golfBallSensory.IsBlackListed(golfBall);
    }

    private void WhiteList(GameObject golfBall)
    {
        golfBallSensory.WhiteList(golfBall);
    }

    private void DecreaseHealth(float deltaTime)
    {
        health.DecreaseHealth(damagePerSecond * deltaTime);
    }

    private bool IsDead()
    {
        return health.GetCurrentHealth() <= 0;
    }

    private bool CanGoForABetterBall(out GameObject bestGolfBall)
    {
        GameObject[] golfBalls = golfBallSensory.GetGolfBalls();
        if (golfBalls.Length == 0)
        {
            bestGolfBall = null;
            return false;
        }

        GameObject candidate = decisionMaker.GetBestGolfBall(golfBalls);
        if (candidate == null)
        {
            bestGolfBall = null;
            return false;
        }

        if (!WorthGoingFor(candidate))
        {
            bestGolfBall = null;
            return false;
        }

        bestGolfBall = candidate;
        return true;
    }

    private bool WorthGoingFor(GameObject golfBall)
    {
        if (golfBallsOnPerson.Count < maxGolfBalls)
        {
            return true;
        }

        GolfBallLevel minLevel = golfBallsOnPerson.Min();

        float minScorePerDistance = ((int)minLevel) / Vector3.Distance(transform.position, golfCart.position);
        float scorePerDistance = ((int)golfBall.GetComponent<GolfBall>().GetLevel()) / (Vector3.Distance(transform.position, golfBall.transform.position) + Vector3.Distance(golfBall.transform.position, golfCart.position));

        return scorePerDistance > minScorePerDistance;
    }

    private void GoFor(GameObject bestGolfBall)
    {
        SetCurrentGolfBall(bestGolfBall);
        agent.SetDestination(currentGolfBall.transform.position);
    }

    private void ReturnToGolfCart()
    {
        agent.SetDestination(golfCart.position);
    }

    public bool GetPathDistance(Vector3 destination, out float distance)
    {
        if (agent == null)
        {
            distance = float.MaxValue;
            return false;
        }

        if (CalculatePath(destination, out UnityEngine.AI.NavMeshPath path))
        {
            distance = 0f;
            if (path.corners.Length > 1)
            {
                for (int i = 1; i < path.corners.Length; i++)
                {
                    distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }
            return true;
        }
        else
        {
            distance = float.MaxValue;
            return false;
        }
    }

    public bool CalculatePath(Vector3 destination, out UnityEngine.AI.NavMeshPath path)
    {
        path = new UnityEngine.AI.NavMeshPath();
        return agent.CalculatePath(destination, path);
    }

    private bool SwapLeastValuableBall(GameObject other, out GolfBallLevel? swappedLevel, bool forceSwap = false)
    {
        if (golfBallsOnPerson.Count == 0)
        {
            swappedLevel = null;
            return false;
        }

        int minIndex = 0;
        GolfBallLevel minLevel = golfBallsOnPerson[0];
        for (int i = 1; i < golfBallsOnPerson.Count; i++)
        {
            if (golfBallsOnPerson[i] < minLevel)
            {
                minIndex = i;
                minLevel = golfBallsOnPerson[i];
            }
        }

        GolfBall otherGolfBall = other.GetComponent<GolfBall>();
        GolfBallLevel otherLevel = otherGolfBall.GetLevel();
        if (!forceSwap && minLevel >= otherLevel)
        {
            swappedLevel = null;
            return false;
        }

        swappedLevel = minLevel;
        otherGolfBall.SetLevel(minLevel);
        golfBallsOnPerson[minIndex] = otherLevel;
        return true;
    }

    private void TakeGolfBall(GameObject golfBall)
    {
        GolfBallLevel level = golfBall.GetComponent<GolfBall>().GetLevel();

        if (golfBallsOnPerson.Count < maxGolfBalls)
        {
            Debug.Log($"Collected a {level} golf ball");
            golfBallsOnPerson.Add(level);
            Destroy(golfBall);
            SetCurrentGolfBall(null);
        }
        else if (SwapLeastValuableBall(golfBall, out GolfBallLevel? swappedLevel))
        {
            Debug.Log($"Swapped a {level} golf ball for a {swappedLevel} golf ball");
            SetCurrentGolfBall(null);
        }
        else
        {
            BlackList(golfBall);
        }
    }

    private void StashGolfBalls()
    {
        foreach (GolfBallLevel level in golfBallsOnPerson)
        {
            score.IncreaseScore((int)level);
        }
        golfBallsOnPerson.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == currentGolfBall)
        {
            TakeGolfBall(other.gameObject);
        }
        else if (other.transform == golfCart)
        {
            StashGolfBalls();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsBlackListed(other.gameObject))
        {
            TakeGolfBall(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (currentGolfBall == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, currentGolfBall.transform.position);
    }
}
