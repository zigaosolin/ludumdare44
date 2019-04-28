using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] Player player;
    [SerializeField] Shop shop;
    [SerializeField] GameObject diedDialog;
    [SerializeField] TextMeshProUGUI diedOrFinishedLabel;
    [SerializeField] GameObject startDialog;
    [SerializeField] float gameStart;
    [SerializeField] float gameEnd;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip confirm;

    private void Start()
    {
        player.transform.position = new Vector3(0, gameStart, 0);
        cameraMovement.transform.position = new Vector3(0, gameStart, -10);
        SetPaused(true);
        audioSource.PlayOneShot(confirm);

    }

    private void Update()
    {
        if(cameraMovement.transform.position.y > gameEnd)
        {
            diedOrFinishedLabel.text = "Game Completed";
            Died();
        }        
    }

    public void TriggerShop()
    {
        audioSource.PlayOneShot(confirm);
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
        audioSource.PlayOneShot(confirm);
        SceneManager.LoadScene(0);
    }

    public void ExitButton()
    {
        audioSource.PlayOneShot(confirm);
        Application.Quit();
    }

    public void StartDialogButton()
    {
        audioSource.PlayOneShot(confirm);
        SetPaused(false);
        startDialog.SetActive(false);
    }
}
