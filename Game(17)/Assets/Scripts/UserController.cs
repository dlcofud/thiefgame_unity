using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 outerSize;   // �ܺ� �簢�� ũ��
    private Vector2 innerSize;   // ���� �簢�� ũ��
    private Vector2 moveDirection; // �̵� ���� (���� ����)
    private float moveSpeed = 2f;  // �̵� �ӵ�

    private float horizontalInput;  // ���� �Է�
    private float verticalInput;    // ���� �Է�

    private Animator animator; // Animator ������Ʈ ����

    private void Start()
    {
        // �ʱ� ���� ���� (�Է¿� ���� �������� ����)
        moveDirection = Vector2.zero;
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
    }

    private void Update()
    {
        // ����� �Է� �ޱ�
        horizontalInput = Input.GetAxis("Horizontal");  // A/D �Ǵ� ��/�� ȭ��ǥ
        verticalInput = Input.GetAxis("Vertical");      // W/S �Ǵ� ��/�� ȭ��ǥ

        // �̵� ���� ����
        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // �̵� ó��
        MoveCharacter();

        // ��踦 Ȯ���ϰ� �̵� ����
        CheckBoundsAndReflect();

        // �ִϸ��̼� ó��
        HandleAnimations();
    }

    // ��� ���� �Լ�
    public void SetBounds(Vector2 outer, Vector2 inner)
    {
        outerSize = outer;
        innerSize = inner;
    }

    // ĳ���� �̵� �Լ�
    private void MoveCharacter()
    {
        // �̵� ó��
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }

    // ��踦 Ȯ���ϰ� ĳ���͸� �ǵ����� �ݻ��Ű�� �Լ�
    private void CheckBoundsAndReflect()
    {
        Vector2 pos = transform.position;
        bool directionChanged = false;

        // �ܺ� �簢�� ��� Ȯ�� �� �̵� ����
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

        // ���� �簢�� ��� Ȯ�� �� �̵� ����
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

        // ��ġ�� ����Ǿ����� ����
        if (directionChanged)
        {
            transform.position = pos;
        }
    }

    // �ִϸ��̼� ���� �Լ�
    private void HandleAnimations()
    {
        // �̵� ���ο� ���� �ִϸ��̼� ��ȯ
        bool isMoving = moveDirection.sqrMagnitude > 0.01f;  // �̵� ���̸� true

        // �̵� �ִϸ��̼� ó��
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            // �̵� ���⿡ ���� �ִϸ��̼� ����
            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                if (horizontalInput > 0)
                {
                    animator.SetInteger("moveDirection", 1); // ������
                }
                else
                {
                    animator.SetInteger("moveDirection", -1); // ����
                }
            }
            else
            {
                if (verticalInput > 0)
                {
                    animator.SetInteger("moveDirection", 2); // ����
                }
                else
                {
                    animator.SetInteger("moveDirection", -2); // �Ʒ���
                }
            }
        }
    }
}
