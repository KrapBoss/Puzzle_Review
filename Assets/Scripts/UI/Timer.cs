using System;
using TMPro;
using UnityEngine;

/// <summary>
/// �ð��� ǥ���մϴ�.
/// </summary>
public class TimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    private void Awake()
    {
        text.text = "";
        text.color = Color.yellow;
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }

    ///�ʴ��� �ð��� �ִ� Hour ������ ��ȯ���� ����
    public void SetTime(float timeBySec, bool transition = true)
    {
        if (!transition) { text.text = $"{timeBySec: 0.#}"; }
        //��ȯ
        else
        {
            float totalSeconds = timeBySec; // ��ȯ�� ��
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);

            // ��:��:�� �������� ���
            string formattedTime = string.Format("{0:D2}:{1:D2}",
                timeSpan.Minutes,
                timeSpan.Seconds);

            text.text = formattedTime; // ���: 01:01:01
        }
    }
}
