using Custom;
using UnityEngine;

public class CameraResizeButton : MonoBehaviour, IOnOff
{
    CameraOption option;

    public bool Active { get; set; }

    public bool Off()
    {
        CustomDebug.PrintW("ī�޶� �� ����¡");

        if (option == null) option = FindObjectOfType(typeof(CameraOption)) as CameraOption;

        //�׻� Off �� �ߵ� ��ŰŰ ����
        Active = true;

        if (option != null)
        {
            option.ResizeCamera();
        }

        return true;
    }

    public bool On()
    {
        return false;
    }
}
