using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationsScripts : MonoBehaviour
{
    public Animator fadeAnimator;
    public Animator levelTextAnimator;

    public TextMeshProUGUI LevelText;

    public void StartFadeOut()
    {
        fadeAnimator.SetTrigger("LoadLevel");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void AnimateLevelText()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            LevelText.text = "Level " + SceneManager.GetActiveScene().buildIndex;
            levelTextAnimator.SetTrigger("Start");
        }
    }

    public void GameOverSound()
    {
        AudioSource s;
        s = GetComponents<AudioSource>()[0];
        s.Play();
    }

    public void GameStartSound()
    {
        AudioSource s;
        s = GetComponents<AudioSource>()[0];
        s.Play();
    }
}
