using System;
using System.Collections;
using UnityEngine;

//수정 있음(player controller)
public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs;  // 캐릭터 프리팹 목록
    public float[] spawnProbabilities;     // 생성 확률 (합이 1이어야 함)
    public Vector2 outerSize;              // 큰 네모의 크기
    public Vector2 innerSize;              // 작은 네모의 크기
    public int numberOfCharacters = 5;     // 생성할 캐릭터 수

    // 서브 캐릭터가 생성될 때 이벤트 (수정)
    public static event Action<GameObject> OnSubCharacterSpawned;

    private void Start()
    {
        // User 캐릭터(Player 태그) 설정
        SetPlayerBounds();

        SpawnCharacters();
    }

    // User 캐릭터의 경계값 설정
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

    // 캐릭터 생성 함수
    private void SpawnCharacters()
    {
        for (int i = 0; i < numberOfCharacters; i++)
        {
            // 확률에 따라 무작위로 캐릭터를 선택
            int randomIndex = GetRandomIndexBasedOnProbability();
            GameObject character = Instantiate(characterPrefabs[randomIndex]);

            // 캐릭터의 시작 위치를 계산
            Vector3 spawnPosition = GetRandomPositionOnPath();
            character.transform.position = spawnPosition;


            // CharacterMovement 스크립트 연결
            CharacterMovement movement = character.GetComponent<CharacterMovement>();
            if (movement != null)
            {
                movement.SetPathBounds(outerSize, innerSize);
            }
            else
            {
                Debug.LogError("CharacterMovement script is not attached to the character prefab.");
            }

            // 서브 캐릭터 생성 이벤트 호출(수정)
            OnSubCharacterSpawned?.Invoke(character);
        }
    }

    // 확률에 따라 캐릭터 인덱스를 반환하는 함수
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

        return spawnProbabilities.Length - 1; // 기본적으로 마지막 인덱스를 반환
    }

    // 경로 위에 무작위 위치를 계산하는 함수
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
