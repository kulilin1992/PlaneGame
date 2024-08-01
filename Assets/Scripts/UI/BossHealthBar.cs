using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : StatsBarHud
{
    protected override void SetPercentText()
    {
        //base.SetPercentText();
        //method1
        //percentText.text = string.Format("{0:N2}%", targetFillAmount * 100f) + "%";
        //method2
        //percentText.text = (targetFillAmount * 100f).ToString("f2") + "%";
        //method3
        percentText.text = targetFillAmount.ToString("P");

    }
}
