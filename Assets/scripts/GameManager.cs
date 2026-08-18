using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Won,
        Dead
    }
    public GameState currentState = GameState.Playing;
    
    void Update()
    {
        if (currentState != GameState.Playing && Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartGame();
        }
    }

    public void WinGame()
    {
        if (currentState != GameState.Playing)
        {
            return;
        }

        currentState = GameState.Won;

        Debug.Log("Game State: Won");
    }

    public void LoseGame()
    {
        if (currentState != GameState.Playing)
        {
            return;
        }

        currentState = GameState.Dead;

        Debug.Log("Game State: Dead");
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}