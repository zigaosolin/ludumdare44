using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Speed,
    Jump
}

public class UpgradeInfo
{
    public UpgradeType Type;
    public int UpgradeLevel;
    public int UpgradePercent;
    public int HPCost;
}

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopUi shopUi;
    [SerializeField] private Player player;

    private List<UpgradeInfo> speedUpgrades = new List<UpgradeInfo>()
    {
        new UpgradeInfo() {
            Type = UpgradeType.Speed,
            UpgradeLevel = 1,
            UpgradePercent = 20,
            HPCost = 20
        },
        new UpgradeInfo() {
            Type = UpgradeType.Speed,
            UpgradeLevel = 2,
            UpgradePercent = 40,
            HPCost = 30
        },
        new UpgradeInfo() {
            Type = UpgradeType.Speed,
            UpgradeLevel = 3,
            UpgradePercent = 60,
            HPCost = 35
        },
        new UpgradeInfo() {
            Type = UpgradeType.Speed,
            UpgradeLevel = 4,
            UpgradePercent = 80,
            HPCost = 40
        },
        new UpgradeInfo() {
            Type = UpgradeType.Speed,
            UpgradeLevel = 5,
            UpgradePercent = 100,
            HPCost = 50
        },
        null
    };

    private List<UpgradeInfo> jumpUpgrades = new List<UpgradeInfo>()
    {
        new UpgradeInfo() {
            Type = UpgradeType.Jump,
            UpgradeLevel = 1,
            UpgradePercent = 20,
            HPCost = 20
        },
        new UpgradeInfo() {
            Type = UpgradeType.Jump,
            UpgradeLevel = 2,
            UpgradePercent = 40,
            HPCost = 30
        },
        new UpgradeInfo() {
            Type = UpgradeType.Jump,
            UpgradeLevel = 3,
            UpgradePercent = 60,
            HPCost = 35
        },
        new UpgradeInfo() {
            Type = UpgradeType.Jump,
            UpgradeLevel = 4,
            UpgradePercent = 80,
            HPCost = 40
        },
        new UpgradeInfo() {
            Type = UpgradeType.Jump,
            UpgradeLevel = 5,
            UpgradePercent = 100,
            HPCost = 50
        },
        null
    };

    private int currentSpeed = 0;
    private int currentJump = 0;

    public void ShowShop(Action onCompleted)
    {
        shopUi.Show((int)player.HitPoints, speedUpgrades[currentSpeed], jumpUpgrades[currentJump],
            (x) =>
            {
                if (x.Type == UpgradeType.Speed)
                {
                    ++currentSpeed;
                }
                else
                {
                    ++currentJump;
                }
                ShowShop(onCompleted);
            },
            () =>
            {
                shopUi.Hide();
                onCompleted?.Invoke();
            }
         );
    }
}
