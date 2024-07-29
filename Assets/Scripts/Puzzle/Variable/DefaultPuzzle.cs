using Custom;
using Fusion;
using System;
using UnityEngine;

/// <summary>
/// 퍼즐에 대한 데이터를 소유하고 있다.
/// </summary>
public class DefaultPuzzle : Piece
{
    [SerializeField] Vector2 Offset;                //실제 퍼즐을 잡을 때의 위치 오프셋
     Vector2 _defalutOffset = new Vector2(0,-0.5f);//기본적으로 클릭 시 퍼즐이 잘 보이도록 하기 위함

    bool isTransfer;    //이동을 위한 변수

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

    //해당 이름의 중복 퍼즐이 존재하는지 판단합니다.
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

    //퍼즐 맞추기
    public override bool Fit(Vector2 position)
    {
        float range = (position - data.FitPosition).magnitude; 
        if (range > FitRange)
        {
            Debug.Log($"{name} 의 퍼즐 조각이 틀립니다.");

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

        //생성 시 깜빡 거리는 현상 방지를 위해 임의로 위치 변경 
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
            //터치입력이 종료됨
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

        //1. 현재 자리가 맞는가?
        if (SearchPuzzle())
        {
            if (action_Fit != null) action_Fit();
            action_Fit = null;
        }
        //2. 현재 자리가 틀린가?
        else
        {
            //지정된 동작을 수행함
            if (action_NotFit != null) action_NotFit();
            action_NotFit = null;

            //생성 시 깜빡 거리는 현상 방지를 위해 임의로 위치 변경 
            transform.localPosition += new Vector3(100, 100, 0);
        }
    }
}
