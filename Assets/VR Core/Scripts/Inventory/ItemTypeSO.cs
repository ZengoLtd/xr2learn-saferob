using UnityEngine;

namespace Zengo.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item/Item", order = 0)]
    public class ItemTypeSO : ScriptableObject
    {
        public ItemModel item;
        public GameObject prefab;
    }
}
