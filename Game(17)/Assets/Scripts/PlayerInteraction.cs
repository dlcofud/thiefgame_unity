using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // ���� ĳ���Ϳ��� ��ȣ�ۿ� ����
    public GameObject rhythmGameUI; // ���� ���� UI ������
    private GameObject instantiatedUI; // ������ ���� ���� UI
    public bool isNearSubCharacter = false; // ���ΰ��� ���� ĳ���� �����̿� �ִ��� ����
    private List<GameObject> subCharacters = new List<GameObject>();

    private void OnEnable()
    {
        // ���� ĳ���� ���� �̺�Ʈ ����
        CharacterSpawner.OnSubCharacterSpawned += AddSubCharacter;
    }

    private void OnDisable()
    {
        // ���� ĳ���� ���� �̺�Ʈ ���� ����
        CharacterSpawner.OnSubCharacterSpawned -= AddSubCharacter;
    }

    // ������ ���� ĳ���͸� ����Ʈ�� �߰�
    private void AddSubCharacter(GameObject spawnedCharacter)
    {
        subCharacters.Add(spawnedCharacter);
        Debug.Log("���ο� ���� ĳ���Ͱ� �߰��Ǿ����ϴ�.");
    }

    private void Update()
    {
        if (subCharacters.Count == 0)
        {
            Debug.LogWarning("���� ĳ���Ͱ� �������� �ʾҽ��ϴ�.");
            return;
        }
        // �÷��̾�� ���� ����� ���� ĳ���͸� ã��
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

        // ���� ����� ĳ���Ͱ� ��ȣ�ۿ� ���� �ȿ� ������
        if (closestCharacter != null && closestDistance <= interactionRange)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log($"���� ����: {closestCharacter.name}�� ������ P�� �������ϴ�.");
                StartRhythmGame(closestCharacter);

            }
        }
    }

    // ���� ���� ���� (Overlay�� UI �߰�)
    private void StartRhythmGame(GameObject selectedSubCharacter)
    {
        // ���� ���� UI�� �̹� ǥ�õǾ� ���� ������
        if (instantiatedUI == null)
        {
            // ���� ���� UI ����
            instantiatedUI = Instantiate(rhythmGameUI);

            // ������ UI�� Canvas ������Ʈ Ȯ��
            Canvas canvas = instantiatedUI.GetComponent<Canvas>();
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                canvas.worldCamera = Camera.main; // Main Camera ����
            }

            // ������ UI�� Ȱ��ȭ
            instantiatedUI.SetActive(true);

            // Background�� GamePanel�� ã�Ƽ� Ȱ��ȭ
            Transform backgroundTransform = instantiatedUI.transform.Find("Background");
            if (backgroundTransform != null)
            {
                backgroundTransform.gameObject.SetActive(true); // Background Ȱ��ȭ
            }

            Transform gamePanelTransform = instantiatedUI.transform.Find("GamePanel");
            if (gamePanelTransform != null)
            {
                gamePanelTransform.gameObject.SetActive(true); // GamePanel Ȱ��ȭ
            }
            // ���� ĳ���� ���� UI�� ����
            SetSubCharacterInfoInUI(selectedSubCharacter);

            Debug.Log("���� ���� UI�� �����ϰ� ǥ���߽��ϴ�."); // Console ���

            // ���� ���� ���·� ����
            Time.timeScale = 0f; // ��� ���� ������Ʈ�� ������ ����
        }
    }
    // ���õ� ���� ĳ���� ���� UI�� ����
    private void SetSubCharacterInfoInUI(GameObject selectedSubCharacter)
    {
        // selectedSubCharacter�� null���� Ȯ��
        if (selectedSubCharacter == null)
        {
            Debug.LogError("selectedSubCharacter�� null�Դϴ�!");
            return;
        }

        if (instantiatedUI == null)
        {
            Debug.LogError("instantiatedUI�� null�Դϴ�!");
            return;
        }

        // �ν��Ͻ�ȭ�� UI ��ҿ� ����
        TMP_Text nameText = instantiatedUI.transform.Find("NameText").GetComponent<TMP_Text>();
        if (nameText == null)
        {
            Debug.LogError("NameText�� ã�� �� �����ϴ�!");
            return;
        }

        nameText.text = selectedSubCharacter.name; // UI�� �̸� ����
    }



    // ���� ���� ����
    public void EndRhythmGame()
    {
        if (instantiatedUI != null)
        {
            // UI ��Ȱ��ȭ
            instantiatedUI.SetActive(false);

            Debug.Log("���� ���� UI�� �����ϰ� ������ �簳�մϴ�."); // Console ���

            // ���� �簳
            Time.timeScale = 1f; // ���� ����
        }
    }
}
