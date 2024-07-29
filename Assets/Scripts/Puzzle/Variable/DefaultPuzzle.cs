using Custom;
using Fusion;
using System;
using UnityEngine;

/// <summary>
/// ���� ���� �����͸� �����ϰ� �ִ�.
/// </summary>
public class DefaultPuzzle : Piece
{
    [SerializeField] Vector2 Offset;                //���� ������ ���� ���� ��ġ ������
     Vector2 _defalutOffset = new Vector2(0,-0.5f);//�⺻������ Ŭ�� �� ������ �� ���̵��� �ϱ� ����

    bool isTransfer;    //�̵��� ���� ����

    readonly float FitRange = 0.3f;

    Action action_NotFit;
    Action action_Fit;

    NetworkTransform networkTF;

    private void Awake()
    {
        networkTF = GetComponent<NetworkTransform>();
    }

    private void Update()
    {
        if (data.Activation) return;

        ProgressTransition();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(Offset.x, Offset.y, 0)+new Vector3(_defalutOffset.x, _defalutOffset.y, 0) + transform.position , FitRange);
    }

    void SetData()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        data.Sorting = spriteRenderer[0].sortingOrder;
        data.Sprite = spriteRenderer[0].sprite;
        data.PieceName = gameObject.name;
        data.FitPosition = transform.localPosition;
        data.Activation = false;
        data.ScaleX = transform.localScale.x;
    }

    //�ش� �̸��� �ߺ� ������ �����ϴ��� �Ǵ��մϴ�.
    bool SearchPuzzle()
    {
        Piece[] Piece = PuzzleDictionary.Instance.GetPieceWithName(data.PieceName);

        Vector2 _fit = transform.localPosition;
        transform.localPosition += new Vector3(100, 100, 0);

        foreach (var p in Piece)
        {
            if (p.data.Activation) continue;
            CustomDebug.PrintE($"{p.name} {p == null}");
            if (p.Fit(_fit)) return true;   
        }

        return false;
    }

    //���� ���߱�
    public override bool Fit(Vector2 position)
    {
        float range = (position - data.FitPosition).magnitude; 
        if (range > FitRange)
        {
            Debug.Log($"{name} �� ���� ������ Ʋ���ϴ�.");

            return false;
        }

        //this.gameObject.SetActive(true);

        data.Activation = true;

        container.FitThePiece();

        foreach (var sprite in spriteRenderer)sprite.sortingOrder = data.Sorting;
        transform.localPosition = data.FitPosition;

        return true;
    }

    public override void Initialize(PuzzleContainer _container)
    {
        container = _container;

        SetData();

        //���� �� ���� �Ÿ��� ���� ������ ���� ���Ƿ� ��ġ ���� 
        transform.localPosition += new Vector3(100, 100, 0);

        foreach (var sprite in spriteRenderer) sprite.sortingOrder = 1;

        //this.gameObject.SetActive(false);
    }

    public override void StartTransition(Action _notFit, Action _fit)
    {
        isTransfer = true;
        action_NotFit = _notFit;
        action_Fit = _fit;

        //gameObject.SetActive(true);
    }

    public override void ProgressTransition()
    {
        if (isTransfer)
        {
            //��ġ�Է��� �����
            if (InputManager.Instance.input.touchState == InputData.TouchState.Up)
            {
                StopTransition();
                return;
            }

            Vector3 pos = InputManager.Instance.input.S2WTouchPosition + -Offset - _defalutOffset;
            transform.position = pos ;
            //CustomDebug.PrintW($"{InputManager.Instance.input.S2WTouchPosition + -Offset - _defalutOffset}");
        }
    }

    public override void StopTransition()
    {
        isTransfer = false;
        //gameObject.SetActive(false);

        //1. ���� �ڸ��� �´°�?
        if (SearchPuzzle())
        {
            if (action_Fit != null) action_Fit();
            action_Fit = null;
        }
        //2. ���� �ڸ��� Ʋ����?
        else
        {
            //������ ������ ������
            if (action_NotFit != null) action_NotFit();
            action_NotFit = null;

            //���� �� ���� �Ÿ��� ���� ������ ���� ���Ƿ� ��ġ ���� 
            transform.localPosition += new Vector3(100, 100, 0);
        }
    }
}
