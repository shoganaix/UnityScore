using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    int leaderboardID = 14902;
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

   public IEnumerator SubmitScoreRoutine(float scoreToUpload)
{
    bool done = false;
    string playerID = PlayerPrefs.GetString("PlayerID");
    //works fine Debug.Log("RightPlayer ID: " + playerID);

    //works fine Debug.Log("RighACalling GetPlayerScoreRoutine");
    yield return StartCoroutine(GetPlayerScoreRoutine(playerID));

    int currentScore = PlayerPrefs.GetInt("PlayerScore");

    int roundedScore = Mathf.RoundToInt(scoreToUpload);

    if (roundedScore > currentScore)
    {
        LootLockerSDKManager.SubmitScore(playerID, roundedScore, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                PlayerPrefs.SetInt("PlayerScore", roundedScore);
                //works fine Debug.Log("Right PlayerScore: " + PlayerPrefs.GetInt("PlayerScore"));
            }
            else
            {
                Debug.Log("Failed to upload score: " + response.Error);
            }
            done = true;
        });
    }
    else
    {
        Debug.Log("Score is not higher than the current score");
        done = true;
    }

    yield return new WaitWhile(() => done == false);
    yield return StartCoroutine(FetchTopHighscoresRoutine());
    //right score Debug.Log("Right score" + roundedScore);

}

    private IEnumerator GetPlayerScoreRoutine(string playerID)
    {
        bool done = false;

        LootLockerSDKManager.GetScoreList(leaderboardID, 1, 0, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] members = response.items;
                if (members.Length > 0)
                {
                    int playerScore = members[0].score;
                    PlayerPrefs.SetInt("PlayerScore", playerScore);
                    //wrong score Debug.Log("Wrong score" + playerScore);

                }
                else
                {
                    PlayerPrefs.SetInt("PlayerScore", 0);
                }
            }
            else
            {
                Debug.Log("Failed to get player score: " + response.Error);
            }
            done = true;
        });

        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        //right Debug.Log("RightCalling GetPlayerScoreRoutine");
        yield return StartCoroutine(GetPlayerScoreRoutine(playerID));

        if (!string.IsNullOrEmpty(playerID))
        {
            LootLockerSDKManager.GetScoreList(leaderboardID, 10, 0, (response) =>
            {
                if (response.success)
                {
                    string tempPlayerNames = "Names\n";
                    string tempPlayerScores = "Scores\n";

                    LootLockerLeaderboardMember[] members = response.items;

                    for (int i = 0; i < members.Length; i++)
                    {
                        tempPlayerNames += members[i].rank + ". ";
                        if (members[i].player.name != "")
                        {
                            tempPlayerNames += members[i].player.name;
                        }
                        else
                        {
                            tempPlayerNames += members[i].player.id;
                        }
                        tempPlayerNames += "\n";
                        tempPlayerScores += members[i].score + "\n";
                    }

                    done = true;
                    playerNames.text = tempPlayerNames;
                    playerScores.text = tempPlayerScores;
                    //wrong Debug.Log("Wrong tempPlayerScores: " + tempPlayerScores);
                }
                else
                {
                    Debug.Log("Failed: " + response.Error);
                    done = true;
                }
            });
        }

        yield return new WaitWhile(() => done == false);
    }
}