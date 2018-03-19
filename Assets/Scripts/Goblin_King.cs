using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goblin_King : MonoBehaviour {

	Collider2D[] colliders;
	Color _color;
	Renderer renderer;

	public int _meleeDamage;
	public float _health;
	public GameObject _deathAnim,ball,chain1,chain2,chain3,hand,shield,dead/*,healthBarGO*/;
	public Image healthBarImg;
	public Sprite[] healthbar;
	public bool deathStarted;
	public Animator animator;
    public GameObject _bigChest;

	// Use this for initialization
	void Start () {
		_color = gameObject.GetComponent<Renderer>().material.color;
		Physics2D.IgnoreLayerCollision (10, 0, true);
		deathStarted = false;
		renderer = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (healthBarGO.GetComponent <SpriteRenderer>().sprite != healthbar [((int)_health)]){
			healthBarGO.GetComponent <SpriteRenderer>().sprite = healthbar [((int)_health)];
		}
		*/
		if (healthBarImg.sprite != healthbar [((int)_health)]){
			healthBarImg.sprite = healthbar [((int)_health)];
		}

		if (!deathStarted) {
			chain2.transform.position = (hand.transform.position + ball.transform.position) / 2;
			chain1.transform.position = (hand.transform.position + chain2.transform.position) / 2;
			chain3.transform.position = (chain2.transform.position + ball.transform.position) / 2;

			colliders = Physics2D.OverlapCircleAll (transform.position, 1f, LayerMask.GetMask ("Default"));
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject != gameObject) {
					if (colliders [i].gameObject.GetComponent<PlayerController> ()) {
						//KOMMENTOITU ULOS, RIKKOO KOODIN. 3.5.2017/TONI
						colliders [i].gameObject.GetComponent<PlayerController> ().Hurt (_meleeDamage);
					}
				}
			}
		}

		if (_health <= 0 && !deathStarted)
		{

			Vector3 kingPos = transform.position;

			gameObject.GetComponent<Animator> ().enabled = false;

			transform.position = kingPos;

			dead.SetActive (true);

			gameObject.GetComponent<Renderer>().enabled=false;
			gameObject.GetComponent<BoxCollider2D> ().enabled = false;

			Destroy (hand);
			Destroy (shield);

			StartCoroutine(Poof (ball.transform.position));
			Destroy (ball);

			StartCoroutine(Poof (chain1.transform.position));
			Destroy (chain1);

			StartCoroutine(Poof (chain2.transform.position));
			Destroy (chain2);

			StartCoroutine(Poof (chain3.transform.position));
			Destroy (chain3);

			StartCoroutine(Die());
			//StartCoroutine(Ajastin());
			deathStarted = true;
		}
	}

	IEnumerator Die()
	{
		
		for (int i = 0; i < 30; i++) {
			StartCoroutine (Poof (new Vector3(Random.Range(renderer.bounds.min.x,renderer.bounds.max.x),Random.Range(renderer.bounds.min.y,renderer.bounds.max.y),transform.position.z)));
			yield return new WaitForSeconds (0.1f);
			
		}
		yield return new WaitForSeconds (0.5f);
        //Debug.Log ("nyt pitäis olla 3.5");
        GameObject endChest = Instantiate(_bigChest, transform.position, transform.rotation);
        endChest.gameObject.SetActive(true);
        endChest.GetComponent<BigChestScript>().PlayFallAnimation();
		Destroy(gameObject);
	}
	/*
	IEnumerator Ajastin()
	{
		Debug.Log ("0");
		yield return new WaitForSeconds (1f);
		Debug.Log ("1");
		yield return new WaitForSeconds (1f);
		Debug.Log ("2");
		yield return new WaitForSeconds (1f);
		Debug.Log ("3");
		yield return new WaitForSeconds (0.5f);
		Debug.Log ("3.5");
		yield return new WaitForSeconds (0.5f);
		Debug.Log ("4");
		yield return new WaitForSeconds (1f);
	}
	*/
	IEnumerator Poof(Vector3 pos){
		GameObject poof = Instantiate(_deathAnim, pos, Quaternion.identity);
		yield return new WaitForSeconds (0.5f);
		Destroy(poof);
	}

	/*
	IEnumerator HurtAnim()
	{
		Renderer rend = gameObject.GetComponent<Renderer>();
		for (int i = 0; i < 2; i++)
		{
			rend.material.color = new Color(1f, 1f, 1f, 0f);
			yield return new WaitForSeconds(0.1f);
			rend.material.color = _color;
			yield return new WaitForSeconds(0.1f);
		}
	}
	*/

	public void TakeDamage(float damage)
	{
		//StartCoroutine(HurtAnim());
		animator.SetTrigger("Hurt");
		shield.GetComponent<Animator> ().SetTrigger ("Hurt");
		//hand.GetComponent<Animator> ().SetTrigger ("Hurt");
		_health -= damage;
	}
}
