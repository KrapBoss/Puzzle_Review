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

        CustomDebug.Print($"{transform.name} Ȱ��ȭ");

        gameObject.SetActive(true);
    }

    public void DeActive()
    {
        if (!gameObject.activeSelf) return;

        CustomDebug.Print($"{transform.name} ��Ȱ��ȭ");

        //��� ������Ʈ ��Ȱ��ȭ
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
