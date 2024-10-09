using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    public Transform player;

    public Vector3 pos;
    public Vector3 offset;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        pos = transform.position;

        if(GameManager.GameState == "Tutorial")
        {
            //Ʃ�丮�� ������ �� ī�޶�
            if (player.position.y > 46.5f && player.position.y < 50.9f)
            {
                pos.y = player.position.y + offset.y;
            }
        }

        else if (GameManager.GameState == "Tutorial Cut Scene")
        {
            //����ũ�� ���� �� �ƾ� ī�޶�
            pos.x = -51.9f;
            pos.y = 47f;
        }

        else if (GameManager.GameState == "Stage1")
        {
            //Stage1 �� ī�޶�
            pos.x = player.position.x + offset.x;
            pos.y = player.position.y + offset.y;
        }

        else if (GameManager.GameState == "Demo")
        {
            //Demo �� ī�޶�
            pos.x = player.position.x + offset.x;
            pos.y = player.position.y + offset.y;
        }

        transform.position = pos;
    }
}
