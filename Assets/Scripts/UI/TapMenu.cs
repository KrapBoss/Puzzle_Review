using UnityEngine;

public class TapMenu : MonoBehaviour
{
    [SerializeField] int defaultIndex;
    [SerializeField] TapMenuButton[] menu;


    private void Awake()
    {
        menu = GetComponentsInChildren<TapMenuButton>();
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        for (int i = 0; i < menu.Length; i++)
        {
            int index = i;
            menu[i].btn.onClick.AddListener(() => ActiveThePanel(index));
            menu[i].Deactivate();
        }

        if (menu.Length <= defaultIndex) defaultIndex = 0;
        menu[defaultIndex].Activate();
    }

    void ActiveThePanel(int index)
    {
        foreach (var item in menu)
        {
            item.Deactivate();
        }

        menu[index].Activate();
    }
}
