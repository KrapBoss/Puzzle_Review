using Custom;
using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// ��� ������ Ʋ
/// ������ �ϼ�
/// </summary>
public class PuzzleContainer : MonoBehaviour
{
    //[SerializeField] Piece[] datas;
    [SerializeField] Piece BackGround; // ��׶���� ���� ���� ������� ������� �ʽ��ϴ�.
    Capture2D capture;

    //�ϼ���
    public float Perfection = 0;
    //�� ���� ���� ��
    public int CountPiece;

    //������ �¾��� ��� ������ ���� ����
    public Action<float> Action_Fit;

    public Sprite BackGroundSprite
    {
        get
        {
            if (BackGround.data.Sprite == null)
            {
                BackGround.Initialize(this);
            }
            return BackGround.data.Sprite;
        }
    }//��׶��� ��������Ʈ�� ��ȯ

    //���� ������ ������ ����԰� ���ÿ� �ʱ�ȭ�� �����Ѵ�.
    public void Initialize()
    {
        CustomDebug.PrintW($"PuzzleContainer {transform.name} Awake");

        PuzzleDictionary.Instance.Clear();

        transform.position = Vector3.zero;

        //���� ���
        RegisterPiece();
        
        BackGround.Fit(Vector2.zero);

        Perfection = 0;
        //������ ��� �Ͽ� ����ڿ��� �����ش�.
        FindObjectOfType<PuzzlePieceScrollView>()?.RegistPiece();
    }

    void RegisterPiece()
    {
        //����
        Piece[] datas = GetComponentsInChildren<Piece>();

        if (datas.Length < 1) { Debug.LogWarning($"{name} �� ���� �������� �����ϴ�."); return; }

        //�̸����⸦ ����
        capture = FindObjectOfType<Capture2D>();
        capture.Capture(BackGround.transform, true);

        //���� ���� �� �ʱ�ȭ
        CountPiece = 0;

        //���� ������ �ʱ� ����
        foreach (Piece data in datas)
        {
            //��Ȱ��ȭ�˴ϴ�.
            data.Initialize(this);

            //��׶���� �������� �ʽ��ϴ�
            if (data == BackGround) continue;

            CountPiece++;
            PuzzleDictionary.Instance.AddPiece(data.data.PieceName, data);
        }
    }

    //���� ������ ��� �´��� Ȯ���մϴ�.
    public void FitThePiece()
    {
        int count = 0;

        //��� ���� ������ �����ɴϴ�.
        PieceData[] datas = PuzzleDictionary.Instance.GetAllPieceData();

        if (datas != null)
        {
            count = datas.Where(x => !x.Activation).ToArray().Length;
        }

        //���� ������ ���� �������ݴϴ�.-------------------------------
        if (Action_Fit != null) Action_Fit((datas.Length - count) / (float)datas.Length);

        if (count > 0)
        {
            Debug.Log($"���� ���� {count}");
        }
        else
        {
            Debug.Log($"���� ���� {count}");
        }
    }

    private void OnDestroy()
    {
        PuzzleDictionary.Instance.Clear();
    }
}
