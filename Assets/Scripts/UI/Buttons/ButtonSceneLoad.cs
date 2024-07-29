using Custom;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSceneLoad : MonoBehaviour
{
    public string SceneName;
    private void Awake()
    {
        CustomDebug.PrintW($"{SceneName} �� �ε��մϴ�.");
        GetComponent<Button>().onClick.AddListener(
            () =>
            {
                SceneManager.LoadScene(SceneName);
            }
            );
    }
}
