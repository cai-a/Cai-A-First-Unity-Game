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
        if (gameManager.currentState != GameManager.GameState.Playing)
        {
            return;
        }
        score += 1;

        scoreText.text = "Score: " + score;

        Debug.Log("Score: " + score);

        if (score>=winScore) 
        {
            gameManager.WinGame();
            winText.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (gameManager.currentState == GameManager.GameState.Won && Keyboard.current.rKey.wasPressedThisFrame)
        {
            gameManager.RestartGame();
        }
    }
}