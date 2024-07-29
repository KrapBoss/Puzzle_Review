using Custom;
using UnityEngine;

public class SetNightObject : MonoBehaviour, INight
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Color morningColor = Color.black;
    [SerializeField] Color nightColor = Color.white;

    private void Awake()
    {
        EventManager.Instance.action_SetNight += SetNight;
    }

    private void OnDestroy()
    {
        EventManager.Instance.action_SetNight -= SetNight;
    }

    public void SetNight(bool night)
    {
        CustomDebug.PrintW($"{transform.name} ������Ʈ�� ���� �����մϴ�. ");
        if (night) { sr.color = nightColor; }
        else { sr.color = morningColor; }
    }
}
