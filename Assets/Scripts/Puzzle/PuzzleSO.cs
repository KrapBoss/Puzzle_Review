using UnityEngine;

/// <summary>
/// �� ���� �帣�� � ������ ����ִ��� Ȯ���մϴ�.
/// </summary>

[CreateAssetMenu(fileName = "NewPuzzle", menuName = "Puzzle/PuzzleData")]
public class PuzzleSO : ScriptableObject
{
    public string classific;    //�ش� ������ �з�
    public string bgm;          //�ش� ������ �����
    public Texture2D background;//���ȭ��
    public GameObject Puzzle;   //����
}
