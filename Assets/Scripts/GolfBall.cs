using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    [SerializeField] private List<Material> materials;
    [SerializeField] private GolfBallLevel level;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetLevel(GolfBallLevel level)
    {
        this.level = level;
        meshRenderer.material = level switch
        {
            GolfBallLevel.Easy => materials[1],
            GolfBallLevel.Medium => materials[2],
            GolfBallLevel.Hard => materials[3],
            _ => meshRenderer.material
        };
    }

    public GolfBallLevel GetLevel()
    {
        return level;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        switch (level)
        {
            case GolfBallLevel.Easy:
                Gizmos.color = Color.green;
                break;
            case GolfBallLevel.Medium:
                Gizmos.color = Color.yellow;
                break;
            case GolfBallLevel.Hard:
                Gizmos.color = Color.red;
                break;
        }
        Gizmos.DrawSphere(transform.position + Vector3.up, 0.5f);
    }
}

public enum GolfBallLevel
{
    Easy = 2,
    Medium = 4,
    Hard = 8
}
