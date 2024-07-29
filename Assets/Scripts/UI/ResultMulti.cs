using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//��Ƽ������ ���ȭ���� �����ش�.
public class ResultMulti : MonoBehaviour
{
    [SerializeField] Image capture;
    [SerializeField] RectTransform capture_parent;
    List<ResultMultiEntity> entities = new List<ResultMultiEntity>();

    [SerializeField] GameObject entity;
    [SerializeField] Transform entityParent;
    //[SerializeField] TMP_Text text_Reult;

    public void ShowResult(PlayerDataNetwork[] data)
    {
        if (gameObject.activeSelf) return;

        gameObject.SetActive(true);

        Sprite sprite = FindObjectOfType<Capture2D>()?.Picture;

        float size = capture_parent.rect.height * 0.9f;

        //�̹��� ������ ���� �̹��� ũ�� ����
        float sizeX = sprite.bounds.size.x;
        float sizeY = sprite.bounds.size.y;
        //float ratio = sizeX > sizeY ? sizeY / sizeX : sizeX / sizeY;
        float ratio = sizeX / sizeY;

        capture.rectTransform.sizeDelta = new Vector2(size * ratio, size);
        capture.sprite = sprite;

        capture.sprite = FindObjectOfType<Capture2D>()?.Picture;

        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == null) continue;

            entities.Add(Instantiate(entity, entityParent).GetComponent<ResultMultiEntity>());

            entities[i].ShowResult(data[i]);
        }
    }
}
