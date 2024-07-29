using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퍼즐 조각들을 불러와 생성합니다.
/// </summary>
/// 
public class PuzzlePieceScrollView : MonoBehaviour
{
    [SerializeField] public List<GameObject> pieces;
    [SerializeField] PuzzlePieceItem piece;
    [SerializeField] GameObject content;
    [SerializeField] ScrollRect scroll;

    //퍼즐 진행을 위한 UI-Image 조각들을 생성한다.
    public void RegistPiece()
    {
        RectTransform myRect = GetComponent<RectTransform>();

        //모든 종류의 데이터를 가져옴
        PieceData[] datas = PuzzleDictionary.Instance.GetVariablePieceData();

        if (datas == null) { Debug.Log("등록을 위한 퍼즐 조각이 없음"); return; }

        foreach (var data in datas)
        {
            GameObject item = Instantiate(piece.gameObject, content.transform);
            item.GetComponent<PuzzlePieceItem>().Initialize(data, myRect.sizeDelta.y, this);
            pieces.Add(item);
        }
    }

    //모든 조각 데이터를 삭제합니다.
    public void DestroyPiece()
    {
        foreach (GameObject g in pieces)
        {
            Destroy(g);
        }
    }

    public void StopScroll()
    {
        StartCoroutine(StopCroutine());
    }

    IEnumerator StopCroutine()
    {
        scroll.enabled = false;

        yield return null;

        scroll.enabled = true;
        scroll.velocity = Vector3.zero;
    }
}
