using Fusion;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���� ������ ���� �߰� ������
/// </summary>
public class PuzzleSortContainer : MonoBehaviour
{
    [SerializeField] PuzzleSortData[] datas;

    private void Awake()
    {
        Destroy(this);
    }

    public void SortAllPuzzleData()
    {
        FindChildData();

        if (datas.Length < 1 || datas == null) { Debug.LogWarning("PuzzleData[] �������� �ʽ��ϴ�."); return; }

        //z ���� �������� ������������ �����մϴ�. (- �� �ϼ��� ī�޶�� �������)
        PuzzleSortData[] puzzleDatas = (from data in datas
                                        orderby data.transform.position.z ascending // ��������
                                        select data).ToArray();

        int sorting = -1;
        foreach (PuzzleSortData data in puzzleDatas)
        {
            data.SetSort(sorting--);
        }

        Debug.Log($"1 : {puzzleDatas[0].name} / LAST  {puzzleDatas[puzzleDatas.Length - 1].name} PuzzleData Sort ���� �Ϸ�");
    }


    //���� ������ �ʱ�ȭ
    public void Initialize()
    {
        if (datas.Length < 1 || datas == null) { Debug.LogWarning("PuzzleData[] �������� �ʽ��ϴ�."); return; }

        foreach (PuzzleSortData data in datas)
        {
            data.SetInit();
        }

        datas = null;

        Debug.Log("���� ������ ���� �ʱ�ȭ");
    }

    public void FindChildData()
    {
        datas = GetComponentsInChildren<PuzzleSortData>();
    }

    //��� ���� ���� ���� ��Ʈ��ũ������ �����.
    public void MakeNetworkPiece()
    {
        if (datas.Length == 0) return;

        var myNO = GetComponent<NetworkObject>();
        if (myNO == null) transform.AddComponent<NetworkObject>();
        var myTR = transform.GetComponent<NetworkTransform>();
        if (myTR == null) transform.AddComponent<NetworkTransform>();
        myTR.DisableSharedModeInterpolation = true;


        foreach (PuzzleSortData data in datas)
        {
            var a = data.GetComponent<NetworkTransform>();
            if (a == null) data.AddComponent<NetworkTransform>();

            //��Ʈ��ũ ������Ʈ���� ó���ϴ� ���� �ƴ϶� Update���� ó����
            a.DisableSharedModeInterpolation = true;
        }
    }
}
