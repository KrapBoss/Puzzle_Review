using Custom;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Vector3 Offset = new Vector3(0, 0, -10);
    [Header("Movement")]
    public float MovementSensitivity = 1.0f;
    public Vector2 MinLeftBotton = new Vector2(-5, -5);
    public Vector2 MaxRightTop = new Vector2(5, 5);

    [Header("Scroll")]
    public float ScrollSensitivity = 1.0f;
    public float MinOrthoSize = 3.0f;
    public float MaxOrthoSize = 15.0f;


    [Header("자동 참조 값")]
    public GraphicRaycaster graphicRaycaster;
    public EventSystem eventSystem;
    [SerializeField] Vector3 previosPosition;
    [SerializeField] Vector3 currentPosition;

    Camera cam;

    InputData input;

    bool Down; // 현재 마우스를 누른 것으로 판단할 지 나타냅니다.


    private void Awake()
    {
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.Instance.input;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ZoomInOut();
    }


    void Movement()
    {
        switch (input.touchState)
        {
            case InputData.TouchState.Down:
                if (IsPointerOverUIElement(input.OriginTouchPosition)) break;

                //Debug.Log("다운");

                Down = true;
                previosPosition = input.S2WTouchPosition;

                break;
            case InputData.TouchState.Move:
                if (!Down) break;

                //Debug.Log("무브");

                currentPosition = input.S2WTouchPosition;

                Vector3 difference = currentPosition - previosPosition;
                Vector3 pos = cam.transform.position - difference * MovementSensitivity;

                //Debug.Log(pos);

                cam.transform.position = CustomCalculator.Clamp(pos, MinLeftBotton, MaxRightTop) + Offset;

                //이동한 만큼 빼서 이전 위치 값으로 사용
                //Cam.ScreenToWorldPoint가 이동된 위치를 기준으로 동작하기 때문
                previosPosition = currentPosition - (difference * MovementSensitivity);

                break;
            case InputData.TouchState.Up:
                //Debug.Log("업");
                Down = false;
                previosPosition = Vector2.zero;
                break;
        }
    }

    void ZoomInOut()
    {
        if (input.Scroll != 0.0f)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + input.Scroll * ScrollSensitivity, MinOrthoSize, MaxOrthoSize);
        }
    }

    private bool IsPointerOverUIElement(Vector2 position)
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = position;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        return raycastResults.Count > 0;
    }
}
