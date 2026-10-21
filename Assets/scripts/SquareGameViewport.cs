using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SquareGameViewport : MonoBehaviour
{
    private Camera gameCamera;

    private void Awake()
    {
        gameCamera = GetComponent<Camera>();
        FitViewport();
    }

    private void Update()
    {
        FitViewport();
    }

    private void FitViewport()
    {
        if (Screen.width <= 0 || Screen.height <= 0)
        {
            return;
        }

        float side = Mathf.Min(Screen.width, Screen.height);
        float width = side / Screen.width;
        float height = side / Screen.height;

        gameCamera.rect = new Rect(
            (1f - width) / 2f,
            (1f - height) / 2f,
            width,
            height);

        gameCamera.aspect = 1f;
    }
}
