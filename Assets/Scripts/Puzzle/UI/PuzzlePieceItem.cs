using Custom;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 스크롤뷰에 존재하는 아이템에 대한 동작을 정의한다.
/// </summary>

public class PuzzlePieceItem : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] Image imageItem;
    [SerializeField] Image imageFrame;
    [SerializeField] Image imageSelect;
    [SerializeField] TMP_Text textCount;
    [SerializeField] PieceData puzzleData;

    [SerializeField] float OffserOver = 0.9f;
    [SerializeField] float OverTime = 0.5f;
    float _overTime = 0.1f;

    Image imageMine;

    float size; // 상위 오브젝트의 Y 사이즈

    bool down;      //터치한 상태
    bool downStay;  // 누르고 머물러 있는 경우

    PuzzlePieceScrollView myView;

    public void Initialize(PieceData data, float _size, PuzzlePieceScrollView view)
    {
        imageMine = GetComponent<Image>();
        myView = view;
        puzzleData = data;
        size = _size * 0.75f;

        Selectable();

        //이미지 비율에 따른 이미지 크기 지정
        float sizeX = puzzleData.Sprite.bounds.size.x;
        float sizeY = puzzleData.Sprite.bounds.size.y;

        //사이즈 지정
        imageMine.rectTransform.sizeDelta = new Vector2(_size, _size);
        imageFrame.rectTransform.sizeDelta = new Vector2(_size, _size);
        imageSelect.rectTransform.sizeDelta = new Vector2(_size, _size);

        //퍼즐 조각 이미지의 비율에 따른 사이즈 지정
        float ratio = sizeX > sizeY ? sizeY / sizeX : sizeX / sizeY;
        imageItem.rectTransform.sizeDelta = (sizeX > sizeY ? new Vector2(size, size * ratio) : new Vector2(size * ratio, size)) * Mathf.Abs(puzzleData.ScaleX);
        imageItem.sprite = puzzleData.Sprite;

        //현재 개수 표시
        textCount.text = $"{PuzzleDictionary.Instance.GetPieceCount(puzzleData.PieceName)}";
    }

    //퍼즐 조각의 움직임이 멈췄을 경우 실행
    public void CancleTransition()
    {
        Debug.Log("퍼즐을 회수 합니다.");

        Selectable();
    }

    //퍼즐 조각이 자리를 찾아갔을 경우
    public void SuccessTransition()
    {
        Debug.Log("퍼즐조각이 자리를 찾아갔습니다");

        int cnt = PuzzleDictionary.Instance.GetPieceCount(puzzleData.PieceName);

        if (cnt < 1) { gameObject.SetActive(false); }
        else { textCount.text = $"{cnt}"; Selectable(); }
    }

    private void Update()
    {
        if (downStay) _overTime += Time.deltaTime;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CustomDebug.PrintW($"포인트 다운 {transform.name}");
        down = true;
        downStay = true;
        _overTime = 0;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //CustomDebug.PrintW($"포인트 무브 {transform.name}");
        if (!down) return;
        downStay = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (!down) return;

        OverTheRect(eventData);

        down = false;
    }

    void OverTheRect(PointerEventData eventData)
    {
        CustomDebug.PrintW($"OverTheRect {puzzleData.PieceName}");

        if (_overTime < OverTime) return;

        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(imageMine.rectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition);

        Debug.Log("벗어남" + localPointerPosition);

        //상단 구역을 벗어났는지 판단한다.
        bool overTheRect = localPointerPosition.y > (imageMine.rectTransform.rect.height * OffserOver * (1 - imageMine.rectTransform.pivot.y));

        if (overTheRect)
        {
            Debug.Log("퍼즐 조각을 생성합니다");
            PuzzleDictionary.Instance.GetPiece(puzzleData.PieceName).StartTransition(CancleTransition, SuccessTransition);

            myView.StopScroll();

            //터치 비활성화
            Selected();
        }
    }

    void Selected()
    {
        imageMine.raycastTarget = false;

        imageSelect.enabled = true;
    }

    void Selectable()
    {
        imageMine.raycastTarget = true;

        imageSelect.enabled = false;
    }
}
