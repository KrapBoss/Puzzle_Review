using UnityEngine;

/// <summary>
/// �ش� ������Ʈ�� Ȱ��, ��Ȱ��ȭ ��
/// </summary>
public class BeActived : MonoBehaviour, IOnOff
{
    public bool Active { get; set; }

    public bool Off()
    {
        //CustomDebug.Print("������մϴ�.");
        gameObject.SetActive(false);

        Active = false;
        return Active;
    }

    public bool On()
    {
        //CustomDebug.Print("����մϴ�.");
        gameObject.SetActive(true);

        Active = true;
        return Active;
    }
}
