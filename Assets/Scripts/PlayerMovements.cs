using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements1 : MonoBehaviour
{
    public float speed = 1f;
    private float move = 0;
    private float dir = 0;
    public float JumpHeight = 5f;
    public float dirSpeed = 1f;
    private float inityscale;

    public bool respawn = true;
    public float respawny = -10f;

    private Vector3 vel;

    public PolygonCollider2D bc;
    public LayerMask lm;
    public Rigidbody2D rb;

    private void Start()
    {
        bc = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        inityscale = bc.bounds.extents.y * 2;
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        move = -Input.GetAxis("Horizontal").CompareTo(0);
        if (Input.GetAxis("Horizontal") == 0) move = 0;
        Debug.Log(Input.GetAxis("Horizontal"));

        if ((TouchingLeft() && move < 0) || (TouchingRight() && move > 0)) rb.velocity = new Vector2(0, rb.velocity.y);

        rb.velocity = new Vector2((TouchingLeft() && move < 0) || (TouchingRight() && move > 0) ? 0 : dir * speed, rb.velocity.y);
        transform.localScale.Set(1, 1 + rb.velocity.y, 1);

        if (dir != move && !((TouchingLeft() && move < 0) || (TouchingRight() && move > 0)))
        {
            for (float i = 0; i < Mathf.Abs(move - dir); i++)
            {
                float dif = Input.GetAxis("Horizontal") - dir;
                dir += dif / (100 - dirSpeed);
            }
        }
        Debug.Log(dir);
        if (TouchingGround())
        {
            if (Input.GetAxis("Jump") != 0) rb.velocity = new Vector2(rb.velocity.x, JumpHeight);
        }

        if (transform.position.y <= respawny && respawn) Respawn();
    }

    bool TouchingGround()
    {
        float extraHeight = 0.1f;
        float offset = 0.001f;

        RaycastHit2D raycast = Physics2D.Raycast(bc.bounds.center + new Vector3(bc.bounds.extents.x + offset, 0, 0), Vector2.down, bc.bounds.extents.y + extraHeight, lm);
        RaycastHit2D raycast2 = Physics2D.Raycast(bc.bounds.center - new Vector3(bc.bounds.extents.x + offset, 0, 0), Vector2.down, bc.bounds.extents.y + extraHeight, lm);
        RaycastHit2D raycast3 = Physics2D.Raycast(bc.bounds.center, Vector2.down, bc.bounds.extents.y + extraHeight, lm);

        Color rayColor;

        if (raycast.collider != null) { rayColor = Color.green; }
        else { rayColor = Color.red; }

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
        float offset = 0.1f;
        RaycastHit2D ray1 = Physics2D.Raycast(bc.bounds.center + new Vector3(bc.bounds.extents.x + offset, bc.bounds.extents.y, 0), Vector2.down, bc.bounds.extents.y * 2, lm);

        Color rayColor;

        if (ray1.collider != null) { rayColor = Color.green; }
        else { rayColor = Color.red; }

        Debug.DrawRay(bc.bounds.center + new Vector3(bc.bounds.extents.x + offset, bc.bounds.extents.y, 0), Vector2.down * bc.bounds.extents.y * 2, rayColor);

        return ray1.collider != null;
    }

    bool TouchingLeft()
    {
        float offset = 0.1f;
        RaycastHit2D ray2 = Physics2D.Raycast(bc.bounds.center + new Vector3(-bc.bounds.extents.x - offset, bc.bounds.extents.y, 0), Vector2.down, bc.bounds.extents.y * 2, lm);

        Color rayColor;

        if (ray2.collider != null) { rayColor = Color.green; }
        else { rayColor = Color.red; }

        Debug.DrawRay(bc.bounds.center + new Vector3(-bc.bounds.extents.x - offset, bc.bounds.extents.y, 0), Vector2.down * bc.bounds.extents.y * 2, rayColor);

        return ray2.collider != null;
    }


    void Respawn()
    {
        GameObject spawn = GameObject.FindGameObjectWithTag("Respawn");
        transform.localScale = Vector3.zero;
        spawn.GetComponent<ParticleSystem>().Play();

        IEnumerator tu()
        {
            SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < 20; i++)
            {
                rb.velocity = Vector2.zero;
                transform.position = new Vector3(
                    spawn.transform.position.x,
                    spawn.transform.position.y,
                    transform.position.z
                );
                transform.localScale += Vector3.one / 20;
                yield return new WaitForSecondsRealtime(1/60);
            }
            for (int i = 0; i < 10; i++)
            {
                rb.velocity = Vector2.zero;
                transform.position = new Vector3(
                    spawn.transform.position.x,
                    spawn.transform.position.y,
                    transform.position.z
                );
                yield return new WaitForSecondsRealtime(1 / 60);
            }
        }
        StartCoroutine(tu());
    }
}
