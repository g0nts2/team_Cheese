using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Table : MonoBehaviour
{
    public GameObject CamaraEvent;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cake")) //����ũ�� ���̺� �������� ����� �̺�Ʈ ������Ʈ
        {
            CamaraEvent.SetActive(true);
            UIManager.is_cake = true;
        }
    }
}
