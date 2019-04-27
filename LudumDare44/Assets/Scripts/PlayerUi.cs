using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    [SerializeField]
    private Slider hitPoints;

    public void SetHitPoint(float current, float max)
    {
        hitPoints.value = current / max;
    }
}
