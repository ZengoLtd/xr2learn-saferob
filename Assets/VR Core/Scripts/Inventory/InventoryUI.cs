using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Zengo.Inventory
{

    public class InventoryUI : MonoBehaviour
    {
        public List<Slots> inventorySlots = new List<Slots>();

        public GameObject[] listSlots;
        public GameObject listSlotPrefab;


        public void PlaceIntoUISlot(Item item)
        {
            foreach (Slots slot in inventorySlots)
            {
                foreach (ItemType itemTypes in slot.allowedItemTypes)
                {
                    if (item.model.type == itemTypes)
                    {
                        slot.ToggleEquipment();
                        AddToList(item);
                        return;
                    }
                }
            }

        }

        public void RemoveFromUISlot(Item item)
        {
            foreach (Slots slot in inventorySlots)
            {
                foreach (ItemType itemTypes in slot.allowedItemTypes)
                {
                    if (item.model.type == itemTypes)
                    {
                        slot.ToggleEquipment();

                    }
                }
            }

        }

        void AddToList(Item item)
        {
            foreach (GameObject listobj in listSlots)
            {
                if (listobj.transform.childCount < 1)
                {
                    GameObject list = Instantiate(listSlotPrefab, listobj.transform);

                    item.listSlot = list;
                    list.GetComponent<ListSlot>().item = item;
                    list.GetComponent<ListSlot>().SetName(item.model.name.GetLocalizedString());

                    return;
                }
            }
        }
    }
}