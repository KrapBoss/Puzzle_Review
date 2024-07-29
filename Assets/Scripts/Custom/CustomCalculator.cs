using UnityEngine;

public struct RectBoundaryPosition
{
    public Vector2 BottonLeft;
    public Vector2 TopRight;
}

namespace Custom
{
    public static class CustomCalculator
    {
        /// <summary>
        /// 전달받은 RectTransform에 대한 실제 Screen에 대응되는 범위에 해당하는 위치값을 반환한다.
        /// </summary>
        /// <param name="rect">범위를 알고 싶은 Rect</param>
        /// <param name="rectPos">값을 저장할 구조체</param>
        public static void GetBoundaryPosition(RectTransform rect, out RectBoundaryPosition rectPos)
        {
            //피봇을 기준으로 한 가로 세로 반지름값
            float xPivotWidth = rect.pivot.x * rect.rect.width;
            float yPivotHeight = rect.pivot.y * rect.rect.height;

            //피봇과 고려한 xMin, yMax 값에서 오차값을 더해 (0,0)[좌하] ~ (W, H)[우상] 값으로 변환
            rectPos.BottonLeft = new Vector2(rect.rect.xMin + xPivotWidth, rect.rect.yMin + yPivotHeight);
            rectPos.TopRight = new Vector2(rect.rect.xMax + xPivotWidth, rect.rect.yMax + yPivotHeight);

            //실제 스크린 사이즈에 맞도록 변경하는 로직이 필요하다.

        }

        public static Vector3 Clamp(Vector2 position, Vector2 leftBottom, Vector2 RightTop)
        {
            Vector3 pos = position;
            pos.x = Mathf.Clamp(pos.x, leftBottom.x, RightTop.x);
            pos.y = Mathf.Clamp(pos.y, leftBottom.y, RightTop.y);
            pos.z = 0;
            return pos;
        }
    }

}
