using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowJudge : MonoBehaviour
{
    public ArrowSpawner.ArrowDirection judgeDirection; // 이 판정 화살표의 방향
    public float greatRange = 0.5f;                    // Great 판정 범위
    public float goodRange = 1f;
    public TextMeshProUGUI scoreText;

  

    private int score = 0;


    private void Update()
    {
        // 사용자가 방향에 맞는 키를 누르면 판정
        if (Input.GetKeyDown(GetKeyForDirection(judgeDirection)))
        {
            CheckArrow();
        }
    }

    void CheckArrow()
    {
        // 모든 화살표 검색
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject arrow in arrows)
        {
            Arrow arrowScript = arrow.GetComponent<Arrow>();

            // 방향이 동일하고, 범위 내에 있는 경우 판정
            if (arrowScript.direction == judgeDirection)
            {
                float distance = Vector3.Distance(arrow.transform.position, transform.position);

                // 판정 범위 내에 있을 경우
                if (distance <= greatRange)
                {
                    HandleJudgement("Great", 20); // Great 판정
                }
                else if (distance <= goodRange)
                {
                    HandleJudgement("Good", 10); // Good 판정
                }
                else if (distance <= goodRange + 0.5f) // 추가 범위로 Bad 판정
                {
                    HandleJudgement("Bad", 0); // Bad 판정
                }
                else
                {
                    continue; // 범위를 벗어난 경우 무시
                }

                Destroy(arrow); // 화살표 제거
                //return;         // 가장 가까운 화살표 하나만 처리
            }
        }
    }

    void HandleJudgement(string judgement, int scoreIncrease)
    {
        Debug.Log($"Judgement: {judgement}");

        Debug.Log($"Direaction: {judgeDirection}");


        // Great 또는 Good 판정일 때만 소리 재생
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
            Debug.LogError("ScoreText가 연결되지 않았습니다!");
            return;
        }

        scoreText.text = "Score: " + score;
        Debug.Log($"Score 업데이트: {scoreText.text}, Position: {scoreText.rectTransform.position}");
    }

    // 방향에 따른 키를 반환
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
