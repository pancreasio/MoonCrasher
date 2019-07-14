using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject minX, maxX, minY, maxY;
    public float maxXOffset, maxYOffset, minXOffset, minYOffset;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> pointList;
    private bool baseSpawned;
    private int points;
    private void Start()
    {
        pointList = new List<Vector2>();
        pointList.Clear();
        points = 0;
        baseSpawned = false;
        lineRenderer.SetPosition(points, new Vector2(minX.transform.position.x, minX.transform.position.y));
        pointList.Add(lineRenderer.GetPosition(points));
        int failsafe = 0;
        while (lineRenderer.GetPosition(points).x < maxX.transform.position.x && failsafe < 60)
        {
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(points + 1, generatePoint(lineRenderer.GetPosition(points)));
            pointList.Add(lineRenderer.GetPosition(points));
            failsafe++;
            points++;
        }
        if (!baseSpawned)
        {
            lineRenderer.SetPosition(points, new Vector2(maxX.transform.position.x, lineRenderer.GetPosition(points - 1).y));
            pointList.Add(lineRenderer.GetPosition(points - 1));
        }
        edgeCollider = lineRenderer.gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.points = pointList.ToArray();
    }

    private Vector2 generatePoint(Vector2 previousPoint)
    {
        int failsafe = 0;
        Vector2 newPoint = new Vector2();
        newPoint.x = Random.Range(previousPoint.x + minXOffset, previousPoint.x + maxXOffset);
        if (!baseSpawned && Random.Range(0, 100) > 80)
        {
            newPoint.y = previousPoint.y;
            baseSpawned = true;
        }
        else
        {
            newPoint.y = Random.Range(previousPoint.y - minYOffset, previousPoint.y + maxYOffset);

            while (newPoint.y > minY.transform.position.y && newPoint.y < maxY.transform.position.y && failsafe < 10)
            {
                newPoint.y = Random.Range(previousPoint.y - minYOffset, previousPoint.y + maxYOffset);
                failsafe++;
            }
        }
        return newPoint;
    }

    private void Update()
    {

    }
}
