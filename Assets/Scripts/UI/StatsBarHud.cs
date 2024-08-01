using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBarHud : StatsBar
{
    [SerializeField] protected Text percentText;

    protected virtual void SetPercentText()
    {
        //percentText.text = (Mathf.RoundToInt(targetFillAmount * 100f)).ToString() + "%";
        percentText.text = targetFillAmount.ToString("P0");
    }

    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercentText();
    }
    protected override IEnumerator BufferedFillingCoroutine(Image image)
    {
        SetPercentText();
        return base.BufferedFillingCoroutine(image);
    }
}
