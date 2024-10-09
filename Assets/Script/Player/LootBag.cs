using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    List<Loot> GetDroppedItems()    // item ������ ��� ����
    {
        int randomNum = Random.Range(1, 101);     // 1-100 �ۼ�Ʈ Ȯ�� ����
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (randomNum <= item.dropChance)   // ���� �� ���� < ������ ��� Ȯ�� -> �������� �����. 
            {
                possibleItems.Add(item);
            }

            if (possibleItems.Count > 0)
            {
                return possibleItems;
            }

            // �� ����� ������ �� �������� 1���� �����ϴ� ���

            //if(possibleItems.Count > 0)
            //{
            //    Loot droppedItems = possibleItems[Random.Range(0, possibleItems.Count)];
            //     return droppeditem;
            //}

        }
         
        // �������� ������� �ʾ��� ��
        Debug.Log("No dropped item");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
       List<Loot> droppedItems = GetDroppedItems();
        if (droppedItems != null)    // �������� ����� ���
        {
            foreach (Loot droppedItem in droppedItems)

            {
                GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;

                // �������� ��ġ�� �ʰ� ���ݾ� �ٸ� ��ġ�� �����ǵ���
                spawnPosition += new Vector3(1, 1, 0);

                // �״� ���ǿ� �ֱ�
                // GetComponent<LootBag>().InstantiateLoot(transform.position);
            }
        }

    }
}

