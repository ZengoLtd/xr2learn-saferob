using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zengo.Inventory
{
    public class Helmet : Item
    {
        public override void EquipItem()
        {
           base.EquipItem();
           if(NetworkDataHolder.Instance != null){
                NetworkDataHolder.Instance.AddOrUpdateData("Helmet", true);
           }
        }
         public override void RemoveItem()
        {
           base.RemoveItem();
           if(NetworkDataHolder.Instance != null){
                NetworkDataHolder.Instance.AddOrUpdateData("Helmet", false);
           }
        }
    }
}
