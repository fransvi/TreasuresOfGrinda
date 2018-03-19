using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
    [SerializeField]
    private float _health;
    [SerializeField]
    private float _knockbackForce; private bool _takingDamage;
    [SerializeField]
    private GameObject _player;
    public Animator _animator;
    [SerializeField]
    private GameObject _hurtPoint;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private int _meleeDamage;
    [SerializeField]
    private bool _facingRight;
    [SerializeField]
    private GameObject _deathAnim;
    [SerializeField]
    private GameObject _stunnedAnim;
    [SerializeField]
    private GameObject _skeletonSkullPrefab;
    [SerializeField]
    private GameObject _hitAnim;
    private Rigidbody2D _rigidBody;
    private Color _color;
    public float _viewDistance;
    private Transform _center;
    private float _minDistance = 0.05f;
    private bool _stunned;
    private bool _attackOnCooldown;
    public float _attackCooldown;
    public float _attackRange;
    public float _jumpForce; private bool _moving;
    private bool _jumpCooldown;
    public bool _grounded;
    public float _groundedRadius;
    public LayerMask _whatIsGround;
    private bool _idle;

    /*
    EnemyTypes
    0 = Goblin
    1 = Bat
    2 = Skeleton
    3 = SkeletonHead
    */

    [SerializeField]
    private int enemyType;

    // Use this for initialization
    void Start() {
        _moving = false;
        _animator = GetComponent<Animator>();
        _idle = true; _takingDamage = false;
        _stunned = false;
        _rigidBody = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(_player.GetComponent<CapsuleCollider2D>(), GetComponent<BoxCollider2D>());
        _color = gameObject.GetComponent<Renderer>().material.color;
        Physics2D.IgnoreLayerCollision(10, 16, true);
        Physics2D.IgnoreLayerCollision(10, 0, true);
        _player = GameObject.FindWithTag("Player");
        _center = transform.Find("CenterPoint");


        if(enemyType == 0)
        {
            Physics2D.IgnoreLayerCollision(10, 9, false);
        }
        else if (enemyType == 1)
        {
            Physics2D.IgnoreLayerCollision(10, 9, true);
            GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
        else if(enemyType == 2)
        {
            Physics2D.IgnoreLayerCollision(10, 9, false);
        }
        else if(enemyType == 3)
        {
            Physics2D.IgnoreLayerCollision(10, 9, false);          
        }
    }

    // Update is called once per frame
    void Update() {


        if (enemyType == 0 && !_takingDamage && !_moving)
        {
            _animator.Play("GoblinMove");
        }
        else if(enemyType == 1 && !_takingDamage && !_moving)
        {
            _animator.Play("BatIdle");
        }else if(enemyType == 1 && !_takingDamage && _moving)
        {
            _animator.Play("BatMove");
        }
        else if (enemyType == 2 && !_takingDamage && !_moving)
        {
            _animator.Play("SkeletonMove");
        }
        else if(enemyType == 3 && !_takingDamage)
        {
            _animator.Play("SkullIdle");
        }
        _grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_center.transform.position, _groundedRadius, _whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                //_gravity = Physics2D.gravity;
                _grounded = true;
            }

        }

        float distance = Vector2.Distance(transform.position, _player.transform.position);
        if (_stunned)
        {
            _rigidBody.velocity = Vector2.zero;
        }else if (_attackOnCooldown && distance < 4f)
        {
            transform.position = Vector2.MoveTowards(transform.position, -_player.transform.position, _moveSpeed * Time.deltaTime);
        }
        else if(distance < _attackRange)
        {
            MeleeAttack();
            StartCoroutine(AttackCooldown());
        }
        else if (distance < _viewDistance && distance > _minDistance && !_stunned)
        {
            _moving = true;


            if (enemyType == 0 && !_takingDamage)
            {
                _animator.Play("GoblinMoveFast");
            }else if(enemyType == 2 && !_takingDamage)
            {
                _animator.Play("SkeletonMoveFast");
            }
            //Enemy hopping
            if (enemyType == 3)
            {
                if (transform.position.x < _player.transform.position.x && _grounded && !_jumpCooldown)
                {
                    StartCoroutine(JumpMovement(1));
                }
                else if (transform.position.x > _player.transform.position.x && _grounded && !_jumpCooldown )
                {
                    StartCoroutine(JumpMovement(-1));
                }

            }
            //Normal Movement
            else if(enemyType != 3)
            {
    
                transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _moveSpeed * Time.deltaTime);
            }
          
        }
        else
        {
        }

        if (_health <= 0)
        {
            Die();
        }
        if (transform.position.x < _player.transform.position.x && !_facingRight)
        {
            Flip();
        }
        else if (transform.position.x > _player.transform.position.x && _facingRight)
        {
            Flip();
        }



    }
    IEnumerator AttackCooldown()
    {
        _attackOnCooldown = true;
        yield return new WaitForSeconds(_attackCooldown);
        _attackOnCooldown = false;
    }
    //Placeholder for hurt anim
    IEnumerator HurtAnim()
    {
        _takingDamage = true;
        Renderer rend = gameObject.GetComponent<Renderer>();
        Vector3 newPos = new Vector3(_center.position.x, _center.position.y, _center.position.z - 5);
        GameObject hitEffect = Instantiate(_hitAnim, newPos, _center.rotation);
        Destroy(hitEffect, 0.25f);
        if(enemyType == 0)
        {
            _animator.Play("GoblinHit");
        }else if(enemyType == 1)
        {
            _animator.Play("BatHit");
        }else if(enemyType == 2)
        {
            _animator.Play("SkeletonHit");
        }else if(enemyType == 3)
        {
            _animator.Play("SkullHit");
        }
        yield return new WaitForSeconds(0.25f);
        /*
        for (int i = 0; i < 2; i++)
        {
            rend.material.color = new Color(1f, 1f, 1f, 0f);
 

            yield return new WaitForSeconds(0.1f);
            rend.material.color = _color;
            yield return new WaitForSeconds(0.1f);
        }
        */
        _takingDamage = false;
    }

    private void Flip()
    {
        // Switch player sprite heading
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    public void TakeDamage(float damage)
    {
        StartCoroutine(HurtAnim());
        if (transform.position.x < _player.transform.position.x)
        {
            //knock vasempaan
            _rigidBody.velocity = Vector2.zero;
            if(enemyType == 0 || enemyType == 2)
            {
                _rigidBody.AddForce(new Vector2(-_knockbackForce, _knockbackForce));
            }

        }
        else if (transform.position.x > _player.transform.position.x)
        {
            //knock oikealle
            _rigidBody.velocity = Vector2.zero;
            if (enemyType == 0 || enemyType == 2)
            {
                _rigidBody.AddForce(new Vector2(_knockbackForce, _knockbackForce));
            }

        }
        _health -= damage;
    }

    public void MaceStun()
    {
        StartCoroutine(Stunned());
    }
    IEnumerator Stunned()
    {
        _stunned = true;
        _stunnedAnim.SetActive(true);
        yield return new WaitForSeconds(2f);
        _stunnedAnim.SetActive(false);
        _stunned = false;
    }
    IEnumerator JumpMovement(int multiplier)
    {
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.AddForce(new Vector2(_jumpForce * multiplier, _jumpForce));
        _jumpCooldown = true;
        yield return new WaitForSeconds(1.5f);
        _jumpCooldown = false;
    }
    private void MeleeAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_center.transform.position, _attackRange, LayerMask.GetMask("Default"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (colliders[i].gameObject.GetComponent<PlayerController>())
                {
                    colliders[i].gameObject.GetComponent<PlayerController>().Hurt(_meleeDamage);
                }

            }

        }
    }




    private void Die()
    {
        if(enemyType == 2)
        {
            GameObject skull = Instantiate(_skeletonSkullPrefab, _center.position, _center.rotation);
        }
        GameObject poof = Instantiate(_deathAnim, _center.position, _center.rotation);
        Destroy(gameObject);
        Destroy(poof, 0.5f);
    }
}
