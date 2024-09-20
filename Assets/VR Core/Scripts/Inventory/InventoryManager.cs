
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Zengo.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public List<Item> slottedItems = new List<Item>();
        public int maxInventorySize;
        public InventoryUI inventoryUI;

        public static InventoryManager Instance;
        void OnDestroy()
        {
            if(Instance == this){
                Instance = null;
            }
        }
        void OnDisable()
        {
            if(Instance == this){
                Instance = null;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                {
                    Debug.Log("Instance already exists, destroying object!");
                    Destroy(this);
                }
            }
            EventManager.OnBeforeSceneChange += () => { Instance = null; };

        }

        public bool HasItem(Item item)
        {
            foreach (Item invItem in slottedItems)
            {
                if (invItem.model.type == item.model.type)
                {
                    return true;
                }
            }
            return false;
        }

        public Item GetitemByType(ItemType type)
        {
            foreach (Item invItem in slottedItems)
            {
                if (invItem.model.type == type)
                {
                    return invItem;
                }
            }
            return null;
        }


        public bool HasItem(ItemType type)
        {
            foreach (Item invItem in slottedItems)
            {
                if (invItem.model.type == type)
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveItemByType(ItemTypeSO type)
        {
            for (int i = slottedItems.Count - 1; i >= 0; i--)
            {
                if (slottedItems[i].descriptor == type)
                {
                    RemoveItem(slottedItems[i]);
                }
            }
        }

        Item AlreadyEquippedItem(Item item)
        {
            foreach (Item invItem in slottedItems)
            {
                if (invItem.model.type == item.model.type)
                {
                    return invItem;
                }
            }
            return null;
        }

        void SwapItems(Item oldItem, Item newItem)
        {
            RemoveItem(oldItem);
            slottedItems.Add(newItem);
            newItem.GetComponent<Item>().EquipItem();
            inventoryUI.PlaceIntoUISlot(newItem);
        }

        public void ItemPickup(Item item)
        {
            EventManager.Equip();
            if (HasItem(item))
            {
                Item oldItem = AlreadyEquippedItem(item);
                SwapItems(oldItem, item);
                CommunicationManager.Instance?.Log("Felhasználó tárgyat cserélt  (" + item.model.name + ").");
            }
            else
            {
                if(maxInventorySize <= slottedItems.Count)
                {
                    Debug.Log("Nincs elég hely az új tárgyaknak");
                    return;
                }
                slottedItems.Add(item);
                item.GetComponent<Item>().EquipItem();
                CommunicationManager.Instance?.Log("Felhasználó tárgyat vett fel  (" + item.model.name + ").");
                inventoryUI.PlaceIntoUISlot(item);
            }
        }

        public void RemoveItem(Item item)
        {
            EventManager.Unequip();
            item.GetComponent<Item>().RemoveItem();
            slottedItems.Remove(item);
            inventoryUI.RemoveFromUISlot(item);
            EventManager.Unequip();
        }
    }
}
