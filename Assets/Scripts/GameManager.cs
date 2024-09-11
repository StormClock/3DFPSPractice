using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Scene 이동에 필요

public class GameManager : MonoBehaviour
{

    public static GameManager instance; // 다른애들이 얘 메소드 쓸려면 필요

    private void Awake()
    {
        instance = this; // 다른애들이 얘 메소드 쓸려면 필요

    }

    public int EnemyLeft = 10;
    public int EnemyKilled = 0;
    public float TimeLeft = 80;

    bool IsPlaying;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject); //다른 씬으로 가도 살리는 코드

    }

    // Update is called once per frame
    void Update()
    {
        IsPlaying = true;
        if (IsPlaying)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft < 0)
            {
                GameOverScene();
            }
        }

    }

    public void EnemyCount()
    {
        EnemyLeft--;
        EnemyKilled++;
        {
            if (EnemyLeft <= 0)
            {
                GameOverScene();
            }
        }
    }

    public void GameOverScene()
    {
        IsPlaying = false;
        SceneManager.LoadScene("GameOverScene");
    }
}
