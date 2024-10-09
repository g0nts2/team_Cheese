using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public string[] SlotDB = new string[4];
    public Image[] SlotImageDB = new Image[4];

    public GameObject[] ItemDB;
    public Sprite[] ItemSpriteDB;

    private Player player;
    private DialogueManager dialogueManager;
    private DialogueContentManager dialogueContentManager;
    private UIManager uiManager;

    private bool canPickup = false;
    private Collider2D currentItemCollider;

    GameObject GetItemObject(string itemName)
    {
        switch (itemName)
        {
            case "BrownTeddyBear": return ItemDB[0];
            case "PinkTeddyBear": return ItemDB[1];
            case "YellowTeddyBear": return ItemDB[2];
            case "Cake": return ItemDB[3];
            case "NPC": return ItemDB[4];

            default: return null;
        }
    }

    Sprite GetItemSprite(string itemName)
    {
        switch (itemName)
        {
            case "BrownTeddyBear": return ItemSpriteDB[0];
            case "PinkTeddyBear": return ItemSpriteDB[1];
            case "YellowTeddyBear": return ItemSpriteDB[2];
            case "Cake": return ItemSpriteDB[3];
            case "NPC": return ItemSpriteDB[4];

            default: return null;
        }
    }

    void PlaceItem(int slotIndex)
    {
        if (SlotDB[slotIndex] == null) return;

        GameObject itemObject = GetItemObject(SlotDB[slotIndex]);
        Vector3 position = !Player.objectCollision ? player.transform.position : Object.pos;

        Instantiate(itemObject, position, Quaternion.identity);
        SlotDB[slotIndex] = null;
        SlotImageDB[slotIndex].sprite = null;
    }

    void PickupItem(string itemName, Collider2D other)
    {
        for (int i = 0; i < SlotDB.Length; i++)
        {
            if (string.IsNullOrEmpty(SlotDB[i]))
            {
                SlotDB[i] = itemName;
                SlotImageDB[i].sprite = GetItemSprite(itemName);
                Destroy(other.gameObject);
                break;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "DroppedBrownTeddyBear":
            case "BrownTeddyBear":
            case "PinkTeddyBear":
            case "YellowTeddyBear":
            case "Cake":
            case "NPC":
                canPickup = true;
                currentItemCollider = other;
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "DroppedBrownTeddyBear":
            case "BrownTeddyBear":
            case "PinkTeddyBear":
            case "YellowTeddyBear":
            case "Cake":
            case "NPC":
                canPickup = false;
                currentItemCollider = null;
                break;
        }
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.Space))
        {
            if (currentItemCollider != null)
            {
                string tag = currentItemCollider.gameObject.tag;

                switch (tag)
                {
                    case "DroppedBrownTeddyBear": PickupItem("BrownTeddyBear", currentItemCollider);
                        dialogueManager.ShowDialogue(dialogueContentManager.d_camera);
                        uiManager.CameraUI.SetActive(true);
                        break;
                    case "BrownTeddyBear": PickupItem("BrownTeddyBear", currentItemCollider); break;
                    case "PinkTeddyBear": PickupItem("PinkTeddyBear", currentItemCollider); break;
                    case "YellowTeddyBear": PickupItem("YellowTeddyBear", currentItemCollider); break;
                    case "Cake": PickupItem("Cake", currentItemCollider); break;
                    case "NPC": PickupItem("NPC", currentItemCollider); break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) PlaceItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) PlaceItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) PlaceItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) PlaceItem(3);
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueContentManager = FindObjectOfType<DialogueContentManager>();
        uiManager = FindObjectOfType<UIManager>();
    }
}