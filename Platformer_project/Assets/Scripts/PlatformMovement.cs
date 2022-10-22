using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    [SerializeField] private float moveTime;
    [SerializeField] private Vector2 direction;
    private float timer = 0;

    private enum State { Moving, Waiting }
    private State state;

    private void Update()
    {
        timer += Time.deltaTime;
        if (state == State.Moving)
        {
            if (timer >= moveTime)
            {
                state = State.Waiting;
                direction *= -1;
                timer = 0f;
            }
            else
            {
                transform.Translate(speed * Time.deltaTime * direction);
            }
        }
        else
        {
            if (timer >= waitTime)
            {
                state = State.Moving;
                timer = 0f;
            }
        }

    }
}
