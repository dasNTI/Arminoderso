using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject Player;
    private Rigidbody2D rb;
    public float Speed;
    public float yoff;
    public float ymin = 0f;

    public Vector2 dirSpeed = new Vector2(50f, 50f);
    public Vector2 forecast = new Vector2(10f, 0f);

    private Vector2 vel;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = Player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 v = rb.velocity;

        float xdif = Player.transform.position.x - transform.position.x;
        float ydif = Mathf.Clamp(Player.transform.position.y, ymin, 10000) - transform.position.y + yoff;

        if (vel.x != v.x)
        {
            for (float i = 0; i < Mathf.Abs(v.x - vel.x); i++)
            {
                float dif = v.x - vel.x;
                vel.x += dif / (200 - dirSpeed.x);
            }
        }

        if (vel.y != v.y)
        {
            for (float i = 0; i < Mathf.Abs(v.y - vel.y); i++)
            {
                float dif = v.y - vel.y;
                vel.y += dif / (200 - dirSpeed.y);
            }
        }

        //xdif += Mathf.Clamp(vel.x / 10, -1, 1) * forecast.x;
        //ydif += Mathf.Clamp(vel.y / 10, -1, 1) * forecast.y;

        transform.Translate(Mathf.Tan(xdif / (200 - Speed)), Mathf.Tan(ydif / (200 - Speed)), 0);
    }
}
