using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI TitleLabel;
    public TextMeshProUGUI EnemyKilledLabel;
    public TextMeshProUGUI TimeLeftdLabel;
    // Start is called before the first frame update
    void Start()
    {

        Cursor.visible = true; // 커서 보이게
        Cursor.lockState = CursorLockMode.None; // 화면 안에 가둔거 풀기

        int EnemyLeft = GameManager.instance.EnemyLeft; // GameManager 의 EnemyLeft 정보를 가져옴
        int EnemyKilled = GameManager.instance.EnemyKilled;
        float TimeLeft = GameManager.instance.TimeLeft;

        if (EnemyLeft <= 0)
        {
            TitleLabel.text = "Clear";
        }
        else
        {
            TitleLabel.text = "Game Over";
        }

        EnemyKilledLabel.text = "Enemy Killed : " + (EnemyKilled);
        TimeLeftdLabel.text = "Time Left : " + TimeLeft.ToString("#.##");

        Destroy(GameManager.instance.gameObject); // 여기서 안부숴놓으면 다시 시작할때 게임매니저가 중첩됨


    }

    public void PlayAgainPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitPressed()
    {
        Application.Quit();
    }

}
