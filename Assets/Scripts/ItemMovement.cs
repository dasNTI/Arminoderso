using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemMovement : MonoBehaviour
{
    private GameObject Player;
    private float dir = 0;
    public Vector3 offset = new Vector3(2.9f, -1, 3);
    public float speed = 85;
    private int d = 1;
    public bool switchSides = false;
    public bool enable;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        dir = offset.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + new Vector3(dir, offset.y, 0);
        if (Mathf.Abs(transform.eulerAngles.z - 180) > 90)
        {
            transform.position -= new Vector3(0, 0, -offset.z);
        }else
        {
            transform.position -= new Vector3(0, 0, offset.z);
        }


        Vector3 f = Player.transform.position - Camera.main.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        Vector3 dif = f + new Vector3(d * 2, 0, 0);

        Vector3 pos = transform.position - Camera.main.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));

        float a = Mathf.Rad2Deg * Mathf.Atan2(pos.y, pos.x);
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            a + 180
        );

        if (enable)
        {
            if (!switchSides)
            {
                if (dif.x < 0)
                {
                    d = -1;
                    if (dir != offset.x)
                    {
                        for (float i = 0; i < Mathf.Abs(offset.x - dir); i++)
                        {
                            float d = offset.x - dir;
                            dir += d / (100 - speed);
                        }
                    }
                }
                else
                {
                    d = 1;
                    if (dir != -offset.x)
                    {
                        for (float i = 0; i < Mathf.Abs(-offset.x - dir); i++)
                        {
                            float d = -offset.x - dir;
                            dir += d / (100 - speed);
                        }
                    }
                }
            }
        }
    }
}
