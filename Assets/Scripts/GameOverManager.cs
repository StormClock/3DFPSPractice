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

        Cursor.visible = true; // Ŀ�� ���̰�
        Cursor.lockState = CursorLockMode.None; // ȭ�� �ȿ� ���а� Ǯ��

        int EnemyLeft = GameManager.instance.EnemyLeft; // GameManager �� EnemyLeft ������ ������
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

        Destroy(GameManager.instance.gameObject); // ���⼭ �Ⱥν������� �ٽ� �����Ҷ� ���ӸŴ����� ��ø��


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
