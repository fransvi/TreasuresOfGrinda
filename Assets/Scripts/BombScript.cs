using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

    public float radius;
    public float power;
    public float fusetime;
    public float speed;
    public float damage;
    public float _speed;
    public int _friendlyFireDamage;

    private bool _grounded;
    public Animator _animator;

    [SerializeField]
    private LayerMask _whatIsGround;

    public GameObject explosionSprite;
    Rigidbody2D rb;
    private bool _facingRight;


    // Use this for initialization
    void Start()
    {
        _grounded = false;
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.TransformDirection(new Vector3(speed, 0, 0));
        rb.AddForce(transform.forward * 30, ForceMode2D.Impulse);
        StartCoroutine(Explode(fusetime));
        Physics2D.IgnoreLayerCollision(18, 16);
        _animator.Play("BombFuseAnim");

        if (_facingRight)
        {
            GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(_speed, 2f, 0));
            Flip();
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(-_speed, 2f, 0));
        }

    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f, _whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                _grounded = true;
            }

        }

    }
    IEnumerator Explode(float ft)
    {

        yield return new WaitForSeconds(ft);
        StartCoroutine(ExpEffect(2f));
        Vector3 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
        foreach (Collider2D hit in colliders)
        {
            if (hit.gameObject.tag == "Enemy" && hit.gameObject.GetComponent<Rigidbody2D>())
            {
                hit.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
            }
            if(hit.gameObject.tag == "Player")
            {
                hit.gameObject.GetComponent<PlayerController>().Hurt(_friendlyFireDamage);
            }
            if (hit.gameObject.tag == "Breakable")
            {
                hit.gameObject.GetComponent<BreakableWall>().BreakWall();
            }

        }

        Destroy(this.gameObject);



    }

    public void CreateBomb(bool b, float spd)
    {
        _facingRight = b;
        _speed = spd;
    }
    IEnumerator ExpEffect(float ft)
    {
        GameObject explosion = Instantiate(explosionSprite, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, 0.5f);
        yield return new WaitForSeconds(ft);

    }
}

