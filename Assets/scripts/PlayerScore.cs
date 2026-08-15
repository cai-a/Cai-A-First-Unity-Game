using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText;

    public TMP_Text winText;

    public void AddPoint()
{
    score += 1;

    scoreText.text = "Score: " + score;

    Debug.Log("Score: " + score);

    if (score==5) 
    {
        winText.gameObject.SetActive(true);
    }
    
}
}