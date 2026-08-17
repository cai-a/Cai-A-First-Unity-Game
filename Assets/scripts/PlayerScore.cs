using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerScore : MonoBehaviour
{
    public int score = 0;
    public int winScore = 5;
    public TMP_Text scoreText;
    public TMP_Text winText;

    public void AddPoint()
{
    score += 1;

    scoreText.text = "Score: " + score;

    Debug.Log("Score: " + score);

    if (score==winScore) 
    {
        winText.gameObject.SetActive(true);
    }
}

void Update()
    {
        if (score >= winScore && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}