using System;

/// <summary>
/// used to Observer
/// </summary>
public class EventManager
{
    static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null) instance = new EventManager();
            return instance;
        }
    }

    public Action<bool> action_SetNight;

    //���� �� �ʱ�ȭ �� �̺�Ʈ�� ����
    public Action action_Starting { get; set; }
}


//�㳷 ��带 ����ϱ� ����
public interface INight
{
    public void SetNight(bool night);
}