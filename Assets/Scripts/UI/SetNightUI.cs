using UnityEngine;
using UnityEngine.UI;

public class SetNightUI : MonoBehaviour, INight
{
    [SerializeField] Image img;
    [SerializeField] Color morning = Color.white;
    [SerializeField] Color night = Color.black;

    void Awake()
    {
        EventManager.Instance.action_SetNight += SetNight;
    }
    private void OnDestroy()
    {
        EventManager.Instance.action_SetNight -= SetNight;
    }

    public void SetNight(bool n)
    {
        img.color = n ? night : morning;
    }
}
