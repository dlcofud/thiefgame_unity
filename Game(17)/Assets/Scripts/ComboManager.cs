using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance; // Singleton으로 설정

    private int comboCount; // 콤보 점수

    public TextMeshProUGUI comboText;

    void Awake()
    {
        // Singleton 초기화
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

    // 콤보 증가
    public void IncreaseCombo()
    {
        comboCount++;
        UpdateComboText();
        Debug.Log("Combo Increased: " + comboCount);
    }

    // 콤보 초기화
    public void ResetCombo()
    {
        comboCount = 0;
        UpdateComboText();
        //Debug.Log("Combo Reset");
    }

    // 현재 콤보 값 가져오기
    public int GetCombo()
    {
        return comboCount;
    }

    // 콤보 텍스트 업데이트
    private void UpdateComboText()
    {
        if (comboText != null)
        {
            comboText.text = "Combo " + comboCount;
        }
    }
}
