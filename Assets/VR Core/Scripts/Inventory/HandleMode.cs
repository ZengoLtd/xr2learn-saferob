using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Zengo.Inventory;

public class HandleMode : MonoBehaviour
{
    public ItemAllowance allowance;

    bool checkInInventory(ItemTypeSO itemdescriptor){

        foreach (Item item in InventoryManager.Instance.slottedItems)
        {
            if (item.descriptor == itemdescriptor)
            {   
                return true;
            }    
        }
        return false;
    }

   
   
    List<ItemType> getOptimalTypes(AllowTypes allowTypes){
        List<ItemType> optimalTypes = new List<ItemType>();
        foreach (ItemTypeSO optimalItemSo in allowTypes.optimalItems)
        {
            if(optimalTypes.Contains(optimalItemSo.item.type) == false){
                optimalTypes.Add(optimalItemSo.item.type);
            }
        }
        return optimalTypes;
    }

    bool checkSingleType(AllowTypes allowTypes,ItemType itemtype){

      

        foreach (ItemTypeSO optimalItemSo in allowTypes.optimalItems)
        {
            //not the type we are looking for
            if(optimalItemSo.item.type != itemtype){
                continue;
            }
            //if we have the correct item type
            if(checkInInventory(optimalItemSo)){
                Debug.Log("Correct Item: "+optimalItemSo.item.type +" needed and we have: "+itemtype );
                return true;
            }

        }
        return false;
    }

    bool checkCombination(AllowTypes allowTypes){
        List<ItemType> optimalTypes = getOptimalTypes(allowTypes);
        foreach (ItemType optimalType in optimalTypes)
        {
            if(allowTypes.type.Contains(optimalType) == false){
                Debug.LogError("Optimal type is not allowed!!!!!!!!!!");
                return false;
            }
            foreach (ItemTypeSO item in allowTypes.badItems)
            {
                //if we have bad item
                if(checkInInventory(item)){
                    Debug.Log("Bad Item: "+item.item.name +" found and we have " );
                    EventManager.BadItemUsed();
                    return false;
                }

            }
            foreach (ItemTypeSO optimalItemSo in allowTypes.notoptimalItems)
            {
                //if we have the correct item type
                if(checkInInventory(optimalItemSo)){
                    Debug.Log("Not optimal Item: "+optimalItemSo.item.name +" found and we have it" );
                    return false;
                }

            }
            if(!checkSingleType(allowTypes,optimalType)){
                return false;
            }
        }
        return true;
    }

    public bool checkOptimals(){
        if (InventoryManager.Instance?.slottedItems.Count == 0){
            return false;
        }
        
        //running for all allowed combinations
        foreach (AllowTypes allowTypes in allowance.allowedItemTypes)
        {
            if(checkCombination(allowTypes)){
                return true;
            }
        }     
        return false;
    }



    public void CheckOptimalEquipmentUse(){
        Debug.Log("Check Optimal");   
        if(checkOptimals()){
            //we have all items
            Debug.Log("Any combination found"+ allowance.name);
            if(ModuleGoals.Instance.optimalUse == null){
                ModuleGoals.Instance.optimalUse = true;
            }
            
        }else{
            //we dont have all items
            Debug.Log("No combination found"+ allowance.name);
            ModuleGoals.Instance.optimalUse = false;
        }
    }

    private void OnEnable()
    {
        CheckItemPermissionOnHotspot();
        //OnOffHandles(false);
        EventManager.OnInventroyEquip += CheckItemPermissionOnHotspot;
        EventManager.OnInventoryUnequip += CheckItemPermissionOnHotspot;
    }


    private void Start()
    {
        CheckItemPermissionOnHotspot();
    }

    private void OnDisable()
    {
        EventManager.OnInventroyEquip -= CheckItemPermissionOnHotspot;
        EventManager.OnInventoryUnequip -= CheckItemPermissionOnHotspot;
    }


    public void CheckItemPermissionOnHotspot()
    {
        int allowTypesCount = 0;
        int count = 0;

        if (InventoryManager.Instance?.slottedItems.Count > 0)
        {
            foreach (AllowTypes allowTypes in allowance.allowedItemTypes)
            {
                allowTypesCount = allowTypes.type.Count;
                count = 0;
                foreach (ItemType itemTypes in allowTypes.type)
                {
                    foreach (Item item in InventoryManager.Instance.slottedItems)
                    {
                        if (item.model.type == itemTypes)
                        {
                            count++;
                            if (count == allowTypesCount)
                            {
                                OnOffHandles(true);
                                return;
                            }
                         
                        }
                        
                    }
                }
            }
        }
        else
        {
            OnOffHandles(false);
        }
        OnOffHandles(false);
    }


    void OnOffHandles(bool mode)
    {
        if (allowance != null)
        {
            transform.GetChild(0).gameObject.SetActive(mode);
        }
    }

    
}
