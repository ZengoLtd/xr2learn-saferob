using UnityEngine;
using UnityEngine.InputSystem;
using BNG;

namespace Zengo.Inventory
{
    [SelectionBase]
    public class Item : MonoBehaviour
    {

        public ItemTypeSO descriptor;

        public ItemModel model => descriptor.item;
        public GameObject itemGameObject;
        public GameObject listSlot;

        public bool hideGameObject = false;


        public virtual void RemoveItem()
        {
            transform.GetChild(0).GetComponent<Collider>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
            if (hideGameObject)
            {
                itemGameObject.SetActive(true);
            }
            DestroyImmediate(listSlot);
        }

        public virtual void EquipItem()
        {
            DebugTextDisplay.SetDebugText($"Felvetted a {descriptor.item.name} tárgyat");
            EventManager.Equip();
            transform.GetChild(0).GetComponent<Collider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            if (hideGameObject)
            {
                itemGameObject.SetActive(false);
            }
        }


    }
}
