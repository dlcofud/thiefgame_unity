using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ArrowMovement : MonoBehaviour
{
    public float speed = 5f;

    private void Update()
    {
        transform.Translate(Vector3.up*speed*Time.deltaTime);
        
        //화면 밖으로 나가면 삭제 
        if (transform.position.y >10f)
        {
            ComboManager.Instance.ResetCombo();
            Destroy(gameObject);
        }
    }





}
