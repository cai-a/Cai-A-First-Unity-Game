using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerScore : MonoBehaviour
{
    public int score = 0;
    public int winScore = 5;
    public TMP_Text scoreText;
    public TMP_Text winText;
    public GameManager gameManager;

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
            gameManager.RestartGame();
        }
    }
}