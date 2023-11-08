using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Leaderboard leaderboard;
    public SimpleGame simpleGame;
    
    IEnumerator DieRoutine()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(leaderboard.SubmitScoreRoutine((int)simpleGame.gamescore));
        Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}