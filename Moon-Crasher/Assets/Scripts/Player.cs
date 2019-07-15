using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public float fuel, rotationSpeed, movementSpeed, boostSpeed, maxLandingAngle, minLandingAngle, maxLandingSpeed, initialForce, landingCameraOffset;
    private float altitude, elapsedTime;
    public Text altText, hVelocityText, vVelocityText, fuelText, scoreText, timeText;
    public Camera mainCam, landingCam;
    public LayerMask terrainMask;
    private Rigidbody2D rig;
    private GameManager gameManager;
    private Vector2 maxX, minX, maxY, minY;
    private bool boost, landed;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        maxX = GameObject.Find("MaxX").transform.position;
        minX = GameObject.Find("MinX").transform.position;
        maxY = GameObject.Find("Player MaxY").transform.position;
        minY = GameObject.Find("MinY").transform.position;
        rig = GetComponent<Rigidbody2D>();
        boost = true;
        landed = false;
        elapsedTime = 0;
        rig.AddForce(transform.right * initialForce);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow) && fuel > 0)
        {
            rig.AddForce(transform.up * movementSpeed * Time.deltaTime);
            fuel -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space) && boost)
        {
            rig.AddForce(transform.up * boostSpeed, ForceMode2D.Impulse);
            boost = false;
        }

        if (transform.position.x < minX.x)
        {
            transform.position = new Vector2(maxX.x, transform.position.y);
        }
        if (transform.position.x > maxX.x)
        {
            transform.position = new Vector2(minX.x, transform.position.y);
        }

        if (transform.position.y > maxY.y)
        {
            Explode();
        }

        if (landed)
        {
            gameManager.GameOver();
        }

        CheckAltitude();

        altitude = transform.position.y - minY.y;
        LevelManager.score = Mathf.RoundToInt(fuel * 100 - elapsedTime * 10);
        Debug.DrawRay(transform.position, Vector3.down* 1000);

        fuelText.text = "Fuel: " + Mathf.Round(fuel) + "Lt";
        vVelocityText.text = "Vertical Velocity: " + Mathf.Round(Mathf.Abs(rig.velocity.y * 100)).ToString() + "Km/H";
        hVelocityText.text = "Horizontal Velocity: " + Mathf.Round(Mathf.Abs(rig.velocity.x * 100)).ToString() + "Km/H";
        altText.text = "Altitude: " + Mathf.Round(altitude * 10).ToString() + "M";
        scoreText.text = "Score: " + LevelManager.score.ToString() + " AP$";
        timeText.text = "Time:" + Mathf.Round(elapsedTime).ToString() + "s";
    }

    private void CheckAltitude()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, landingCameraOffset, terrainMask);
        if (hit)
        {
            landingCam.gameObject.SetActive(true);
            landingCam.transform.rotation = Quaternion.identity;
            mainCam.gameObject.SetActive(false);
        }
        else
        {
            landingCam.gameObject.SetActive(false);
            mainCam.gameObject.SetActive(true);
        }

    }


    private void Explode()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Terrain")
        {
            Bounds shipBounds = GetComponent<Collider2D>().bounds;
            Vector3 shipPosition = transform.position;

            Vector2 leftRayPos = new Vector3(shipBounds.min.x, shipPosition.y);
            Vector2 rightRayPos = new Vector3(shipBounds.max.x, shipPosition.y);

            RaycastHit2D hitLeft = Physics2D.Raycast(leftRayPos, -transform.up);
            RaycastHit2D hitRight = Physics2D.Raycast(rightRayPos, -transform.up);

            if (hitLeft && hitRight)
            {
                CheckAngle();
            }
            else
            {
                Explode();
            }
        }
    }

    private void CheckAngle()
    {
        if (transform.eulerAngles.z < maxLandingAngle || transform.eulerAngles.z > 180)
        {
            if (transform.eulerAngles.z > minLandingAngle || transform.eulerAngles.z < 180)
            {
                CheckSpeed();
            }
            else
            {
                Explode();
            }
        }
        else
        {
            Explode();
        }
    }

    private void CheckSpeed()
    {
        if (rig.velocity.magnitude < maxLandingSpeed)
        {
            landed = true;
        }
        else
        {
            Explode();
        }
    }
}
