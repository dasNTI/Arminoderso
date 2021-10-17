using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mask : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool a = false;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.isPressed && !a)
        {
            a = true;
            sr.enabled = !sr.enabled;
        }
        if (!Keyboard.current.rKey.isPressed) a = false;
    }
}
