using Custom;
using UnityEngine;

public class CameraResizeButton : MonoBehaviour, IOnOff
{
    CameraOption option;

    public bool Active { get; set; }

    public bool Off()
    {
        CustomDebug.PrintW("카메라 리 사이징");

        if (option == null) option = FindObjectOfType(typeof(CameraOption)) as CameraOption;

        //항상 Off 를 발동 시키키 위함
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
