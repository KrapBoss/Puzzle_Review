using System;
using TMPro;
using UnityEngine;

/// <summary>
/// 시간을 표시합니다.
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

    ///초단위 시간과 최대 Hour 단위로 변환할지 선택
    public void SetTime(float timeBySec, bool transition = true)
    {
        if (!transition) { text.text = $"{timeBySec: 0.#}"; }
        //변환
        else
        {
            float totalSeconds = timeBySec; // 변환할 초
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);

            // 시:분:초 형식으로 출력
            string formattedTime = string.Format("{0:D2}:{1:D2}",
                timeSpan.Minutes,
                timeSpan.Seconds);

            text.text = formattedTime; // 출력: 01:01:01
        }
    }
}
