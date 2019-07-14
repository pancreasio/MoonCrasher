using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float rotationSpeed, movementSpeed, boostSpeed, maxLandingAngle, minLandingAngle, maxLandingSpeed;
    private Rigidbody2D rig;
    private GameManager gameManager;
    private Vector2 maxX, minX, maxY;
    private bool boost;
    private bool landed;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        maxX = GameObject.Find("MaxX").transform.position;
        minX = GameObject.Find("MinX").transform.position;
        maxY = GameObject.Find("Player MaxY").transform.position;
        rig = GetComponent<Rigidbody2D>();
        boost = true;
        landed = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rig.AddForce(transform.up * movementSpeed * Time.deltaTime);
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
        if (transform.eulerAngles.z<maxLandingAngle || transform.eulerAngles.z > 180)
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
