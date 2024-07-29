using Custom;
using UnityEngine;

public class Capture2D : MonoBehaviour
{
    [SerializeField] Camera renderCamera;

    public Sprite Picture { get; private set; }

    //public Transform[] objectsToCapture;
    //public string filePath = "Assets/Screenshot.png";

    public Texture2D Capture(Transform boundObject, bool Save)
    {
        CustomDebug.PrintW("�̹��� ĸ�ĸ� �����մϴ�.");

        renderCamera.enabled = true;

        // ������Ʈ���� ��� ���� ���
        Bounds bounds = CalculateBounds(boundObject);

        // ī�޶��� Orthographic Size �� ��ġ ����
        renderCamera.orthographic = true;
        renderCamera.orthographicSize = bounds.extents.y;
        renderCamera.transform.position = new Vector3(bounds.center.x, bounds.center.y, renderCamera.transform.position.z);

        // RenderTexture ũ�� ����
        int textureWidth = (int)(bounds.size.x * 100); // �ȼ� ���� ũ�� (�ʿ信 ���� ����)
        int textureHeight = (int)(bounds.size.y * 100);

        // RenderTexture ���� �� ����
        RenderTexture renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
        renderCamera.targetTexture = renderTexture;

        // ī�޶�� ������
        renderCamera.Render();

        // RenderTexture�� Texture2D�� ��ȯ
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
        texture.Apply();


        // RenderTexture ��� ����
        renderCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        renderCamera.enabled = false;

        if (Save)
        {
            Picture = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Picture.name = boundObject.name;
        }

        //������ ������� �ʴ� �̹����� ���� ���ҽ��� �����Ѵ�.
        Resources.UnloadUnusedAssets();

        return texture;

        //// ���Ϸ� ����
        //byte[] bytes = texture.EncodeToPNG();
        //File.WriteAllBytes(filePath, bytes);

        //// RenderTexture ��� ����
        //renderCamera.targetTexture = null;
        //RenderTexture.active = null;
        //Destroy(renderTexture);

        //Debug.Log("Screenshot saved to: " + filePath);
    }

    private Bounds CalculateBounds(Transform objects)
    {
        if (objects == null)
        {
            return new Bounds();
        }

        Bounds bounds = new Bounds(objects.position, Vector3.zero);
        bounds.Encapsulate(objects.GetComponent<Renderer>().bounds);

        return bounds;
    }
}
