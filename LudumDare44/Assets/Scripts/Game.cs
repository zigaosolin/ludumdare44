using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] Player player;
    [SerializeField] Shop shop;
    [SerializeField] GameObject diedDialog;
    [SerializeField] GameObject startDialog;
    [SerializeField] float gameStart;

    private void Start()
    {
        player.transform.position = new Vector3(0, gameStart, 0);
        cameraMovement.transform.position = new Vector3(0, gameStart, -10);
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

    public void Died()
    {
        SetPaused(true);
        diedDialog.SetActive(true);
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void StartDialogButton()
    {
        SetPaused(false);
        startDialog.SetActive(false);
    }
}
