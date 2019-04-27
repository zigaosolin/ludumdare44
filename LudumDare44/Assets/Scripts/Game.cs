using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] Player player;
    [SerializeField] Shop shop;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TriggerShop();
        }
    }

    public void TriggerShop()
    {
        SetPaused(true);
        shop.ShowShop(() => SetPaused(false));
    }

    private void SetPaused(bool pause)
    {
        cameraMovement.enabled = !pause;
        player.enabled = !pause;
    }
}
