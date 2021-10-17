using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EyeMovement : MonoBehaviour
{
    private GameObject par;
    private float dir = 0;
    private float dir2 = 0;
    public float speed = 10f;
    public float speed2 = 10f;
    public bool mode = true;
    public float offset = 0;

    private Vector2 min = new Vector2(0, 0);
    private Vector2 max = new Vector2(0, 0);

    void Start()
    {
        par = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        float x, y, s;

        //Debug.Log(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));

        if (mode)
        {
            transform.position = par.transform.position + new Vector3(Mathf.Clamp(Mathf.Tan(dir) * 0.5f, -1.5f, 1.5f), 0 + Mathf.Clamp(dir2, -0.5f, 0.75f), -0.9f);
            x = stick(GetDir());
            y = par.GetComponent<Rigidbody2D>().velocity.y / 10;
            s = speed;
        }
        else
        {
            transform.position = par.transform.position + new Vector3(Mathf.Clamp(dir * 0.5f, -1, 1), 0 + Mathf.Clamp(dir2, -0.5f, 1), -0.9f) + Vector3.up * offset;
            Vector2 div = new Vector2(10, 10);
            Vector2 dif = Camera.main.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue())) - transform.position;
            x = dif.x / div.x;
            y = dif.y / div.y;
            s = speed2;
        }

        if (dir != x)
        {
            for (float i = 0; i < Mathf.Abs(x - dir); i++)
            {
                float dif = x - dir;
                dir += dif / (100 - s);
            }
        }
        if (dir2 != y / 10)
        {
            for (float i = 0; i < Mathf.Abs(y - dir2); i++)
            {
                float dif = y - dir2;
                dir2 += dif / (100 - s);
            }
        }

        Vector2 v = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());

        mode = (0 < v.x && v.x > Screen.width) && (0 < v.y && v.y >= Screen.height);
    }

    float GetDir()
    {
        float output = 0;
        if (Keyboard.current.aKey.isPressed) output -= 1;
        if (Keyboard.current.dKey.isPressed) output += 1;
        output = Mathf.Clamp(output, -1, 1);
        if (Gamepad.current.leftStick.x.ReadValue() != 0) output += Gamepad.current.leftStick.x.ReadValue();

        output = Mathf.Clamp(output, -1, 1);
        return output;
    }
    int stick(float i)
    {
        if (i > 0) return 1;
        if (i < 0) return -1;
        return 0;
    }
}
