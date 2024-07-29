using UnityEngine;

/// <summary>
/// 퍼즐 정렬을 위한 개별 데이터
/// </summary>
public class PuzzleSortData : MonoBehaviour
{
    [SerializeField] int Sort = -999;//정렬 번호
    [SerializeField] SpriteRenderer[] spriteRenderer;//모든 스프라이트 렌더러


    private void Awake()
    {
        Destroy(this);
    }

    public void SetSort(int sort)
    {
        if (Sort != -999) { Debug.LogWarning($"이미 정렬된 값이 존재합니다. SortingOrder : {Sort}"); return; }

        FindSpriteRenderer();

        Sort = sort;
        foreach (var renderer in spriteRenderer) renderer.sortingOrder = Sort;
    }

    public void SetInit()
    {
        Sort = -999;
        foreach (var renderer in spriteRenderer) renderer.sortingOrder = 0;

        spriteRenderer = null;
    }

    //하위 스프라이트 렌더러를 찾습니다.
    public void FindSpriteRenderer()
    {
        //if (spriteRenderer.Length > 0) return;
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
    }
}
