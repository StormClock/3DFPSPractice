using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Scene �̵��� �ʿ�

public class GameManager : MonoBehaviour
{

    public static GameManager instance; // �ٸ��ֵ��� �� �޼ҵ� ������ �ʿ�

    private void Awake()
    {
        instance = this; // �ٸ��ֵ��� �� �޼ҵ� ������ �ʿ�

    }

    public int EnemyLeft = 10;
    public int EnemyKilled = 0;
    public float TimeLeft = 80;

    bool IsPlaying;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject); //�ٸ� ������ ���� �츮�� �ڵ�

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
