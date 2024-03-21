using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Settings : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button replayButton;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject Menu;
    private bool isSettingsOpen = false;
    public static Settings instance;
    private void Awake()
    {
        instance = this;
    }
    public void OnClick()
    {
        SFX.instance.ButtonClick();
        isSettingsOpen = !isSettingsOpen;
        if (isSettingsOpen)
        {
            Time.timeScale = 0;
            Menu.SetActive(true);

        }
        else
        {
            Menu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void GameOverNotify()
    {
        gameOverPanel.SetActive(true);
    }

    public void OnClickReplay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
