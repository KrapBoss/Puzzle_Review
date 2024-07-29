using System;
using UnityEngine;

/// <summary>
/// �� ���� ������ ���� ������
/// </summary>
[System.Serializable]
public class PieceData
{
    public string PieceName;
    public Sprite Sprite;
    public Vector2 FitPosition;
    public int Sorting;
    public bool Activation;
    public float ScaleX;// �� �̹��� ���� ũ�⸦ �����ϰ� �ϱ� ����
}

/// <summary>
/// ���� ������ �ݵ�� ����� �޾Ƽ� ���
/// </summary>
public abstract class Piece : MonoBehaviour, IPieceMovement
{
    public PieceData data;                  //�� ���� ������
    public SpriteRenderer[] spriteRenderer; //������ ���� ��
    protected PuzzleContainer container;

    public abstract void Initialize(PuzzleContainer _container);
    public abstract void StartTransition(Action _notFit, Action _fit);
    public abstract void ProgressTransition();
    public abstract void StopTransition();
    public abstract bool Fit(Vector2 fitPosition);
}


//������ ������ ������ ���� ��
public interface IPieceMovement
{
    //�Է� ������ �˸�
    public void StartTransition(Action _notFit, Action _fit);

    //�Է� ���� �� �̵��մϴ�.
    public void ProgressTransition();

    //�Է��� �����մϴ�.
    public void StopTransition();
}