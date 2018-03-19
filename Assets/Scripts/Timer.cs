using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public int timeLeft = 5;
    public Text countdownText;
    public GameObject GameOver;
    private bool isShowing;
    internal static float deltaTime;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        countdownText.text = ("TIME: " + timeLeft);

        if (timeLeft <= 0 && !isShowing)
        {
            isShowing = !isShowing;
            GameOver.SetActive(isShowing);
            Time.timeScale = 0;
        }
    }

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }
}