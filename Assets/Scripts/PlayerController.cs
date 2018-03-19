using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //Animaattori
    //11
    public Animator CharacterAnimator;

    //Audio
    public AudioSource jumpSound;
    public AudioSource swordSound;
    public AudioSource landingSound;
    public AudioSource playerHitSound;
    public AudioSource playerRunLoop;
    public int _bowDamage = 1;  
    public int _wandDamage = 1;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    bool jumpSoundPlayed = false;
    bool runLoopPlayed = false;
    public VirtualJoystick _joystick;
    private float _horizontalJoystick; private bool _mobileOffDown = false; private bool _mobileVersion;
    private float _verticalJoystick; private bool _bowFullyCharged;
    public GameObject[] _weaponsList;

    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _jumpCooldown;
    [SerializeField]
    private float _attackCooldown;
    [SerializeField]
    private bool _airControl;
    [SerializeField]
    private LayerMask _whatIsGround;
    [SerializeField]
    private LayerMask _whatIsPlatform;
    [SerializeField]
    private LayerMask _whatIsSpikes;
    [SerializeField]
    private LayerMask _whatIsLadder;
    [SerializeField]
    private LayerMask _enemyLayerMask;
    [SerializeField]
    private LayerMask _whatIsItem;
    [SerializeField]
    private LayerMask _whatIsDoor;
    [SerializeField]
    private LayerMask _whatIsChest;
    [SerializeField]
    private LayerMask _whatIsStairs;
    [SerializeField]
    private LayerMask _whatIsLever;
    [SerializeField]
    private LayerMask _whatIsBigChest;
    [SerializeField]
    private float _meleeRadius;
    [SerializeField]
    private float _meleeDamage;
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private float _knockbackForce;
    [SerializeField]
    private Transform _ladderCheck;
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _bomb;
    [SerializeField]
    private GameObject _potionEffectSprite;

    private bool _itemPickupCooldown;
    [SerializeField]
    private GameObject _itemPrefab;

    private int _health;
    private bool _jumpOnCooldown = false;
    private float _horizontalMove = 0;
    private bool _jump = false;
    private Transform _groundCheck;
    private Transform _meleeCheck; public GameObject _bowGlint;
    private Transform _headCheck;
    private int _gender;
    private GameObject _playerManager;
    private bool _attackOnCooldown = false;

    private GameManager _gameManager;

    private Transform _weaponSwing;
    private Transform _swordSwing;
    private Transform _maceSwing;

    private KeyCode _key1;
    private KeyCode _key2;
    private KeyCode _key3;

    [SerializeField]
    private float _slopeFriction;
    [SerializeField]
    private GameObject _shootPoint;
    private float _bombForce = 1f;
    private bool _drawingBow;
    private bool _dying;
    private bool _layingBomb;
    private bool _releasingBow;
    private float _bowForce;
    private bool _onLadder;
    private Rigidbody2D _rigidBody;
    private bool _invulnurable;
    const float _groundedRadius = .1f;
    private bool _grounded;
    private float tempSpeed; private bool _usingWand;
    private bool _facingRight; private bool _blocking;
    private bool _isTakingDamage; private bool _offCooldown;
    private bool _onPlatform; private bool _onStairs;
    private float _selectedControls;
    private bool _isJumping = false;
    private Vector2 _gravity;
    private Color _color;
    private bool _ableToShootBow;
    [SerializeField]
    private float _bowCoolDown;

    private void Awake()
    {
    }

    public void SetGender(int g)
    {
        _gender = g;
    }
    public int GetGender()
    {
        return _gender;
    }

    void Start()
    {
        _layingBomb = false; _itemPickupCooldown = false;
        _ableToShootBow = true; _onStairs = false; _bowFullyCharged = false;
        _meleeCheck = transform.Find("MeleeCheck");
        _groundCheck = transform.Find("GroundCheck");
        _ladderCheck = transform.Find("LadderCheck");
        _weaponSwing = transform.Find("Swing");
        _swordSwing = transform.Find("Swing");
        _maceSwing = transform.Find("MaceSwing");
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _health = _maxHealth;
        _dying = false; _usingWand = false; _offCooldown = false;
        _releasingBow = false;
        _drawingBow = false;
        _invulnurable = false;
        _onLadder = false;
        _weaponSwing.gameObject.SetActive(false);
        _maceSwing.gameObject.SetActive(false);
        _swordSwing.gameObject.SetActive(false);
        _gravity = Physics2D.gravity;
        tempSpeed = _maxSpeed;
        _playerManager = GameObject.Find("PlayerManager");
        Physics2D.IgnoreLayerCollision(0, 16, true);
        Physics2D.IgnoreLayerCollision(0, 25, true);
        Physics2D.IgnoreLayerCollision(16, 25, true);
        Physics2D.IgnoreLayerCollision(25, 25, true);
        Physics2D.IgnoreLayerCollision(18, 25, true);

        //Animaattori
        _color = gameObject.GetComponent<Renderer>().material.color;
        CharacterAnimator = GetComponent<Animator>();
        if (_selectedControls == 0)
        {
            _key1 = KeyCode.Q;
            _key2 = KeyCode.W;
            _key3 = KeyCode.E;
        }
        else if (_selectedControls == 1)
        {
            _key1 = KeyCode.Z;
            _key2 = KeyCode.X;
            _key3 = KeyCode.C;
        }
        //Audio
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.Play(44100);
    }
    //Commands for mobile controls

    public void MobileJumpDown()
    {
        _jump = true;
    }
    public void MobileJumpUp()
    {
        _jump = false;
    }
    public void MobileMainDown()
    {
        if (!_attackOnCooldown)
        {
            Attack();
            swordSound.Play();
        }
    }
    public void MobileMainUp()
    {

    }
    public void MobileOffDown()
    {

        _mobileOffDown = true;
        

    }
    public void MobileOffUp()
    {
   
        int ow = _playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon();
        if (ow == 1)
        {
            _drawingBow = false;
            _bowFullyCharged = false;
            UseOffWeapon();
            _bowForce = 0;
            StartCoroutine(BowRelease());




        }
        else if (ow == 2)
        {
            if (!_offCooldown)
            {
                UseOffWeapon();
                StartCoroutine(BombLayAnim());
            }

        }
        else if (ow == 3)
        {
            Debug.Log("Blocking false");
            _blocking = false;
        }
        else if (ow == 4)
        {
            StartCoroutine(WandAttack());
            _bowForce = 0;
        }
        _mobileOffDown = false;
    }
    public void MobileConsumableDown()
    {
        UsePotion();
    }
    public void OffHold()
    {
        int ow = _playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon();
        if (ow == 1 && !_offCooldown)
        {
            _drawingBow = true;
            if (_bowForce < 70)
            {
                _bowForce += 1;
            }
            else if (_bowForce > 50)
            {
                if (!_bowFullyCharged)
                {
                    StartCoroutine(BowGlintAnim());
                    _bowFullyCharged = true;
                }

            }


        }
        else if (ow == 2)
        {

        }
        else if (ow == 3)
        {
            _blocking = true;
        }
        else if (ow == 4)
        {
            _bowForce = 10;
        }
    }

    public void OffRelease()
    {
        int ow = _playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon();
        if (ow == 1)
        {
            _drawingBow = false;
            UseOffWeapon();
            _bowForce = 0;
            StartCoroutine(BowRelease());




        }
        else if (ow == 2)
        {
            if (!_offCooldown)
            {
                UseOffWeapon();
                StartCoroutine(BombLayAnim());
            }
 
        }
        else if (ow == 3)
        {
            Debug.Log("Blocking false");
            _blocking = false;
        }
        else if (ow == 4)
        {
            StartCoroutine(WandAttack());
            _bowForce = 0;
        }
        _mobileOffDown = false;
    }
    public void MobileConsumableUp()
    {

    }
    IEnumerator BowGlintAnim()
    {
        Vector3 newPos = new Vector3(_shootPoint.transform.position.x+0.1f, _shootPoint.transform.position.y-0.1f, _shootPoint.transform.position.z - 5);
        GameObject glint = Instantiate(_bowGlint, newPos, _shootPoint.transform.rotation);
        Destroy(glint, 0.25f);
        yield return new WaitForSeconds(0.25f);
    }

    public void SetMobileVersion(bool boo)
    {
        _mobileVersion = boo;
    }
    void Update()
    {

        if (_playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon() == 0)
        {
            _weaponSwing = _swordSwing;
        }
        else if (_playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon() == 1)
        {
            _weaponSwing = _maceSwing;
        }
        if (_mobileOffDown)
        {
            OffHold();
        }

        //Gold automated pickup

        Collider2D[] items = Physics2D.OverlapCircleAll(_groundCheck.position, 0.5f, _whatIsItem);

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].gameObject != gameObject)
            {
                float distance = Vector2.Distance(items[i].transform.position, transform.position);
                if (items[i].gameObject.GetComponent<ItemScript>().GetIsCoin())
                {
                    StartCoroutine(AutomatedCoinPickup());

                }else if (!items[i].gameObject.GetComponent<ItemScript>().GetIsCoin())
                {
                    items[i].GetComponent<ItemScript>().SetPickupArrowActive(true);
                }


            }
        }



        // Go down platforms
        if (_selectedControls == 0)
        {

        }
        else if (_selectedControls == 1)
        {

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
            //JumpDown();
            //Jump-animaatio

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _jump = false;
            //JumpUp();
        }
        if (Input.GetKeyDown(_key1))
        {
            if (!_attackOnCooldown)
            {
                Attack();
                swordSound.Play();
            }

        }

        if (Input.GetKeyDown(_key3))
        {
            UsePotion();
            //TODO
        }
        if (_mobileVersion)
        {
            if (_verticalJoystick > 0.5f)
            {
                if (!_itemPickupCooldown)
                {
                    StartCoroutine(ItemPickupCooldown());
                    PickUpItem();
                    OpenObject();
                }

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                StartCoroutine(ItemPickupCooldown());
                PickUpItem();
                OpenObject();
            }
        }

        if (Input.GetKey(_key2))
        {
            int ow = _playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon();
            if (ow == 1 && !_offCooldown)
            {
                _drawingBow = true;
                if (_bowForce < 70)
                {
                    _bowForce += 1;
                }
                else if (_bowForce > 50)
                {
                    if (!_bowFullyCharged)
                    {
                        StartCoroutine(BowGlintAnim());
                        _bowFullyCharged = true;
                    }

                }

            }
            else if (ow == 2)
            {

            }
            else if (ow == 3)
            {
                _blocking = true;
            }
            else if (ow == 4)
            {
                _bowForce = 10;
            }

        }
        if (Input.GetKeyUp(_key2)) // !_offWCooldown
        {
            int ow = _playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon();
            if (ow == 1)
            {
                _drawingBow = false;
                _bowFullyCharged = false;
                UseOffWeapon();
                _bowForce = 0;
                StartCoroutine(BowRelease());




            }
            else if (ow == 2)
            {
                if (!_offCooldown)
                {
                    UseOffWeapon();
                    StartCoroutine(BombLayAnim());
                }

            }
            else if (ow == 3)
            {
                Debug.Log("Blocking false");
                _blocking = false;
            }
            else if (ow == 4)
            {
                StartCoroutine(WandAttack());
                _bowForce = 0;
            }

        }
    }
    private void OffWeaponAttackDown()
    {

    }
    private void OffWeaponAttackUp()
    {

    }
    IEnumerator AutomatedCoinPickup()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("LSPPLSDF");
        PickUpItem();

    }
    IEnumerator OffWCooldown()
    {
        _offCooldown = true;
        yield return new WaitForSeconds(1f);
        _offCooldown = false;
    }
    IEnumerator BombLayAnim()
    {
        _layingBomb = true;
        yield return new WaitForSeconds(0.45f);
        _layingBomb = false;
    }
    IEnumerator BowRelease()
    {
        _releasingBow = true;
        yield return new WaitForSeconds(0.25f);
        _releasingBow = false;
    }
    IEnumerator WandAttack()
    {
        _usingWand = true;
        yield return new WaitForSeconds(0.15f);
        UseOffWeapon();
        yield return new WaitForSeconds(0.30f);
        _usingWand = false;
    }

    private void OpenObject()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, 0.50f, _whatIsDoor);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (colliders[i].gameObject.GetComponent<DoorScript>().GetRequiresKey())
                {
                    if (_playerManager.GetComponent<PlayerInventory>().getHasKey1() && colliders[i].gameObject.GetComponent<DoorScript>().GetDoorState() == 1)
                    {
                        _gameManager.SaveData();
                        _playerManager.GetComponent<PlayerInventory>().setHasKey1(false);
                        _gameManager.LoadLevel1_2(false);
                    }
                    else if (_playerManager.GetComponent<PlayerInventory>().getHasKey1())
                    {
                        colliders[i].gameObject.GetComponent<DoorScript>().SetDoorState(1);

                    }
                }
                else
                {
                    if (colliders[i].gameObject.GetComponent<DoorScript>().GetDoorState() == 0)
                    {
                        colliders[i].gameObject.GetComponent<DoorScript>().SetDoorState(1);
                    }
                    else if (colliders[i].gameObject.GetComponent<DoorScript>().GetDoorState() == 1)
                    {
                        _gameManager.SaveData();
                        _playerManager.GetComponent<PlayerInventory>().setHasKey1(false);
                        _gameManager.LoadLevel1_2(false);
                    }

                }


            }
        }

        Collider2D[] chests = Physics2D.OverlapCircleAll(_groundCheck.position, 0.50f, _whatIsChest);
        for (int i = 0; i < chests.Length; i++)
        {
            if (chests[i].gameObject != gameObject)
            {
                chests[i].gameObject.GetComponent<ChestScript>().OpenChest();
            }
        }

        Collider2D[] chestss = Physics2D.OverlapCircleAll(_groundCheck.position, 1f, _whatIsBigChest);
        for (int i = 0; i < chestss.Length; i++)
        {
            if (chestss[i].gameObject != gameObject)
            {

                chestss[i].gameObject.GetComponent<BigChestScript>().OpenChest();
            }
        }

        Collider2D[] levers = Physics2D.OverlapCircleAll(_groundCheck.position, 0.50f, _whatIsLever);
        for (int i = 0; i < levers.Length; i++)
        {
            if (levers[i].gameObject != gameObject)
            {
                if (!levers[i].gameObject.GetComponent<LeverScript>().GetSpikesRaising() && !levers[i].gameObject.GetComponent<LeverScript>().GetSpikesLowering())
                {
                    if (levers[i].gameObject.GetComponent<LeverScript>().GetLeverState() == 0)
                    {
                        levers[i].gameObject.GetComponent<LeverScript>().SetLeverState(1);
                    }
                    else if (levers[i].gameObject.GetComponent<LeverScript>().GetLeverState() == 1)
                    {
                        levers[i].gameObject.GetComponent<LeverScript>().SetLeverState(0);
                    }
                }
            }
        }
    }

    IEnumerator BowCooldown()
    {
        _ableToShootBow = false;
        yield return new WaitForSeconds(0.45f);
        _ableToShootBow = true;
    }


    private void UseOffWeapon()
    {
        //Bow
        if (_playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon() == 1 && _ableToShootBow)
        {
            GameObject go = Instantiate(_bullet, _meleeCheck.position, _meleeCheck.rotation);
            if (_facingRight)
            {
                go.GetComponent<BulletScript>().createBullet(true, _bowForce, 0, false, _bowDamage);
            }
            else
            {
                go.GetComponent<BulletScript>().createBullet(false, _bowForce, 0, false, _bowDamage);
            }
            StartCoroutine(BowCooldown());
            Destroy(go, 3.0f);
        }
        //Bomb
        if (_playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon() == 2 && _ableToShootBow)
        {
            GameObject go = Instantiate(_bomb, _shootPoint.transform.position, _shootPoint.transform.rotation);
            if (_facingRight)
            {
                go.GetComponent<BombScript>().CreateBomb(true, 5);
            }
            else
            {
                go.GetComponent<BombScript>().CreateBomb(false, 5);
            }
            StartCoroutine(OffWCooldown());

        }

        if (_playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon() == 3)
        {
            //Done as boolean value
        }
        //Wand
        if (_playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon() == 4 && _ableToShootBow)
        {
            GameObject go = Instantiate(_bullet, _shootPoint.transform.position, _shootPoint.transform.rotation);
            if (_facingRight)
            {
                go.GetComponent<BulletScript>().createBullet(true, 10, 1, false, _wandDamage);
            }
            else
            {
                go.GetComponent<BulletScript>().createBullet(false, 10, 1, false, _wandDamage);
            }

            Destroy(go, 6.0f);

        }

    }

    private void PickUpItem()
    {

        /*
        Item types:

        0: Consumable
            0: Health potion
            1: Large Coin
            2: Small Coin
            3: Key1
        1: Main Weapon
            0: Sword
            1: Mace
            2: SuperSword
        2: Off Weapon
            0: None
            1: Bow
            2: Bomb
            3: Shield
            4: Magic Wand

      */

        int currentOffWep = _playerManager.GetComponent<PlayerInventory>().GetCurrentOffWeapon();
        int currentMainWep = _playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, 0.5f, _whatIsItem);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {

                int itemInt = colliders[i].gameObject.GetComponent<ItemScript>().GetItemInt();
                int itemType = colliders[i].gameObject.GetComponent<ItemScript>().GetItemType();

                //Consumable loot
                if (itemType == 0)
                {
                    if (itemInt == 0)
                    {
                        _playerManager.GetComponent<PlayerInventory>().gainHealthPotion();
                    }
                    else if (itemInt == 1)
                    {
                        _playerManager.GetComponent<PlayerInventory>().gainGold(5f);
                    }
                    else if (itemInt == 2)
                    {
                        _playerManager.GetComponent<PlayerInventory>().gainGold(1f);

                    }
                    else if (itemInt == 3)
                    {
                        _playerManager.GetComponent<PlayerInventory>().gainKey1();
                    }

                }
                else if (itemType == 1)
                {
                    DropLastItem(itemType, currentMainWep);
                    _playerManager.GetComponent<PlayerInventory>().SetCurrentMainWeapon(itemInt);
                }
                else if (itemType == 2)
                {
                    if (currentOffWep > 0)
                    {
                        DropLastItem(itemType, currentOffWep);
                        _playerManager.GetComponent<PlayerInventory>().SetCurrentOffWeapon(itemInt);
                    }
                    else
                    {
                        _playerManager.GetComponent<PlayerInventory>().SetCurrentOffWeapon(itemInt);
                    }

                }
                /*
				for (int j=0;j<colliders[i].transform.childCount; j++){
					if(colliders[i].transform.GetChild(j).gameObject.activeSelf==true){
						colliders [i].transform.GetChild (j).GetComponent<AudioSource> ().PlayOneShot (colliders [i].transform.GetChild (j).GetComponent<AudioSource> ().clip);
					}
				}
                */
                colliders[i].gameObject.GetComponent<ItemScript>().Die();
            }
        }
    }

    private void DropLastItem(int itemType, int itemInt)
    {
        GameObject go = Instantiate(_itemPrefab, transform.position, transform.rotation);
        go.GetComponent<ItemScript>().SetItemType(itemType);
        go.GetComponent<ItemScript>().SetItemInt(itemInt);

    }

    public void SetJoystick(GameObject js)
    {
        _joystick = js.GetComponent<VirtualJoystick>();
    }

    void FixedUpdate()
    {

        //Ignore platform when jumping on it
        if(_joystick != null)
        {
            _horizontalJoystick = _joystick.Horizontal();
            _verticalJoystick = _joystick.Vertical();
            _horizontalMove = _horizontalJoystick;
        }
        else
        {
            _verticalJoystick = Input.GetAxis("Vertical");
            _horizontalJoystick = Input.GetAxis("Horizontal");
            _horizontalMove = _horizontalJoystick;
            
        }


        if (transform.GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            Physics2D.IgnoreLayerCollision(0, 9, true);
        }

        else if (_verticalJoystick < -0.90f)
        {
            Physics2D.IgnoreLayerCollision(0, 9, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(0, 9, false);
        }
        //Counter velocity on ramps
        /*
        float angle;
        RaycastHit2D[] hits = new RaycastHit2D[2];
        //cast ray downwards
        int h = Physics2D.RaycastNonAlloc(transform.position, -Vector2.up, hits);
        if (h > 1)
        {
            angle = Mathf.Abs(Mathf.Atan2(hits[1].normal.x, hits[1].normal.y) * Mathf.Rad2Deg); //get angle
            if (angle > 30)
            {

                _maxSpeed = tempSpeed / 2;
            }
            else
            {
                _maxSpeed = tempSpeed;
            }
        }

        */
        //old gravity code
        //_rigidBody.AddForce(_gravity * _rigidBody.mass);
        _grounded = false;
        _onStairs = false;
        //Checks for ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                //_gravity = Physics2D.gravity;
                _grounded = true;
            }

        }
        Collider2D[] stairs = Physics2D.OverlapCircleAll(_groundCheck.position, 0.2f, _whatIsStairs);
        for (int i = 0; i < stairs.Length; i++)
        {
            if (stairs[i].gameObject != gameObject)
            {
                _onStairs = true;
                _grounded = true;

            }

        }
        Collider2D[] pressurePads = Physics2D.OverlapCircleAll(_groundCheck.position, 0.2f, 1 << LayerMask.NameToLayer("PressurePad"));
        for (int i = 0; i < pressurePads.Length; i++)
        {
            if (pressurePads[i].gameObject != gameObject)
            {
                _grounded = true;
                pressurePads[i].gameObject.GetComponent<PressurePadScript>().SetPressurePadState(1);

            }

        }
        Collider2D[] stageLimits = Physics2D.OverlapCircleAll(_groundCheck.position, 0.2f, 1 << LayerMask.NameToLayer("StageLimit"));
        for (int i = 0; i < stageLimits.Length; i++)
        {
            if (stageLimits[i].gameObject != gameObject)
            {
                _health = 0;
                Die();
            }

        }

        Collider2D[] ladders = Physics2D.OverlapCircleAll(_ladderCheck.position, _groundedRadius, _whatIsLadder);
        for (int i = 0; i < ladders.Length; i++)
        {
            if (ladders[i].gameObject != gameObject)
            {
                _onLadder = true;
            }


        }
        if (ladders.Length < 1)
        {
            _onLadder = false;
        }

        //Freeze movement on stairs when velocity == 0
        if (_grounded && _horizontalMove == 0 && !_jump && !_isJumping && _onStairs)
        {
            _rigidBody.isKinematic = true;
            _rigidBody.velocity = Vector2.zero;
        }
        else
        {
            _rigidBody.isKinematic = false;
        }

        if (_onLadder)
        {

            _rigidBody.gravityScale = 0f;
            _rigidBody.velocity = new Vector2(0, 0);
            if (_verticalJoystick > 0)
            {
                transform.Translate(0, 3 * Time.deltaTime, 0);
            }
            else if (_verticalJoystick < 0)
            {
                transform.Translate(0, -3 * Time.deltaTime, 0);
            }
        }
        else
        {
            _rigidBody.gravityScale = 1.5f;

        }

        Move(_horizontalMove, _jump);



    }

    public void Move(float move, bool jump)
    {

        if (_dying && _gender == 0)
        {
            CharacterAnimator.Play("Die");
        }
        else if (_dying && _gender == 1)
        {
            CharacterAnimator.Play("FDie");
        }


        else if (_blocking && _gender == 0)
        {
            _rigidBody.velocity = new Vector2(0, _rigidBody.velocity.y);
            CharacterAnimator.Play("ShieldBlock");
        }
        else if (_blocking && _gender == 1)
        {
            _rigidBody.velocity = new Vector2(0, _rigidBody.velocity.y);
            CharacterAnimator.Play("FShieldBlock");
        }


        else if (_grounded || _airControl)
        {
            if (_attackOnCooldown)
            {
                if (_playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon() == 0 && _gender == 0)
                {
                    CharacterAnimator.Play("AttackSword");
                }
                else if (_playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon() == 0 && _gender == 1)
                {
                    CharacterAnimator.Play("FAttackSword");
                }
                else if (_playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon() == 1 && _gender == 0)
                {
                    CharacterAnimator.Play("AttackMace");
                }
                else if (_playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon() == 1 && _gender == 1)
                {
                    CharacterAnimator.Play("FAttackMace");
                }
                else if (_playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon() == 2 && _gender == 0)
                {
                    CharacterAnimator.Play("AttackSuperSword");
                }
                else if (_playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon() == 2 && _gender == 1)
                {
                    CharacterAnimator.Play("FAttackSuperSword");
                }

            }
            else if (_usingWand && _gender == 0)
            {
                CharacterAnimator.Play("AttackWand");
            }
            else if (_usingWand && _gender == 1)
            {
                CharacterAnimator.Play("FAttackMagicWand");
            }


            else if (_drawingBow && _ableToShootBow && _gender == 0)
            {
                CharacterAnimator.Play("BowDraw");
            }
            else if (_drawingBow && _ableToShootBow && _gender == 1)
            {
                CharacterAnimator.Play("FDrawBow");
            }
            else if (_releasingBow && _gender == 0)
            {
                CharacterAnimator.Play("BowRelease");
            }
            else if (_releasingBow && _gender == 1)
            {
                CharacterAnimator.Play("FReleaseBow");
            }
            else if (_layingBomb && _gender == 0)
            {
                CharacterAnimator.Play("AttackDefault");
            }
            else if (_layingBomb && _gender == 1)
            {
                CharacterAnimator.Play("FAttackDefault");
            }
            else if (_invulnurable && _gender == 0)
            {
                CharacterAnimator.Play("Damage");
            }
            else if (_invulnurable && _gender == 1)
            {
                CharacterAnimator.Play("FHurt");
            }
            else if (_onLadder && _gender == 0)
            {
                CharacterAnimator.Play("Climb");
            }
            else if (_onLadder && _gender == 1)
            {
                CharacterAnimator.Play("FClimb");
            }
            else if (_grounded && move != 0 && _gender == 0)
            {
                //CharacterAnimator.SetBool("Walk", true);
                //CharacterAnimator.Play("Walk");
                CharacterAnimator.Play("Walk");
                if (!runLoopPlayed)
                {
                    runLoopPlayed = true;
                    playerRunLoop.loop = true;
                    playerRunLoop.Play();
                }
            }
            else if (_grounded && move != 0 && _gender == 1)
            {
                CharacterAnimator.Play("FRun");
                if (!runLoopPlayed)
                {
                    runLoopPlayed = true;
                    playerRunLoop.loop = true;
                    playerRunLoop.Play();
                }
            }
            else if (_isJumping && _gender == 0)
            {
                if (!jumpSoundPlayed)
                {
                    jumpSound.Play();
                    jumpSoundPlayed = true;
                }
                CharacterAnimator.Play("Jump");
                if (runLoopPlayed)
                {
                    playerRunLoop.loop = false;
                    runLoopPlayed = false;
                }
            }
            else if (_isJumping && _gender == 1)
            {
                if (!jumpSoundPlayed)
                {
                    jumpSound.Play();
                    jumpSoundPlayed = true;
                }
                CharacterAnimator.Play("FJump");
                if (runLoopPlayed)
                {
                    playerRunLoop.loop = false;
                    runLoopPlayed = false;
                }
            }
            else if (!_grounded && !_isJumping && _gender == 0)
            {
                CharacterAnimator.Play("Fall");
                if (jumpSoundPlayed)
                {
                    jumpSoundPlayed = false;
                }
                if (runLoopPlayed)
                {
                    playerRunLoop.loop = false;
                    runLoopPlayed = false;
                }
            }
            else if (!_grounded && !_isJumping && _gender == 1)
            {
                CharacterAnimator.Play("FFall");
                if (jumpSoundPlayed)
                {
                    jumpSoundPlayed = false;
                }
                if (runLoopPlayed)
                {
                    playerRunLoop.loop = false;
                    runLoopPlayed = false;
                }
            }

            else
            {
                if (_gender == 0)
                {
                    CharacterAnimator.Play("Idle");
                }
                else
                {
                    CharacterAnimator.Play("FIdle");
                }

                if (runLoopPlayed)
                {
                    playerRunLoop.loop = false;
                    runLoopPlayed = false;
                }
                //CharacterAnimator.SetBool("Walk", false);
            }

            //Horizontal force
            _rigidBody.velocity = new Vector2(move * _maxSpeed, _rigidBody.velocity.y);

            //Direction flip
            if (move > 0 && !_facingRight)
            {
                Flip();
            }
            else if (move < 0 && _facingRight)
            {
                Flip();
            }

            if (_grounded && _jump && !_jumpOnCooldown && !_invulnurable)
            {
                _grounded = false;
                _rigidBody.velocity = Vector2.zero;
                _rigidBody.AddForce(new Vector2(0f, _jumpForce));
                StartCoroutine(JumpCooldown());

            }

        }

    }

    public void UsePotion()
    {
        if (_playerManager.GetComponent<PlayerInventory>().getHasPotion())
        {
            if (_health == 20)
            {
                //Do nothing
            }
            else if (_health + 10 > 20)
            {
                StartCoroutine(PotionEffect(2f));
                _health = 20;
                _playerManager.GetComponent<PlayerInventory>().useHealthPotion();
            }
            else
            {
                StartCoroutine(PotionEffect(2f));
                _health += 10;
                _playerManager.GetComponent<PlayerInventory>().useHealthPotion();
            }
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

    //Player takes damage;
    public void Hurt(int damage)
    {
        if (_health - damage > 0 && !_invulnurable && !_blocking)
        {
            _health -= damage;

            _rigidBody.velocity = Vector2.zero;
            _rigidBody.AddForce(new Vector2(-_knockbackForce, _knockbackForce));
            StartCoroutine(Flicker(5));
            StartCoroutine(InvulnTimer());
            playerHitSound.Play(); // AUDIO

        }
        else if (_health - damage <= 0)
        {
            _health = 0;
            Die();
        }
    }

    IEnumerator DeathAnimation()
    {
        //TODO play death animation and load Gameover/retry screen
        _dying = true;
        yield return new WaitForSeconds(1.5f);
        _gameManager.GetComponent<GameManager>().LoadGameOverScreen();
    }

    private void Die()
    {
        StartCoroutine(DeathAnimation());
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }

    IEnumerator PotionEffect(float ft)
    {
        GameObject explosion = Instantiate(_potionEffectSprite, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, 1f);
        yield return new WaitForSeconds(ft);

    }
    IEnumerator ItemPickupCooldown()
    {
        _itemPickupCooldown = true;
        yield return new WaitForSeconds(1f);
        _itemPickupCooldown = false;
    }

    IEnumerator InvulnTimer()
    {
        _invulnurable = true;
        yield return new WaitForSeconds(1f);
        _invulnurable = false;
    }

    IEnumerator Flicker(int times)
    {

        //TODO play hurt animation
        Renderer rend = gameObject.GetComponent<Renderer>();
        for (int i = 0; i < times; i++)
        {
            rend.material.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.1f);
            rend.material.color = _color;
            yield return new WaitForSeconds(0.1f);
        }


    }

    public void SetSelectedControls(int i)
    {
        _selectedControls = i;
    }

    //Placeholder for attack anims
    IEnumerator AttackAnim(float cooldown)
    {
        //_weaponSwing.gameObject.SetActive(true);
        _attackOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        //_weaponSwing.gameObject.SetActive(false);
        _attackOnCooldown = false;
    }

    IEnumerator JumpCooldown()
    {
        _jumpOnCooldown = true;
        _isJumping = true;
        yield return new WaitForSeconds(_jumpCooldown);
        _isJumping = false;
        _jumpOnCooldown = false;

    }

    public void Attack()
    {

        //Check if enemies hit

        GameObject currWeapon = _weaponsList[_playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon()];
        float weaponDamage = currWeapon.GetComponent<WeaponStats>()._weaponDamage;
        float weaponRadius = currWeapon.GetComponent<WeaponStats>()._weaponHitRadius;
        bool weaponHasStun = currWeapon.GetComponent<WeaponStats>()._hasStunEffect;
        float weaponSpeed = currWeapon.GetComponent<WeaponStats>()._weaponSpeed;
        float weaponCooldown = currWeapon.GetComponent<WeaponStats>()._weaponCooldown;
        StartCoroutine(AttackAnim(weaponCooldown));
        //Mace can break walls
        if(_playerManager.GetComponent<PlayerInventory>().GetCurrentMainWeapon() == 1)
        {
            Collider2D[] walls = Physics2D.OverlapCircleAll(_meleeCheck.position, weaponRadius, LayerMask.GetMask("Breakable"));
            for (int i = 0; i < walls.Length; i++)
            {
                if (walls[i].gameObject != gameObject)
                {
                    walls[i].gameObject.GetComponent<BreakableWall>().BreakWall();
                }
            }
        }


        Collider2D[] colliders = Physics2D.OverlapCircleAll(_meleeCheck.position, weaponRadius, _enemyLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                //Enemy hit
                if (colliders[i].gameObject.GetComponent<EnemyScript>())
                {
                    colliders[i].gameObject.GetComponent<EnemyScript>().TakeDamage(weaponDamage);
                    if (weaponHasStun)
                    {
                        float rand = UnityEngine.Random.Range(0, 100);
                        if (rand > 75)
                        {
                            if (colliders[i].gameObject.GetComponent<EnemyScript>())
                            {
                                colliders[i].gameObject.GetComponent<EnemyScript>().MaceStun();
                            }
                        }
                    }
                }
                if (colliders[i].gameObject.GetComponent<Goblin_King>())
                {
                    colliders[i].gameObject.GetComponent<Goblin_King>().TakeDamage(weaponDamage);
                }
            }
        }
    }


    public int GetHealth()
    {
        return _health;
    }
    public void SetHealth(int h)
    {
        _health = h;
    }

    //Funktiot touch screen liikkumiselle
    public void AddSpeed()
    {
        if (_horizontalMove <= 1f)
        {
            if (_horizontalMove + 0.15f > 1f)
            {
                _horizontalMove = 1f;
            }
            else
            {
                _horizontalMove += 0.15f;
            }

        }
    }
    public void DecreaseSpeed()
    {
        if (_horizontalMove >= -1f)
        {
            if (_horizontalMove - 0.15f < -1f)
            {
                _horizontalMove = -1f;
            }
            else
            {
                _horizontalMove -= 0.15f;
            }

        }
    }
    public void StopMove()
    {
        if (_horizontalMove > 0f)
        {
            {
                if (_horizontalMove - 0.10f < 0f)
                {
                    _horizontalMove = 0f;
                }
                else if (_horizontalMove > 0f)
                {
                    _horizontalMove -= 0.10f;
                }
            }

        }
        else
        {
            if (_horizontalMove + 0.10f > 0f)
            {
                _horizontalMove = 0f;
            }
            else if (_horizontalMove < 0f)
            {
                _horizontalMove += 0.10f;
            }
        }
    }
    public void LeftMoveDown()
    {
        CancelInvoke();
        InvokeRepeating("DecreaseSpeed", 0, 0.05f);
    }
    public void RightMoveDown()
    {
        CancelInvoke();
        InvokeRepeating("AddSpeed", 0, 0.05f);
    }
    public void MoveUp()
    {
        CancelInvoke();
        InvokeRepeating("StopMove", 0, 0.05f);
    }
    public void JumpUp()
    {
        _gravity = Physics2D.gravity * 1.5f;
        _jump = false;
    }
    public void JumpDown()
    {
        _gravity = _gravity / 1.5f;
        _jump = true;
    }

}