using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public static Vector3 Object_pos;

    //void OnCollisionEnter2D
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.object_collision = "사물";
            Object_pos = transform.position;
        }

        if(other.gameObject.tag == "item2")
        {
            //UIManager.Next_value = 10;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.object_collision = "땅";
        }
    }

}
