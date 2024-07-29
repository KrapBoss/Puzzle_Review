using Custom;
using UnityEngine;

public class CameraOption : MonoBehaviour, INight
{
    public Vector3 Offset = new Vector3(0, 0, -10);

    Camera cam;

    PuzzleContainer container = null;
    public PuzzleContainer Container { set { CustomDebug.PrintE("���� �����̳� ���"); container = value; } }

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
        t = t > 1 ? t * 1.2f : 1.1f; // ī�޶� ũ�⸦ Ȯ���ų ������ ��Ÿ���ϴ�.

        // ��������Ʈ�� ���� ũ�⸦ ������
        float spriteWidth = container.BackGroundSprite.bounds.size.x;

        // ȭ���� ��Ⱦ�� (����/���� ����)�� ������
        float aspectRatio = Screen.width / (float)Screen.height;

        // ī�޶��� orthographicSize ��� (���� ũ�⿡ ����)
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
