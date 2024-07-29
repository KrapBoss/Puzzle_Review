using Custom;
using UnityEngine;

public class CameraOption : MonoBehaviour, INight
{
    public Vector3 Offset = new Vector3(0, 0, -10);

    Camera cam;

    PuzzleContainer container = null;
    public PuzzleContainer Container { set { CustomDebug.PrintE("퍼즐 컨테이너 등록"); container = value; } }

    private void Awake()
    {
        cam = GetComponent<Camera>();

        GameManager.Instance.InsertGameStateAction(LocalGameState.Paying, ResizeCamera);

        EventManager.Instance.action_SetNight += SetNight;
    }

    private void OnDestroy()
    {
        EventManager.Instance.action_SetNight -= SetNight;
        GameManager.Instance.DeleteGameStateAction(LocalGameState.Paying, ResizeCamera);
    }

    public void ResizeCamera()
    {
        if (container == null) container = FindObjectOfType<PuzzleContainer>();

        if (container == null) return;

        float t = Screen.width / Screen.height;
        t = t > 1 ? t * 1.2f : 1.1f; // 카메라 크기를 확대시킬 정도를 나타냅니다.

        // 스프라이트의 가로 크기를 가져옴
        float spriteWidth = container.BackGroundSprite.bounds.size.x;

        // 화면의 종횡비 (가로/세로 비율)를 가져옴
        float aspectRatio = Screen.width / (float)Screen.height;

        // 카메라의 orthographicSize 계산 (가로 크기에 맞춤)
        cam.orthographicSize = spriteWidth / 2 / aspectRatio * t;

        Vector3 _position = container.transform.position + Offset;
        _position.z = transform.position.z;
        transform.position = _position;
    }

    public void SetNight(bool night)
    {
        cam.backgroundColor = night ? Color.black : Color.white;
    }
}
