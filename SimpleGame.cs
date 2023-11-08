using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGame : MonoBehaviour
{
    public Leaderboard leaderboard;
    public int gamescore;
    public ParticleSystem ParticleClick;
    private bool gameStarted = false;


    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = ParticleClick.GetComponent<ParticleSystem>();
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    void Update()
    {
        if (!gameStarted)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            gamescore += 1;
            StartCoroutine(GameOverRoutine(gamescore));

            Vector3 clickPosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(clickPosition);
            worldPosition.z = 0f;

            particleSystem.transform.position = worldPosition;
            particleSystem.Play(true);
        }
    }

    IEnumerator GameOverRoutine(int score)
    {
        yield return StartCoroutine(leaderboard.SubmitScoreRoutine(score)); 
    }
}