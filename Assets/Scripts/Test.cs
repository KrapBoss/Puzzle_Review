using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"{eventData.position}");
    }

    // Start is called before the first frame update
    void Start()
    {
        var a = Resources.LoadAll<GameObject>("Network/Puzzle");
        foreach (GameObject go in a) Debug.Log(go.name);
    }
}
