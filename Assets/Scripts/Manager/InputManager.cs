using UnityEngine;

/// <summary>
/// ���� ���� �� ������� �Է� ���� �Ǵ��ϰ� �Ǵܵ� �Է��� ������ �ֽ��ϴ�.
/// </summary>
public class InputManager : MonoBehaviour
{
    static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            if (instance == null) { instance = new InputManager(); Debug.LogWarning("InputManager ����!!!."); }
            return instance;
        }
    }

    [HideInInspector] public InputData input { get; private set; }

    private void Awake()
    {
        Debug.LogWarning("InputManager �����˴ϴ�.");
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
        if (instance != null) { instance = null; Debug.LogWarning("InputManager �ı�"); }
    }

    void MobileTouch()
    {
        // �� �հ��� ��ġ ��ǥ ����
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            input.OriginTouchPosition = touch.position;
        }

        // �� �հ��� ��ġ�� ���� ��
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // ���� ��ġ�� ���� ��ġ�� �Ÿ� ���
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // �� ��ġ ������ �Ÿ� ���� ���
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            input.Scroll = deltaMagnitudeDiff;
        }
    }

    void WindowTouch()
    {
        //Debug.Log($"��ġ ���� �� {Input.GetAxis("Mouse ScrollWheel")}");

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
