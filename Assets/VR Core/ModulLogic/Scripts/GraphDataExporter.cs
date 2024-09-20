
using System.Collections.Generic;
using UnityEngine;

public class GraphDataExporter : MonoBehaviour
{
     [SerializeField]
    string nodes = "";
    [SerializeField]
    string connections = "";

    void Start()
    {
        nodes = GetNodes();
        connections = GetConnections(); 

    }


    string GetPath(GameObject gameObject){
        string path = gameObject.name;
        while(gameObject.transform.parent != null){
            gameObject = gameObject.transform.parent.gameObject;
            path = gameObject.name + "." + path;
        }
        return path;

    }
    string NodeToName(BlockLogicBase block){
        return GetPath(block.gameObject)+"."+block.name ;
    }

    string GetConnections(){
        string connections = "[";
        foreach (var block in GetBlocks())
        {
            foreach (var data in block.ActivateOn)
            {
                foreach (var action in data.actions)
                {
                    if(data.block != null && block != null){
                        connections += "('" + NodeToName(block) + "', '" + NodeToName(data.block) + "'),";
                    }
                    
                }
            }
        }

        connections += "]";
        return connections;
    }



    // getnodes function to string in the format of ['A','B','C']
        
    string GetNodes(){
        string returndata = "";
        returndata += "[";
        
        var blocks = GetBlocks();
        foreach (var block in blocks)
        {
            returndata += "'" +NodeToName(block)+ "',";
        }
       
        returndata += "]";
       return returndata;
    }
   List<BlockLogicBase> GetBlocks(){
        var returndata = new List<BlockLogicBase>();
        
        var blocks = FindObjectsOfType<BlockLogicBase>();
        foreach (var block in blocks)
        {
            returndata.Add(block);
        }


        return returndata;

   }
}
