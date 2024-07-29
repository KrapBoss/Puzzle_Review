using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TapMenuButton : MonoBehaviour
{
    [SerializeField] string myName;
    [SerializeField] TapMenuPanel connectedPanel;
    [SerializeField] Image Selector;

    [HideInInspector] public Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        GetComponentInChildren<TMP_Text>().text = myName;
    }

    public void Deactivate()
    {
        //CustomDebug.Print($"{transform.name} 의 버튼이 비활성화되었음");
        connectedPanel.DeActive();
        SelectorActive(false);
    }

    public void Activate()
    {
        //CustomDebug.Print($"{transform.name} 의 버튼이 활성화되었음");
        connectedPanel.Active();
        SelectorActive(true);
    }

    void SelectorActive(bool isActive)
    {
        if (Selector == null) return;

        Selector.enabled = isActive;
    }
}
