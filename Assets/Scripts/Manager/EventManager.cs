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

    //시작 시 초기화 할 이벤트를 저장
    public Action action_Starting { get; set; }
}


//밤낮 모드를 사용하기 위함
public interface INight
{
    public void SetNight(bool night);
}