using Custom;
using UnityEngine;

public class TapMenuPanel : MonoBehaviour
{
    IOnOff[] iOnOff;

    private void Awake()
    {
        iOnOff = GetComponentsInChildren<IOnOff>();
    }

    public void Active()
    {
        if (gameObject.activeSelf) return;

        CustomDebug.Print($"{transform.name} 활성화");

        gameObject.SetActive(true);
    }

    public void DeActive()
    {
        if (!gameObject.activeSelf) return;

        CustomDebug.Print($"{transform.name} 비활성화");

        //모든 오브젝트 비활성화
        if (iOnOff != null)
        {
            foreach (var item in iOnOff)
            {
                if (item.Active) item.Off();
            }
        }

        gameObject.SetActive(false);
    }
}
