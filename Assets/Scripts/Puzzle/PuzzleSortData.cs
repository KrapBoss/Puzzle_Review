using UnityEngine;

/// <summary>
/// ���� ������ ���� ���� ������
/// </summary>
public class PuzzleSortData : MonoBehaviour
{
    [SerializeField] int Sort = -999;//���� ��ȣ
    [SerializeField] SpriteRenderer[] spriteRenderer;//��� ��������Ʈ ������


    private void Awake()
    {
        Destroy(this);
    }

    public void SetSort(int sort)
    {
        if (Sort != -999) { Debug.LogWarning($"�̹� ���ĵ� ���� �����մϴ�. SortingOrder : {Sort}"); return; }

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

    //���� ��������Ʈ �������� ã���ϴ�.
    public void FindSpriteRenderer()
    {
        //if (spriteRenderer.Length > 0) return;
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
    }
}
