using System;
using System.Collections;
using UnityEngine;

//���� ����(player controller)
public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs;  // ĳ���� ������ ���
    public float[] spawnProbabilities;     // ���� Ȯ�� (���� 1�̾�� ��)
    public Vector2 outerSize;              // ū �׸��� ũ��
    public Vector2 innerSize;              // ���� �׸��� ũ��
    public int numberOfCharacters = 5;     // ������ ĳ���� ��

    // ���� ĳ���Ͱ� ������ �� �̺�Ʈ (����)
    public static event Action<GameObject> OnSubCharacterSpawned;

    private void Start()
    {
        // User ĳ����(Player �±�) ����
        SetPlayerBounds();

        SpawnCharacters();
    }

    // User ĳ������ ��谪 ����
    private void SetPlayerBounds()
    {
        GameObject userCharacter = GameObject.FindWithTag("Player");
        if (userCharacter != null)
        {
            PlayerController playerController = userCharacter.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetBounds(outerSize, innerSize);
            }
            else
            {
                Debug.LogError("PlayerController script is not attached to the User character.");
            }
        }
        else
        {
            Debug.LogError("No GameObject with the 'Player' tag found.");
        }
    }

    // ĳ���� ���� �Լ�
    private void SpawnCharacters()
    {
        for (int i = 0; i < numberOfCharacters; i++)
        {
            // Ȯ���� ���� �������� ĳ���͸� ����
            int randomIndex = GetRandomIndexBasedOnProbability();
            GameObject character = Instantiate(characterPrefabs[randomIndex]);

            // ĳ������ ���� ��ġ�� ���
            Vector3 spawnPosition = GetRandomPositionOnPath();
            character.transform.position = spawnPosition;


            // CharacterMovement ��ũ��Ʈ ����
            CharacterMovement movement = character.GetComponent<CharacterMovement>();
            if (movement != null)
            {
                movement.SetPathBounds(outerSize, innerSize);
            }
            else
            {
                Debug.LogError("CharacterMovement script is not attached to the character prefab.");
            }

            // ���� ĳ���� ���� �̺�Ʈ ȣ��(����)
            OnSubCharacterSpawned?.Invoke(character);
        }
    }

    // Ȯ���� ���� ĳ���� �ε����� ��ȯ�ϴ� �Լ�
    private int GetRandomIndexBasedOnProbability()
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        float cumulativeProbability = 0f;

        for (int i = 0; i < spawnProbabilities.Length; i++)
        {
            cumulativeProbability += spawnProbabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return i;
            }
        }

        return spawnProbabilities.Length - 1; // �⺻������ ������ �ε����� ��ȯ
    }

    // ��� ���� ������ ��ġ�� ����ϴ� �Լ�
    private Vector2 GetRandomPositionOnPath()
    {
        float x, y;
        int area = UnityEngine.Random.Range(0, 4);
        switch(area)
        {
            case 0:
                x = UnityEngine.Random.Range(-outerSize.x / 2, outerSize.x / 2 );
                y = UnityEngine.Random.Range(innerSize.y / 2, outerSize.y / 2 );
                break;
            case 1:
                x = UnityEngine.Random.Range(-outerSize.x / 2, outerSize.x / 2);
                y = UnityEngine.Random.Range(-outerSize.y / 2, -innerSize.y / 2);
                break;
            case 2:
                x = UnityEngine.Random.Range(-outerSize.x / 2, -innerSize.x / 2);
                y = UnityEngine.Random.Range(-outerSize.y / 2, outerSize.y / 2);
                break;
            case 3:
                x = UnityEngine.Random.Range(innerSize.x / 2, outerSize.x / 2);
                y = UnityEngine.Random.Range(-outerSize.y / 2, outerSize.y / 2);
                break;
            default:
                x = 0;
                y = 0;
                break;

        }
        return new Vector2 (x, y);
             

    }
}
