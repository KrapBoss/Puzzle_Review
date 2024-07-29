using Custom;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ��ũ�Ѻ信 �����ϴ� �����ۿ� ���� ������ �����Ѵ�.
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

    float size; // ���� ������Ʈ�� Y ������

    bool down;      //��ġ�� ����
    bool downStay;  // ������ �ӹ��� �ִ� ���

    PuzzlePieceScrollView myView;

    public void Initialize(PieceData data, float _size, PuzzlePieceScrollView view)
    {
        imageMine = GetComponent<Image>();
        myView = view;
        puzzleData = data;
        size = _size * 0.75f;

        Selectable();

        //�̹��� ������ ���� �̹��� ũ�� ����
        float sizeX = puzzleData.Sprite.bounds.size.x;
        float sizeY = puzzleData.Sprite.bounds.size.y;

        //������ ����
        imageMine.rectTransform.sizeDelta = new Vector2(_size, _size);
        imageFrame.rectTransform.sizeDelta = new Vector2(_size, _size);
        imageSelect.rectTransform.sizeDelta = new Vector2(_size, _size);

        //���� ���� �̹����� ������ ���� ������ ����
        float ratio = sizeX > sizeY ? sizeY / sizeX : sizeX / sizeY;
        imageItem.rectTransform.sizeDelta = (sizeX > sizeY ? new Vector2(size, size * ratio) : new Vector2(size * ratio, size)) * Mathf.Abs(puzzleData.ScaleX);
        imageItem.sprite = puzzleData.Sprite;

        //���� ���� ǥ��
        textCount.text = $"{PuzzleDictionary.Instance.GetPieceCount(puzzleData.PieceName)}";
    }

    //���� ������ �������� ������ ��� ����
    public void CancleTransition()
    {
        Debug.Log("������ ȸ�� �մϴ�.");

        Selectable();
    }

    //���� ������ �ڸ��� ã�ư��� ���
    public void SuccessTransition()
    {
        Debug.Log("���������� �ڸ��� ã�ư����ϴ�");

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
        CustomDebug.PrintW($"����Ʈ �ٿ� {transform.name}");
        down = true;
        downStay = true;
        _overTime = 0;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //CustomDebug.PrintW($"����Ʈ ���� {transform.name}");
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

        Debug.Log("���" + localPointerPosition);

        //��� ������ ������� �Ǵ��Ѵ�.
        bool overTheRect = localPointerPosition.y > (imageMine.rectTransform.rect.height * OffserOver * (1 - imageMine.rectTransform.pivot.y));

        if (overTheRect)
        {
            Debug.Log("���� ������ �����մϴ�");
            PuzzleDictionary.Instance.GetPiece(puzzleData.PieceName).StartTransition(CancleTransition, SuccessTransition);

            myView.StopScroll();

            //��ġ ��Ȱ��ȭ
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
