using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 outerSize;   // 외부 사각형 크기
    private Vector2 innerSize;   // 내부 사각형 크기
    private Vector2 moveDirection; // 이동 방향 (단위 벡터)
    private float moveSpeed = 2f;  // 이동 속도

    private float horizontalInput;  // 수평 입력
    private float verticalInput;    // 수직 입력

    private Animator animator; // Animator 컴포넌트 참조

    private void Start()
    {
        // 초기 방향 설정 (입력에 따라 설정되지 않음)
        moveDirection = Vector2.zero;
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }

    private void Update()
    {
        // 사용자 입력 받기
        horizontalInput = Input.GetAxis("Horizontal");  // A/D 또는 좌/우 화살표
        verticalInput = Input.GetAxis("Vertical");      // W/S 또는 상/하 화살표

        // 이동 방향 설정
        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // 이동 처리
        MoveCharacter();

        // 경계를 확인하고 이동 제한
        CheckBoundsAndReflect();

        // 애니메이션 처리
        HandleAnimations();
    }

    // 경계 설정 함수
    public void SetBounds(Vector2 outer, Vector2 inner)
    {
        outerSize = outer;
        innerSize = inner;
    }

    // 캐릭터 이동 함수
    private void MoveCharacter()
    {
        // 이동 처리
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
            directionChanged = true;
        }
        if (pos.x > outerSize.x / 2)
        {
            pos.x = outerSize.x / 2;
            directionChanged = true;
        }
        if (pos.y < -outerSize.y / 2)
        {
            pos.y = -outerSize.y / 2;
            directionChanged = true;
        }
        if (pos.y > outerSize.y / 2)
        {
            pos.y = outerSize.y / 2;
            directionChanged = true;
        }

        // 내부 사각형 경계 확인 및 이동 제한
        if (Mathf.Abs(pos.x) < innerSize.x / 2 && Mathf.Abs(pos.y) < innerSize.y / 2)
        {
            if (Mathf.Abs(pos.x) >= innerSize.x / 2 - 0.1f)
            {
                pos.x = Mathf.Sign(pos.x) * (innerSize.x / 2);
                directionChanged = true;
            }
            if (Mathf.Abs(pos.y) >= innerSize.y / 2 - 0.1f)
            {
                pos.y = Mathf.Sign(pos.y) * (innerSize.y / 2);
                directionChanged = true;
            }
        }

        // 위치가 변경되었으면 적용
        if (directionChanged)
        {
            transform.position = pos;
        }
    }

    // 애니메이션 제어 함수
    private void HandleAnimations()
    {
        // 이동 여부에 따른 애니메이션 전환
        bool isMoving = moveDirection.sqrMagnitude > 0.01f;  // 이동 중이면 true

        // 이동 애니메이션 처리
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            // 이동 방향에 따라 애니메이션 변경
            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                if (horizontalInput > 0)
                {
                    animator.SetInteger("moveDirection", 1); // 오른쪽
                }
                else
                {
                    animator.SetInteger("moveDirection", -1); // 왼쪽
                }
            }
            else
            {
                if (verticalInput > 0)
                {
                    animator.SetInteger("moveDirection", 2); // 위쪽
                }
                else
                {
                    animator.SetInteger("moveDirection", -2); // 아래쪽
                }
            }
        }
    }
}
