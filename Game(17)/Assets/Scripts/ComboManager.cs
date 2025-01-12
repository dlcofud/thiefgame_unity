using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance; // Singleton���� ����

    private int comboCount; // �޺� ����

    public TextMeshProUGUI comboText;

    void Awake()
    {
        // Singleton �ʱ�ȭ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �޺� ����
    public void IncreaseCombo()
    {
        comboCount++;
        UpdateComboText();
        Debug.Log("Combo Increased: " + comboCount);
    }

    // �޺� �ʱ�ȭ
    public void ResetCombo()
    {
        comboCount = 0;
        UpdateComboText();
        //Debug.Log("Combo Reset");
    }

    // ���� �޺� �� ��������
    public int GetCombo()
    {
        return comboCount;
    }

    // �޺� �ؽ�Ʈ ������Ʈ
    private void UpdateComboText()
    {
        if (comboText != null)
        {
            comboText.text = "Combo " + comboCount;
        }
    }
}
