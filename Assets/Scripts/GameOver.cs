using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public GameObject canvas;

    // Use this for initialization
	void Start () {
	}

	public void InitiateGameOver(){
		// health bar -skripti kutsuu tätä funktiota
		StartCoroutine (DeathAnimation ());
	}

	IEnumerator DeathAnimation()
	{
		// Tänne kuolemisanimaatio
		yield return new WaitForSeconds(5);
		canvas.gameObject.SetActive(true);
	}
	
}