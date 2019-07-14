using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject minX, maxX, minY, maxY;
    public float maxXOffset, maxYOffset, minXOffset, minYOffset;
    private int points;
    private void Start()
    {
        points = 0;
        lineRenderer.SetPosition(points, new Vector2(minX.transform.position.x, minX.transform.position.y));
        int failsafe = 0;
        while (lineRenderer.GetPosition(points).x < maxX.transform.position.x && failsafe < 30)
        {
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(points+1, generatePoint(lineRenderer.GetPosition(points)));
            failsafe++;
            points++;
        }
        lineRenderer.SetPosition(points, maxX.transform.position);
    }

    private Vector2 generatePoint(Vector2 previousPoint)
    {
        int failsafe = 0;
        Vector2 newPoint = new Vector2();
        newPoint.x = Random.Range(previousPoint.x + minXOffset, previousPoint.x + maxXOffset);
        newPoint.y = Random.Range(previousPoint.y - minYOffset, previousPoint.y + maxYOffset);
        while (newPoint.y > minY.transform.position.y && newPoint.y < maxY.transform.position.y && failsafe < 10)
        {
            newPoint.y = Random.Range(previousPoint.y - minYOffset, previousPoint.y + maxYOffset);
            failsafe++;
        }
        return newPoint;
    }

    private void Update()
    {

    }
}
