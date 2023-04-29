using UnityEngine;

public class QuitButton : MonoBehaviour
{
    /// <summary>
    /// Exits the application / Stops play mode
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}