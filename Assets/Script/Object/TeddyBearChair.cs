using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TeddyBearChair : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BrownTeddyBear")) //����ũ�� ���̺� �������� ����� �̺�Ʈ ������Ʈ
            UIManager.is_bear = true;
    }
}
