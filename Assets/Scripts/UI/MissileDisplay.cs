using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    static Text number;
    static Image coolDown;
    void Awake()
    {
        number = transform.Find("Number").GetComponent<Text>();
        coolDown = transform.Find("CoolDown").GetComponent<Image>();
    }
    public static void UpdateMissileCount(int count)
    {
        number.text = count.ToString();
    }

    public static void UpdateMissileCoolDown(float fillAmount) => coolDown.fillAmount = fillAmount;
}
