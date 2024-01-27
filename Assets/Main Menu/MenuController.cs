using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{
    public AudioSource _audioSource;
    public CanvasGroup _blackout;

    public void StartGame()
    {
        StartCoroutine(CStartGame());
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator CStartGame()
    {
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime;

            _blackout.alpha = timer;
            _audioSource.volume =  1f - timer;

            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("Main");
    }
}
