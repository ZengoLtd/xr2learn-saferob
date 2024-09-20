using MEM;
using TMPro;
using UnityEngine;

namespace Zengo.Inventory
{
    public class ListSlot : MonoBehaviour
    {

        public TMP_Text itemName;
        public Item item;


        public void SetName(string name)
        {
            itemName.text = name;
        }

        public void RemoveItemButtonClick()
        {
            if (item != null)
            {
                if (item.model.type == ItemType.Ropes)
                {
                    int count = 0;
                    foreach (Item item in InventoryManager.Instance.slottedItems)
                    {
                        if (item.model.type == ItemType.Ropes)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        return;
                    }
                }
                InventoryManager.Instance.RemoveItem(item);
            }
        }

    }
}
