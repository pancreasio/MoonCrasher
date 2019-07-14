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
    private bool baseSpawned, paused;
    private int points;
    public static int score;
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
        paused = false;
        score = 0;
    }

    private Vector2 generatePoint(Vector2 previousPoint)
    {
        int failsafe = 0;
        Vector2 newPoint = new Vector2();
        newPoint.x = Random.Range(previousPoint.x + minXOffset, previousPoint.x + maxXOffset);
        if (!baseSpawned && Random.Range(0, 100) > 90)
        {
            newPoint.y = previousPoint.y;
            newPoint.x = previousPoint.x + maxXOffset;
            baseSpawned = true;
        }
        else
        {
            newPoint.y = Random.Range(previousPoint.y - minYOffset, previousPoint.y + maxYOffset);

            while (newPoint.y > minY.transform.position.y && newPoint.y < maxY.transform.position.y && failsafe < 40)
            {
                newPoint.y = Random.Range(previousPoint.y - minYOffset, previousPoint.y + maxYOffset);
                failsafe++;
            }
        }
        return newPoint;
    }

    private void Update()
    {
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    public void Pause()
    {
        paused = !paused;
    }
}
