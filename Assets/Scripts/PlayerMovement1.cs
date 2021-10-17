using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerMovement1 : MonoBehaviour
{
    public float speed = 20f;
    private float move = 0f;
    private float dir = 0;
    public float JumpHeight = 20f;
    public float dirSpeed = 95f;
    public float maxangleButtom = 20f;
    public float maxangleTop = 20f;

    public bool respawn = true;
    public float respawny = -10f;

    public bool Squishy = true;
    public float SquishMul = 1f;
    public PolygonCollider2D bc;
    public LayerMask lm;
    public Rigidbody2D rb;
    public Animator ani;

    private void Start()
    {
        bc = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        move = stick(GetDir());

        if ((TouchingLeft() && move < 0) || (TouchingRight() && move > 0)) rb.velocity = new Vector2(0, rb.velocity.y);

        rb.velocity = new Vector2((TouchingLeft() && move < 0) || (TouchingRight() && move > 0) ? 0 : dir * speed, rb.velocity.y);
        transform.localScale.Set(1, 1 + rb.velocity.y, 1);

        if (dir != move && !((TouchingLeft() && move < 0) || (TouchingRight() && move > 0)))
        {
            for (float i = 0; i < Mathf.Abs(move - dir); i++)
            {
                float dif = move - dir;
                dir += dif / (100 - dirSpeed);
            }
        }

        if (TouchingGround())
        {
            if (Keyboard.current.spaceKey.isPressed) rb.velocity = new Vector2(rb.velocity.x, JumpHeight);
        }

        if (stick(GetDir()) != 0)
        {
            if (!ani.GetBool("walking"))
            {
                float f = 0;
                DOTween.To(() => f, x => f = x, 1f, 0.4f).OnUpdate(() => {
                    ani.SetFloat("Blend", f);
                }); 
            }
            
            ani.SetBool("walking", true);
        }else
        {
            if (ani.GetBool("walking"))
            {
                float f = 1f;
                DOTween.To(() => f, x => f = x, 0f, 0.4f).OnUpdate(() => {
                    ani.SetFloat("Blend", f);
                });
            }

            ani.SetBool("walking", false);
        }

        if (transform.position.y <= respawny && respawn) Respawn();
    }

    float GetDir()
    {
        float output = 0;
        if (Keyboard.current.aKey.isPressed) output -= 1; 
        if (Keyboard.current.dKey.isPressed) output += 1;
        output = Mathf.Clamp(output, -1, 1);

        //if (Gamepad.current.leftStick.x.ReadValue() != 0) output += Gamepad.current.leftStick.x.ReadValue();

        output = Mathf.Clamp(output, -1, 1);
        return output;
    }

    float ButtomAngle()
    {
        return Mathf.Tan(2*Mathf.PI* maxangleButtom / 360) * bc.bounds.extents.x;
    }

    float TopAngle()
    {
        return Mathf.Tan(2 * Mathf.PI * maxangleTop / 360) * bc.bounds.extents.x;
    }

    bool TouchingGround()
    {
        float extraHeight = 0.1f;
        float offset = 0.001f;

        RaycastHit2D raycast = Physics2D.Raycast(bc.bounds.center + new Vector3(bc.bounds.extents.x + offset, 0, 0), Vector2.down, bc.bounds.extents.y + extraHeight, lm);
        RaycastHit2D raycast2 = Physics2D.Raycast(bc.bounds.center - new Vector3(bc.bounds.extents.x + offset, 0, 0), Vector2.down, bc.bounds.extents.y + extraHeight, lm);
        RaycastHit2D raycast3 = Physics2D.Raycast(bc.bounds.center, Vector2.down, bc.bounds.extents.y + extraHeight, lm);

        Color rayColor;

        if (raycast.collider != null) {rayColor = Color.green;}
        else{rayColor = Color.red;}

        Debug.DrawRay(bc.bounds.center + new Vector3(bc.bounds.extents.x + offset, 0, 0), Vector2.down * (bc.bounds.extents.y + extraHeight), rayColor);

        if (raycast2.collider != null) { rayColor = Color.green; }
        else { rayColor = Color.red; }

        Debug.DrawRay(bc.bounds.center - new Vector3(bc.bounds.extents.x + offset, 0, 0), Vector2.down * (bc.bounds.extents.y + extraHeight), rayColor);

        if (raycast3.collider != null) { rayColor = Color.green; }
        else { rayColor = Color.red; }

        Debug.DrawRay(bc.bounds.center, Vector2.down * (bc.bounds.extents.y + extraHeight), rayColor);

        return raycast.collider != null || raycast2.collider != null || raycast3.collider != null;
    }

    bool TouchingRight()
    {
        float offset = 0.2f;

        RaycastHit2D ray1 = Physics2D.Raycast(bc.bounds.center + new Vector3(bc.bounds.extents.x + offset, bc.bounds.extents.y - TopAngle(), 0), Vector2.down, bc.bounds.extents.y * 2 - ButtomAngle() - TopAngle(), lm);

        Color rayColor;

        if (ray1.collider != null) { rayColor = Color.green; }
        else { rayColor = Color.red; }

        Debug.DrawRay(bc.bounds.center + new Vector3(bc.bounds.extents.x + offset, bc.bounds.extents.y, 0), Vector2.down * bc.bounds.extents.y * 2, rayColor);

        return ray1.collider != null;
    }

    bool TouchingLeft()
    {
        float offset = 0.2f;

        RaycastHit2D ray2 = Physics2D.Raycast(bc.bounds.center + new Vector3(-bc.bounds.extents.x - offset, bc.bounds.extents.y - TopAngle(), 0), Vector2.down, bc.bounds.extents.y * 2 - ButtomAngle() - TopAngle(), lm);

        Color rayColor;

        if (ray2.collider != null) { rayColor = Color.green; }
        else { rayColor = Color.red; }

        Debug.DrawRay(bc.bounds.center + new Vector3(-bc.bounds.extents.x - offset, bc.bounds.extents.y, 0), Vector2.down * bc.bounds.extents.y * 2 , rayColor);

        return ray2.collider != null;
    }


    void Respawn()
    {
        GameObject spawn = GameObject.FindGameObjectWithTag("Respawn");
        transform.localScale = Vector3.zero;
        spawn.GetComponent<ParticleSystem>().Play();
        //GameObject.FindGameObjectWithTag("DeathFlash").GetComponent<Animation>().Play();

        IEnumerator tu()
        {
            for (int i = 0; i < 20; i++)
            {
                rb.velocity = Vector2.zero;
                transform.position = new Vector3(
                    spawn.transform.position.x,
                    spawn.transform.position.y - bc.bounds.extents.y,
                    transform.position.z
                );
                transform.localScale += Vector3.one / 20;
                yield return new WaitForEndOfFrame();
            }
            for (int i = 0; i < 10; i++)
            {
                rb.velocity = Vector2.zero;
                transform.position = new Vector3(
                    spawn.transform.position.x,
                    spawn.transform.position.y - bc.bounds.extents.y,
                    transform.position.z
                );
                yield return new WaitForEndOfFrame();
                dir = 0;
            }
        }
        StartCoroutine(tu());
    }
    private int stick(float i)
    {
        //int v = 0;
        if (i > 0) return 1;
        if (i < 0) return -1;
        return 0;
    }
}
