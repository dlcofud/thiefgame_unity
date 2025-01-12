using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // 서브 캐릭터와의 상호작용 범위
    public GameObject rhythmGameUI; // 리듬 게임 UI 프리팹
    private GameObject instantiatedUI; // 생성된 리듬 게임 UI
    public bool isNearSubCharacter = false; // 주인공이 서브 캐릭터 가까이에 있는지 여부
    private List<GameObject> subCharacters = new List<GameObject>();

    private void OnEnable()
    {
        // 서브 캐릭터 생성 이벤트 구독
        CharacterSpawner.OnSubCharacterSpawned += AddSubCharacter;
    }

    private void OnDisable()
    {
        // 서브 캐릭터 생성 이벤트 구독 해제
        CharacterSpawner.OnSubCharacterSpawned -= AddSubCharacter;
    }

    // 생성된 서브 캐릭터를 리스트에 추가
    private void AddSubCharacter(GameObject spawnedCharacter)
    {
        subCharacters.Add(spawnedCharacter);
        Debug.Log("새로운 서브 캐릭터가 추가되었습니다.");
    }

    private void Update()
    {
        if (subCharacters.Count == 0)
        {
            Debug.LogWarning("서브 캐릭터가 설정되지 않았습니다.");
            return;
        }
        // 플레이어와 가장 가까운 서브 캐릭터를 찾기
        GameObject closestCharacter = null;
        float closestDistance = Mathf.Infinity;

        foreach (var subCharacter in subCharacters)
        {
            if (subCharacter == null) continue;

            float distance = Vector3.Distance(transform.position, subCharacter.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCharacter = subCharacter;
            }
        }

        // 가장 가까운 캐릭터가 상호작용 범위 안에 있으면
        if (closestCharacter != null && closestDistance <= interactionRange)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log($"조건 충족: {closestCharacter.name}와 가깝고 P를 눌렀습니다.");
                StartRhythmGame(closestCharacter);

            }
        }
    }

    // 리듬 게임 시작 (Overlay로 UI 추가)
    private void StartRhythmGame(GameObject selectedSubCharacter)
    {
        // 리듬 게임 UI가 이미 표시되어 있지 않으면
        if (instantiatedUI == null)
        {
            // 리듬 게임 UI 생성
            instantiatedUI = Instantiate(rhythmGameUI);

            // 생성된 UI의 Canvas 컴포넌트 확인
            Canvas canvas = instantiatedUI.GetComponent<Canvas>();
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                canvas.worldCamera = Camera.main; // Main Camera 설정
            }

            // 생성된 UI를 활성화
            instantiatedUI.SetActive(true);

            // Background와 GamePanel을 찾아서 활성화
            Transform backgroundTransform = instantiatedUI.transform.Find("Background");
            if (backgroundTransform != null)
            {
                backgroundTransform.gameObject.SetActive(true); // Background 활성화
            }

            Transform gamePanelTransform = instantiatedUI.transform.Find("GamePanel");
            if (gamePanelTransform != null)
            {
                gamePanelTransform.gameObject.SetActive(true); // GamePanel 활성화
            }
            // 서브 캐릭터 정보 UI에 전달
            SetSubCharacterInfoInUI(selectedSubCharacter);

            Debug.Log("리듬 게임 UI를 생성하고 표시했습니다."); // Console 출력

            // 게임 정지 상태로 변경
            Time.timeScale = 0f; // 모든 게임 오브젝트의 움직임 정지
        }
    }
    // 선택된 서브 캐릭터 정보 UI에 전달
    private void SetSubCharacterInfoInUI(GameObject selectedSubCharacter)
    {
        // selectedSubCharacter가 null인지 확인
        if (selectedSubCharacter == null)
        {
            Debug.LogError("selectedSubCharacter가 null입니다!");
            return;
        }

        if (instantiatedUI == null)
        {
            Debug.LogError("instantiatedUI가 null입니다!");
            return;
        }

        // 인스턴스화된 UI 요소에 접근
        TMP_Text nameText = instantiatedUI.transform.Find("NameText").GetComponent<TMP_Text>();
        if (nameText == null)
        {
            Debug.LogError("NameText를 찾을 수 없습니다!");
            return;
        }

        nameText.text = selectedSubCharacter.name; // UI에 이름 설정
    }



    // 리듬 게임 종료
    public void EndRhythmGame()
    {
        if (instantiatedUI != null)
        {
            // UI 비활성화
            instantiatedUI.SetActive(false);

            Debug.Log("리듬 게임 UI를 종료하고 게임을 재개합니다."); // Console 출력

            // 게임 재개
            Time.timeScale = 1f; // 게임 진행
        }
    }
}
