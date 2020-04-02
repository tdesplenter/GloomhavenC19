using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  private void Start()
  {
    // Screen resolution
    //Screen.SetResolution(1920, 1080, false);
  }

  public void LoadScene(string sceneName)
  {
    Debug.Log($"Loading {sceneName}");
    SceneManager.LoadScene(sceneName);
  }

  public void BackToMainMenu()
  {
    SceneManager.LoadScene(0);
  }
}
