using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int score = 0;

    public void AddPoint()
    {
        score += 1;

        Debug.Log("Score: " + score);
    }
}