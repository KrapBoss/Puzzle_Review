using Fusion;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 퍼즐 정렬을 위한 중간 데이터
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

        if (datas.Length < 1 || datas == null) { Debug.LogWarning("PuzzleData[] 존재하지 않습니다."); return; }

        //z 축을 기준으로 내림차순으로 정렬합니다. (- 값 일수록 카메라와 가까워짐)
        PuzzleSortData[] puzzleDatas = (from data in datas
                                        orderby data.transform.position.z ascending // 오름차순
                                        select data).ToArray();

        int sorting = -1;
        foreach (PuzzleSortData data in puzzleDatas)
        {
            data.SetSort(sorting--);
        }

        Debug.Log($"1 : {puzzleDatas[0].name} / LAST  {puzzleDatas[puzzleDatas.Length - 1].name} PuzzleData Sort 정렬 완료");
    }


    //하위 데이터 초기화
    public void Initialize()
    {
        if (datas.Length < 1 || datas == null) { Debug.LogWarning("PuzzleData[] 존재하지 않습니다."); return; }

        foreach (PuzzleSortData data in datas)
        {
            data.SetInit();
        }

        datas = null;

        Debug.Log("하위 데이터 정렬 초기화");
    }

    public void FindChildData()
    {
        datas = GetComponentsInChildren<PuzzleSortData>();
    }

    //모든 퍼즐 조각 들을 네트워크용으로 만든다.
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

            //네트워크 오브젝트에서 처리하는 것이 아니라 Update에서 처리함
            a.DisableSharedModeInterpolation = true;
        }
    }
}
