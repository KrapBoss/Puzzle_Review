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
        /// ���޹��� RectTransform�� ���� ���� Screen�� �����Ǵ� ������ �ش��ϴ� ��ġ���� ��ȯ�Ѵ�.
        /// </summary>
        /// <param name="rect">������ �˰� ���� Rect</param>
        /// <param name="rectPos">���� ������ ����ü</param>
        public static void GetBoundaryPosition(RectTransform rect, out RectBoundaryPosition rectPos)
        {
            //�Ǻ��� �������� �� ���� ���� ��������
            float xPivotWidth = rect.pivot.x * rect.rect.width;
            float yPivotHeight = rect.pivot.y * rect.rect.height;

            //�Ǻ��� ����� xMin, yMax ������ �������� ���� (0,0)[����] ~ (W, H)[���] ������ ��ȯ
            rectPos.BottonLeft = new Vector2(rect.rect.xMin + xPivotWidth, rect.rect.yMin + yPivotHeight);
            rectPos.TopRight = new Vector2(rect.rect.xMax + xPivotWidth, rect.rect.yMax + yPivotHeight);

            //���� ��ũ�� ����� �µ��� �����ϴ� ������ �ʿ��ϴ�.

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
