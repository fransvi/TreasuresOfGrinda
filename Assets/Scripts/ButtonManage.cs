using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManage : MonoBehaviour
{

	public Button[] buttons;
	public Image cursor;
    public GameManager gm;


	private int highlightedButton=0;

    void Start()
    {
        Time.timeScale = 1.0f;
    }

	void Update() {
        // syöttö

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
			moveCursor (-1);
		} else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
			moveCursor (1);
		}

		// kursorin liike
		cursor.transform.position=new Vector3(buttons[highlightedButton].transform.position.x-70,buttons[highlightedButton].transform.position.y,cursor.transform.position.z);

		// valinta
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Space)) {
			buttons[highlightedButton].GetComponent<Button> ().onClick.Invoke ();
		}
	}

	void moveCursor(int amount){
		if (highlightedButton + amount < 0) {
			highlightedButton = buttons.GetLength (0) - 1;
		} else if (highlightedButton + amount > buttons.Length - 1) {
			highlightedButton = 0;
		} else {
			highlightedButton += amount;
		}
	}

    public void SetControlSet(int f)
    {
        gm.GetComponent<GameManager>().SetControls(f);
    }

    public void SetGameManager(GameManager g)
    {
        gm = g;
        buttons[5].onClick.AddListener(gm.ResetSaveData);
        buttons[6].onClick.AddListener(gm.SaveData);
    }

    public void RetryBtn()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void LoadLevelBtn(string sceneName)
	{
        StartCoroutine(ChangeLevel(sceneName));
	}

	public void LoadLevelBtn(int sceneID) // build index parametrina, voi olla hyödyllinen
	{
        Debug.Log("load scene " + sceneID);
        gm.LoadLevelInt(sceneID);
	}

    IEnumerator ChangeLevel(string sceneName)
    {
        float fadeTime = GetComponent<AutoFade>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

    public void PauseLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StartGameBtn()
    {
    }

    public void ExitGameBtn()
    {
        Application.Quit();
    }
}
