using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject[] arrowPrefabs; // Down, Up, Left, Right
    public Transform[] judgePoints; // Down, Up, Left, Right 위치
    public string jsonFilePath = "Assets/Data/musicnode1.json"; // JSON 파일 경로

    public enum ArrowDirection { Down = 0, Up = 1, Left = 2, Right = 3 }


    private List<DrumNode> drumNodes; // JSON에서 읽은 데이터 저장
    private int currentNodeIndex = 0; // 현재 노드 위치
    private float songStartTime;

    [System.Serializable]
    public class DrumNode
    {
        public float time; // 드럼 소리가 발생할 시간
        public ArrowDirection type; // 화살표 방향
    }

    private void Start()
    {
        songStartTime = Time.time; // 노래 시작 시간 기록
        LoadJsonData(); // JSON 파일 읽기
    }

    private void Update()
    {
        // 노래 재생 후 경과 시간 계산
        float elapsedTime = Time.time - songStartTime;

        // 현재 노드 확인 및 화살표 생성
        while (currentNodeIndex < drumNodes.Count && drumNodes[currentNodeIndex].time <= elapsedTime)
        {
            SpawnArrow(drumNodes[currentNodeIndex]);
            currentNodeIndex++;
        }
    }

    void LoadJsonData()
    {
        // JSON 파일 읽기
        string jsonData = File.ReadAllText(jsonFilePath);
        drumNodes = new List<DrumNode>(JsonHelper.FromJson<DrumNode>(jsonData));

        // drumNodes 리스트 내용 출력
        foreach (var node in drumNodes)
        {
            Debug.Log($"DrumNode - Time: {node.time}, Type: {node.type}");
        }
    }

    void SpawnArrow(DrumNode node)
    {
        int directionIndex = (int)node.type; // ArrowDirection을 정수로 변환
        if (directionIndex < 0 || directionIndex >= arrowPrefabs.Length) return;

        // 화살표 생성
        Vector3 spawnPosition = new Vector3(judgePoints[directionIndex].position.x, -4f, 0f);
        Instantiate(arrowPrefabs[directionIndex], spawnPosition, Quaternion.identity);
    }

    int GetDirectionIndex(string type)
    {
        switch (type.ToLower())
        {
            case "bass": return 0; // Down
            case "snare": return 1; // Up
            case "hihat": return 2; // Left
            case "floor tom": return 3; // Right
            default: return -1; // 알 수 없는 타입
        }
    }

    // JSON 배열 파싱 헬퍼 클래스
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{ \"Items\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
