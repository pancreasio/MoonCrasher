using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float rotationSpeed, movementSpeed, boostSpeed;
    private Rigidbody2D rig;
    private Vector2 maxX, minX, maxY;
    private bool boost;

    private void Start()
    {
        maxX = GameObject.Find("MaxX").transform.position;
        minX = GameObject.Find("MinX").transform.position;
        maxY = GameObject.Find("Player MaxY").transform.position;
        rig = GetComponent<Rigidbody2D>();
        boost = true;
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
    }

    private void Explode()
    {
        Destroy(this.gameObject);
    }
}
