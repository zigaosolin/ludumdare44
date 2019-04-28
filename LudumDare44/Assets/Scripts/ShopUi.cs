using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpLabel;
    [SerializeField] private TextMeshProUGUI upgrade1Label;
    [SerializeField] private TextMeshProUGUI upgrade2Label;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip confirm;

    Action<UpgradeInfo> boughtCallback;
    Action closeCallback;
    UpgradeInfo upgradeLeft;
    UpgradeInfo upgradeRight;

    public void Show(int availableHp, UpgradeInfo upgrade1, UpgradeInfo upgrade2, 
        Action<UpgradeInfo> boughtCallback, Action closeCallback)
    {
        this.closeCallback = closeCallback;
        this.boughtCallback = boughtCallback;

        upgradeLeft = upgrade1;
        upgradeRight = upgrade2;

        gameObject.SetActive(true);

        hpLabel.text = $"{availableHp} Life Available";
        SetUpgrade(upgrade1Label, upgrade1, availableHp);
        SetUpgrade(upgrade2Label, upgrade2, availableHp);
    }

    private void SetUpgrade(TextMeshProUGUI label, UpgradeInfo upgrade, int availableHp)
    {
        var button = label.transform.parent.GetComponent<Button>();
        if(upgrade == null)
        {
            button.gameObject.SetActive(false);
            return;
        }

        button.gameObject.SetActive(true);

        label.text =
            $"+{upgrade.UpgradePercent}% {upgrade.Type}\n" +
            $"Level: {upgrade.UpgradeLevel}/5\n" +
            $"Cost: {upgrade.HPCost} Life";
        
        button.interactable = availableHp > upgrade.HPCost;
    }

    public void BuyLeftClicked()
    {
        audioSource.PlayOneShot(confirm);
        boughtCallback?.Invoke(upgradeLeft);
    }

    public void BuyRightClicked()
    {
        audioSource.PlayOneShot(confirm);
        boughtCallback?.Invoke(upgradeRight);
    }

    public void DoneClocked()
    {
        audioSource.PlayOneShot(confirm);
        closeCallback?.Invoke();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
