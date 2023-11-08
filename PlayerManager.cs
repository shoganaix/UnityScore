using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public Leaderboard leaderboard;
    public SimpleGame simpleGame;
    public TMP_InputField playerNameInputfield;
    public GameObject playerNameInput;
    public GameObject Panel;
    public GameObject BackgroundUI;
    public GameObject BackgroundUITitle;
    public GameObject Title;
    public GameObject Character;
    public GameObject Typesound;
    public Button PlayButton;
    public Button ExitButton;
    public Button MuteButton;

    private bool titleActive = true;
    private bool backgroundActive = true;

    void Start()
    {
        StartCoroutine(SetupRoutine());
    }

    void Update()
    {
        if (titleActive && backgroundActive && Input.anyKeyDown)
        {
            titleActive = false;
            backgroundActive = false;
            Title.SetActive(false);
            BackgroundUITitle.SetActive(false);
            BackgroundUI.SetActive(true);
            PlayButton.gameObject.SetActive(true);
            ExitButton.gameObject.SetActive(true);
            MuteButton.gameObject.SetActive(true);
        }
    }

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerNameInputfield.text, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name");
                Panel.SetActive(true);
                BackgroundUI.SetActive(true);
                playerNameInput.SetActive(false);
                Character.SetActive(false);
                Typesound.SetActive(false);
                simpleGame.gameObject.SetActive(true);
                simpleGame.StartGame();
            }
            else
            {
                Debug.Log("Could not set player name" + response.Error);
            }
        });
    }

    IEnumerator SetupRoutine()
    {
        yield return StartCoroutine(LoginRoutine());
        yield return StartCoroutine(leaderboard.FetchTopHighscoresRoutine());
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not log in");
                done = true;
            }
        });

        yield return new WaitWhile(() => !done);
    }

    public void OnPlayButtonClicked()
    {
        playerNameInput.SetActive(true);
        Character.SetActive(true);
        Panel.SetActive(false);
        Typesound.SetActive(true);


        PlayButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        MuteButton.gameObject.SetActive(false);
        
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
