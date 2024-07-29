using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� �������� �ҷ��� �����մϴ�.
/// </summary>
/// 
public class PuzzlePieceScrollView : MonoBehaviour
{
    [SerializeField] public List<GameObject> pieces;
    [SerializeField] PuzzlePieceItem piece;
    [SerializeField] GameObject content;
    [SerializeField] ScrollRect scroll;

    //���� ������ ���� UI-Image �������� �����Ѵ�.
    public void RegistPiece()
    {
        RectTransform myRect = GetComponent<RectTransform>();

        //��� ������ �����͸� ������
        PieceData[] datas = PuzzleDictionary.Instance.GetVariablePieceData();

        if (datas == null) { Debug.Log("����� ���� ���� ������ ����"); return; }

        foreach (var data in datas)
        {
            GameObject item = Instantiate(piece.gameObject, content.transform);
            item.GetComponent<PuzzlePieceItem>().Initialize(data, myRect.sizeDelta.y, this);
            pieces.Add(item);
        }
    }

    //��� ���� �����͸� �����մϴ�.
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
