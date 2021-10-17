using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bump : MonoBehaviour
{
    public float MaxBump = 1.0f;
    public float MinBump = 0.9f;
    private float dif;
    public float speed = 10f;
    private bool Dir = true; // true = down, false = up
    private float State;

    void Start()
    {
        dif = MaxBump - MinBump;
        State = MaxBump;
    }

    
    void Update()
    {
        if (Dir)
        {
            State -= dif / speed;
            transform.Translate(0, (State - 1) / speed, 0);
            if (State <= MinBump) Dir = false;
        }else
        {
            State += dif / speed;
            transform.Translate(0, -(State - 1) / speed, 0);
            if (State >= MaxBump) Dir = true;
        }
        transform.localScale = new Vector3(1, State, 1);
    }
}
