using UnityEngine;
using UnityEngine.UI;

public class ShowSampleImage : MonoBehaviour, IOnOff
{
    public Image imageSample;
    public RectTransform rectFrame;
    public float scaleOffset = 0.8f;

    [SerializeField] Sprite texture;

    Capture2D capture;

    private void Awake()
    {
        capture = FindObjectOfType<Capture2D>();
    }

    public bool Active { get; set; }

    public bool Off()
    {
        this.gameObject.SetActive(false);

        Active = false;

        return true;
    }

    public bool On()
    {
        this.gameObject.SetActive(true);

        texture = capture.Picture;

        if (texture == null) return false;

        Active = true;

        float size = rectFrame.rect.height * scaleOffset;

        //이미지 비율에 따른 이미지 크기 지정
        float sizeX = texture.bounds.size.x;
        float sizeY = texture.bounds.size.y;
        //float ratio = sizeX > sizeY ? sizeY / sizeX : sizeX / sizeY;
        float ratio = sizeX / sizeY;

        imageSample.rectTransform.sizeDelta = new Vector2(size * ratio, size) * scaleOffset;
        imageSample.sprite = texture;
        return true;
    }
}
