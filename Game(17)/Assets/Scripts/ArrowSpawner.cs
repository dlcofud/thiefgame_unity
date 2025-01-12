using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject[] arrowPrefabs; // Down, Up, Left, Right
    public Transform[] judgePoints; // Down, Up, Left, Right ��ġ
    public string jsonFilePath = "Assets/Data/musicnode1.json"; // JSON ���� ���

    public enum ArrowDirection { Down = 0, Up = 1, Left = 2, Right = 3 }


    private List<DrumNode> drumNodes; // JSON���� ���� ������ ����
    private int currentNodeIndex = 0; // ���� ��� ��ġ
    private float songStartTime;

    [System.Serializable]
    public class DrumNode
    {
        public float time; // �巳 �Ҹ��� �߻��� �ð�
        public ArrowDirection type; // ȭ��ǥ ����
    }

    private void Start()
    {
        songStartTime = Time.time; // �뷡 ���� �ð� ���
        LoadJsonData(); // JSON ���� �б�
    }

    private void Update()
    {
        // �뷡 ��� �� ��� �ð� ���
        float elapsedTime = Time.time - songStartTime;

        // ���� ��� Ȯ�� �� ȭ��ǥ ����
        while (currentNodeIndex < drumNodes.Count && drumNodes[currentNodeIndex].time <= elapsedTime)
        {
            SpawnArrow(drumNodes[currentNodeIndex]);
            currentNodeIndex++;
        }
    }

    void LoadJsonData()
    {
        // JSON ���� �б�
        string jsonData = File.ReadAllText(jsonFilePath);
        drumNodes = new List<DrumNode>(JsonHelper.FromJson<DrumNode>(jsonData));

        // drumNodes ����Ʈ ���� ���
        foreach (var node in drumNodes)
        {
            Debug.Log($"DrumNode - Time: {node.time}, Type: {node.type}");
        }
    }

    void SpawnArrow(DrumNode node)
    {
        int directionIndex = (int)node.type; // ArrowDirection�� ������ ��ȯ
        if (directionIndex < 0 || directionIndex >= arrowPrefabs.Length) return;

        // ȭ��ǥ ����
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
            default: return -1; // �� �� ���� Ÿ��
        }
    }

    // JSON �迭 �Ľ� ���� Ŭ����
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
