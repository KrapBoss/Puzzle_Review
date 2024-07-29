using UnityEngine;
/// <summary>
/// �Է� ������
/// </summary>
public class InputData : MonoBehaviour
{
    public enum TouchState
    {
        Down, Move, Up
    }

    public Vector2 S2WTouchPosition;
    public Vector2 OriginTouchPosition;
    public TouchState touchState;
    public float Scroll;
}
