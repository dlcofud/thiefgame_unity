using UnityEngine;

//수정없음
public class CharacterMovement : MonoBehaviour
{
    private Vector2 outerSize;    // 외부 사각형 크기
    private Vector2 innerSize;    // 내부 사각형 크기
    private Vector2 moveDirection; // 이동 방향 (단위 벡터)
    private float moveSpeed = 2f; // 이동 속도

    private bool isPaused = false;      // 멈춤 상태 여부
    private float pauseTimer = 0f;      // 멈춤 타이머
    private float pauseDuration = 0f;   // 멈추는 시간

    private float pauseProbability = 0.05f; // 각 프레임에서 멈출 확률 (0 ~ 1)
    private float minPauseDuration = 1f; // 최소 멈춤 시간
    private float maxPauseDuration = 3f; // 최대 멈춤 시간

    private void Start()
    {
        SetRandomDirection(); // 랜덤 초기 방향 설정
    }

    private void Update()
    {
        if (isPaused)
        {
            HandlePause(); // 멈춤 상태 처리
        }
        else
        {
            MoveCharacter();
            CheckBoundsAndReflect();
            HandleRandomPause(); // 무작위로 멈추기 처리
        }
    }



    // 경계 설정 함수
    public void SetPathBounds(Vector2 outer, Vector2 inner)
    {
        outerSize = outer;
        innerSize = inner;
    }

    // 캐릭터 이동 함수
    //public으로 수정한 함수
    // MoveCharacter를 public으로 변경하여 다른 스크립트에서 호출 가능하도록 합니다.
    private void MoveCharacter()
    {
        // 이동 처리 로직
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }

    

    // 경계를 확인하고 캐릭터를 되돌리고 반사시키는 함수
    private void CheckBoundsAndReflect()
    {
        Vector2 pos = transform.position;
        bool directionChanged = false;

        // 외부 사각형 경계 확인 및 이동 제한
        if (pos.x < -outerSize.x / 2)
        {
            pos.x = -outerSize.x / 2;
            moveDirection.x *= -1;
            directionChanged = true;
        }
        if (pos.x > outerSize.x / 2)
        {
            pos.x = outerSize.x / 2;
            moveDirection.x *= -1;
            directionChanged = true;
        }
        if (pos.y < -outerSize.y / 2)
        {
            pos.y = -outerSize.y / 2;
            moveDirection.y *= -1;
            directionChanged = true;
        }
        if (pos.y > outerSize.y / 2)
        {
            pos.y = outerSize.y / 2;
            moveDirection.y *= -1;
            directionChanged = true;
        }

        // 내부 사각형 경계 확인 및 위치 조정
        if (Mathf.Abs(pos.x) < innerSize.x / 2 && Mathf.Abs(pos.y) < innerSize.y / 2)
        {
            if (Mathf.Abs(pos.x) >= innerSize.x / 2 - 0.1f)
            {
                pos.x = Mathf.Sign(pos.x) * (innerSize.x / 2);
                moveDirection.x *= -1;
                directionChanged = true;
            }
            if (Mathf.Abs(pos.y) >= innerSize.y / 2 - 0.1f)
            {
                pos.y = Mathf.Sign(pos.y) * (innerSize.y / 2);
                moveDirection.y *= -1;
                directionChanged = true;
            }
        }

        if (directionChanged)
        {
            transform.position = pos;
        }
    }

    // 랜덤한 초기 방향 설정 함수
    private void SetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
    }

    // 무작위로 멈추기 처리 함수
    private void HandleRandomPause()
    {
        if (Random.value < pauseProbability) // 멈출 확률 (매 프레임마다)
        {
            StartPause(); // 멈추기 시작
        }
    }

    // 멈춤 상태 시작 함수
    private void StartPause()
    {
        if (!isPaused) // 이미 멈춰 있지 않으면
        {
            isPaused = true;
            pauseDuration = Random.Range(minPauseDuration, maxPauseDuration); // 랜덤으로 멈추는 시간 설정
            pauseTimer = pauseDuration; // 타이머 설정
        }
    }

    // 멈춤 상태 처리 함수
    private void HandlePause()
    {
        pauseTimer -= Time.deltaTime; // 멈춤 타이머 감소
        if (pauseTimer <= 0f)
        {
            isPaused = false; // 멈춤 해제
        }
    }
}
