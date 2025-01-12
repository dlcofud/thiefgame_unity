using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowJudge : MonoBehaviour
{
    public ArrowSpawner.ArrowDirection judgeDirection; // �� ���� ȭ��ǥ�� ����
    public float greatRange = 0.5f;                    // Great ���� ����
    public float goodRange = 1f;
    public TextMeshProUGUI scoreText;

  

    private int score = 0;


    private void Update()
    {
        // ����ڰ� ���⿡ �´� Ű�� ������ ����
        if (Input.GetKeyDown(GetKeyForDirection(judgeDirection)))
        {
            CheckArrow();
        }
    }

    void CheckArrow()
    {
        // ��� ȭ��ǥ �˻�
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject arrow in arrows)
        {
            Arrow arrowScript = arrow.GetComponent<Arrow>();

            // ������ �����ϰ�, ���� ���� �ִ� ��� ����
            if (arrowScript.direction == judgeDirection)
            {
                float distance = Vector3.Distance(arrow.transform.position, transform.position);

                // ���� ���� ���� ���� ���
                if (distance <= greatRange)
                {
                    HandleJudgement("Great", 20); // Great ����
                }
                else if (distance <= goodRange)
                {
                    HandleJudgement("Good", 10); // Good ����
                }
                else if (distance <= goodRange + 0.5f) // �߰� ������ Bad ����
                {
                    HandleJudgement("Bad", 0); // Bad ����
                }
                else
                {
                    continue; // ������ ��� ��� ����
                }

                Destroy(arrow); // ȭ��ǥ ����
                //return;         // ���� ����� ȭ��ǥ �ϳ��� ó��
            }
        }
    }

    void HandleJudgement(string judgement, int scoreIncrease)
    {
        Debug.Log($"Judgement: {judgement}");

        Debug.Log($"Direaction: {judgeDirection}");


        // Great �Ǵ� Good ������ ���� �Ҹ� ���
        if (judgement == "Great" || judgement == "Good")
        {
            switch (judgeDirection)
            {
                case ArrowSpawner.ArrowDirection.Up:
                    AudioManager.instance.PlaySound(AudioManager.instance.upArrowClip);
                    break;
                case ArrowSpawner.ArrowDirection.Down:
                    AudioManager.instance.PlaySound(AudioManager.instance.downArrowClip);
                    break;
                case ArrowSpawner.ArrowDirection.Left:
                    AudioManager.instance.PlaySound(AudioManager.instance.leftArrowClip);
                    break;
                case ArrowSpawner.ArrowDirection.Right:
                    AudioManager.instance.PlaySound(AudioManager.instance.rightArrowClip);
                    break;
            }
        }

        ComboManager.Instance.IncreaseCombo();
        ScoreManager.Instance.IncreaseScore(scoreIncrease);


        //UpdateScore();
    }


    void UpdateScore()
    {
        if (scoreText == null)
        {
            Debug.LogError("ScoreText�� ������� �ʾҽ��ϴ�!");
            return;
        }

        scoreText.text = "Score: " + score;
        Debug.Log($"Score ������Ʈ: {scoreText.text}, Position: {scoreText.rectTransform.position}");
    }

    // ���⿡ ���� Ű�� ��ȯ
    KeyCode GetKeyForDirection(ArrowSpawner.ArrowDirection direction)
    {
        switch (direction)
        {
            case ArrowSpawner.ArrowDirection.Up:
                return KeyCode.UpArrow;
            case ArrowSpawner.ArrowDirection.Down:
                return KeyCode.DownArrow;
            case ArrowSpawner.ArrowDirection.Left:
                return KeyCode.LeftArrow;
            case ArrowSpawner.ArrowDirection.Right:
                return KeyCode.RightArrow;
            default:
                return KeyCode.None;
        }
    }
}
