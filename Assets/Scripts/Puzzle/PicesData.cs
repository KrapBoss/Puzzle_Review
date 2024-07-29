using System;
using UnityEngine;

/// <summary>
/// 각 퍼즐 조각을 위한 데이터
/// </summary>
[System.Serializable]
public class PieceData
{
    public string PieceName;
    public Sprite Sprite;
    public Vector2 FitPosition;
    public int Sorting;
    public bool Activation;
    public float ScaleX;// 각 이미지 별로 크기를 상이하게 하기 위해
}

/// <summary>
/// 퍼즐 조각은 반드시 상속을 받아서 사용
/// </summary>
public abstract class Piece : MonoBehaviour, IPieceMovement
{
    public PieceData data;                  //각 퍼즐 데이터
    public SpriteRenderer[] spriteRenderer; //솔팅을 위한 것
    protected PuzzleContainer container;

    public abstract void Initialize(PuzzleContainer _container);
    public abstract void StartTransition(Action _notFit, Action _fit);
    public abstract void ProgressTransition();
    public abstract void StopTransition();
    public abstract bool Fit(Vector2 fitPosition);
}


//퍼즐의 움직임 구현을 위한 것
public interface IPieceMovement
{
    //입력 시작을 알림
    public void StartTransition(Action _notFit, Action _fit);

    //입력 시작 시 이동합니다.
    public void ProgressTransition();

    //입력을 종료합니다.
    public void StopTransition();
}