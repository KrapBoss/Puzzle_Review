using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 배경 색이 바뀌는 동작을 수행하기 위함
/// </summary>
public class NightSlider : MonoBehaviour, IOnOff
{
    public Slider slider;

    public bool Active { get; set; }

    public bool Off()
    {
        Active = false;
        SetNight(false);
        return Active;
    }

    public bool On()
    {
        Active = true;
        SetNight(true);
        return Active;
    }
    void SetNight(bool night)
    {
        if (night)
        {
            StartCoroutine(SetNightCroutine(1));
        }
        else
        {
            StartCoroutine(SetNightCroutine(0));
        }
    }

    IEnumerator SetNightCroutine(float target)
    {
        if(slider != null)
        {
            float value = slider.value;

            float t = 0;

            while (t < 1.0f)
            {
                t += Time.deltaTime * 2.0f;
                value = Mathf.Lerp(value, target, t);
                slider.value = value;

                yield return null;
            }
        }

        EventManager.Instance.action_SetNight(target != 0.0f);
    }
}
