using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;
    public LineRenderer lineRenderer;
    public GameObject minX, maxX, minY, maxY, bomber;
    public Canvas mainCanvas, pauseCanvas, endCanvas, loadCanvas;
    public Button pauseButton, retryButton, nextButton;
    public Text scoreText, loadText, wonText, lostText;
    public float maxXOffset, maxYOffset, minXOffset, minYOffset;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> pointList;
    private bool baseSpawned, paused;
    private int points;
    public static int score;
    private bool levelEnded, loading;
    private float fakePercentage;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
        int bomberPoint = Random.Range(0,pointList.Count);
        Vector2 newPosition = new Vector2(pointList.ToArray()[bomberPoint].x, pointList.ToArray()[bomberPoint].y);
        Instantiate(bomber, newPosition, Quaternion.identity);

        edgeCollider = lineRenderer.gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.points = pointList.ToArray();
        paused = false;
        score = 0;
        pauseCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        levelEnded = false;
        loading = false;
        fakePercentage = 0;
    }

    private Vector2 generatePoint(Vector2 previousPoint)
    {
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

            if (newPoint.y < minY.transform.position.y)
            {
                newPoint.y = Random.Range(minY.transform.position.y, previousPoint.y + maxYOffset);
            }
            if (newPoint.y > maxY.transform.position.y)
            {
                newPoint.y = Random.Range(previousPoint.y - minYOffset, maxY.transform.position.y);
            }
        }
        return newPoint;
    }

    private void Update()
    {
        if (paused)
        {
            Time.timeScale = 0;
            pauseCanvas.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
        }
        else
        {
            if (!levelEnded)
            {
                Time.timeScale = 1;
            }
        }

        if (loading)
        {
            Time.timeScale = 1;
            fakePercentage += Time.deltaTime * 100;
            loadText.text = "Loading: " + Mathf.Round(fakePercentage).ToString() + "%";
            if (fakePercentage >= 99)
            {
                gameManager.Retry();
            }
        }
    }

    public void Pause()
    {
        pauseButton.gameObject.SetActive(!pauseButton.gameObject.activeSelf);
        pauseCanvas.gameObject.SetActive(!pauseCanvas.gameObject.activeSelf);
        paused = !paused;
    }

    public void Menu()
    {
        gameManager.Menu();
    }

    public void NextLevel()
    {
        FakeLoad();
    }

    public void GameOver()
    {
        gameManager.GameOver();
    }

    public void LevelEnded(bool won)
    {
        Time.timeScale = 0;
        levelEnded = true;
        mainCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(true);
        if (won)
        {
            wonText.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            lostText.gameObject.SetActive(true);
            retryButton.gameObject.SetActive(true);
            GameManager.score = 0;
        }
        scoreText.text = "Score: " + GameManager.score.ToString();
    }

    public void FakeLoad()
    {
        loadCanvas.gameObject.SetActive(true);
        loading = true;
    }
}
