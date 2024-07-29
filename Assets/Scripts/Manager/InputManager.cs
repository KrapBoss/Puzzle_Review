using UnityEngine;

/// <summary>
/// 게임 진행 중 사용자의 입력 상태 판단하고 판단된 입력을 가지고 있습니다.
/// </summary>
public class InputManager : MonoBehaviour
{
    static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            if (instance == null) { instance = new InputManager(); Debug.LogWarning("InputManager 생성!!!."); }
            return instance;
        }
    }

    [HideInInspector] public InputData input { get; private set; }

    private void Awake()
    {
        Debug.LogWarning("InputManager 생성됩니다.");
        if (instance == null) instance = this;

        input = FindObjectOfType(typeof(InputData)) as InputData;
        if (input == null) input = new InputData();

        input.touchState = InputData.TouchState.Up;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        WindowTouch();
#elif UNITY_IOS || UNITY_ANDROID
        MobileTouch();
#endif
    }

    private void OnDestroy()
    {
        if (instance != null) { instance = null; Debug.LogWarning("InputManager 파괴"); }
    }

    void MobileTouch()
    {
        // 한 손가락 터치 좌표 감지
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            input.OriginTouchPosition = touch.position;
        }

        // 두 손가락 터치를 통한 줌
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // 이전 위치와 현재 위치의 거리 계산
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // 두 터치 사이의 거리 차이 계산
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            input.Scroll = deltaMagnitudeDiff;
        }
    }

    void WindowTouch()
    {
        //Debug.Log($"터치 동작 중 {Input.GetAxis("Mouse ScrollWheel")}");

        input.Scroll = -Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetMouseButtonDown(0))
        {
            input.S2WTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            input.OriginTouchPosition = Input.mousePosition;
            input.touchState = InputData.TouchState.Down;
        }
        else if (Input.GetMouseButton(0))
        {
            input.S2WTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            input.OriginTouchPosition = Input.mousePosition;
            input.touchState = InputData.TouchState.Move;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            input.S2WTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            input.OriginTouchPosition = Input.mousePosition;
            input.touchState = InputData.TouchState.Up;
        }
    }
}
