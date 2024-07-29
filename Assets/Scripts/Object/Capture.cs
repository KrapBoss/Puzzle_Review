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
        CustomDebug.PrintW("이미지 캡쳐를 시작합니다.");

        renderCamera.enabled = true;

        // 오브젝트들의 경계 영역 계산
        Bounds bounds = CalculateBounds(boundObject);

        // 카메라의 Orthographic Size 및 위치 설정
        renderCamera.orthographic = true;
        renderCamera.orthographicSize = bounds.extents.y;
        renderCamera.transform.position = new Vector3(bounds.center.x, bounds.center.y, renderCamera.transform.position.z);

        // RenderTexture 크기 설정
        int textureWidth = (int)(bounds.size.x * 100); // 픽셀 단위 크기 (필요에 따라 조정)
        int textureHeight = (int)(bounds.size.y * 100);

        // RenderTexture 생성 및 설정
        RenderTexture renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
        renderCamera.targetTexture = renderTexture;

        // 카메라로 렌더링
        renderCamera.Render();

        // RenderTexture를 Texture2D로 변환
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
        texture.Apply();


        // RenderTexture 사용 해제
        renderCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        renderCamera.enabled = false;

        if (Save)
        {
            Picture = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Picture.name = boundObject.name;
        }

        //이전에 사용하지 않는 이미지에 대해 리소스를 제거한다.
        Resources.UnloadUnusedAssets();

        return texture;

        //// 파일로 저장
        //byte[] bytes = texture.EncodeToPNG();
        //File.WriteAllBytes(filePath, bytes);

        //// RenderTexture 사용 해제
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
