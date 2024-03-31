using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacleScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float distance = 10f;

    private Vector3 initialPosition;
    private float timer = 0f;
    private bool movingForward = true;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float movement = moveSpeed * Time.deltaTime;

        if (movingForward)
        {
            transform.Translate(Vector3.forward * movement);
            timer += movement;
            if (timer >= distance)
            {
                movingForward = false;
                timer = 0f;
            }
        }
        else
        {
            transform.Translate(Vector3.back * movement);
            timer += movement;
            if (timer >= distance)
            {
                movingForward = true;
                timer = 0f;
            }
        }
    }
}
