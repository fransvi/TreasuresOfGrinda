using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletScript : MonoBehaviour {

    [SerializeField]
    private int _bulletDamage;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _enemyProjectile;
    [SerializeField]
    private bool _dartTrapProjectile;
    [SerializeField]
    private Sprite[] _projectileSprites;

    private bool _facingRight;

    private int _bulletType;
    public Animator _animator;
    private bool _ignoreGroundCollider;

    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        if (_bulletType == 0)
        {
            GetComponent<SpriteRenderer>().sprite = _projectileSprites[0];
            _animator.Play("arrow");
        }
        else if(_bulletType == 1)
        {
 
            GetComponent<SpriteRenderer>().sprite = _projectileSprites[1];
            _animator.Play("FireBallAnim");
        }

        if (!_dartTrapProjectile)
        {
            GetComponent<SpriteRenderer>().sprite = _projectileSprites[2];
            Physics2D.IgnoreLayerCollision(18, 0, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(18, 0, false);
        }

        if (_dartTrapProjectile)
        {
            StartCoroutine(IgnoreGround());
        }
        Physics2D.IgnoreLayerCollision(18, 16);
        if (_facingRight)
        {
            GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(_speed, 2f, 0));
            if(_bulletType == 0)
            {
                Flip();
            }

        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(-_speed, 2f, 0));
            if (_bulletType == 1)
            {
                Flip();
            }
        }
        if(_bulletType == 0)
        {
            GetComponent<Rigidbody2D>().AddForce(transform.forward * 30, ForceMode2D.Impulse);
        }
        else if(_bulletType == 1)
        {
            GetComponent<Rigidbody2D>().AddForce(transform.forward * 30, ForceMode2D.Impulse);
        }
        if(_bulletType == 1)
        {
            Destroy(gameObject, 8f);
        }
        else
        {
            Destroy(gameObject, 3f);
        }


    }

    IEnumerator IgnoreGround()
    {
        _ignoreGroundCollider = true;
        yield return new WaitForSeconds(1f);
        _ignoreGroundCollider = false;
    }

    public void createBullet(bool fr, float bf, int type, bool dart, int bd)
    {
        _facingRight = fr;
        _speed = bf;
        _bulletType = type;
        _dartTrapProjectile = dart;
        _bulletDamage = bd;
        if(_speed < 10)
        {
            _speed = 10;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && _dartTrapProjectile)
        {
            other.gameObject.GetComponent<PlayerController>().Hurt(1);
            if(_bulletType != 1)
            {
                Destroy(gameObject);
            }

        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<EnemyScript>().TakeDamage(_bulletDamage);
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && !_dartTrapProjectile)
        {

            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Spikes") && !_dartTrapProjectile)
        {
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform") && !_dartTrapProjectile)
        {
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Stairs") && !_dartTrapProjectile)
        {
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("PressurePad") && !_dartTrapProjectile)
        {
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        }

    }

    private void Flip()
    {
        // Switch player sprite heading
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    // Update is called once per frame
    void Update () {

        if (transform.GetComponent<Rigidbody2D>().velocity.x > 0 && !_facingRight)
        {
            //Flip();
        }
        else if (transform.GetComponent<Rigidbody2D>().velocity.x < 0 && _facingRight)
        {
            //Flip();
        }

    }
}
