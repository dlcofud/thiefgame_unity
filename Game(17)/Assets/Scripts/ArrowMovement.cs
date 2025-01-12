using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ArrowMovement : MonoBehaviour
{
    public float speed = 5f;

    private void Update()
    {
        transform.Translate(Vector3.up*speed*Time.deltaTime);
        
        //ȭ�� ������ ������ ���� 
        if (transform.position.y >10f)
        {
            ComboManager.Instance.ResetCombo();
            Destroy(gameObject);
        }
    }





}
