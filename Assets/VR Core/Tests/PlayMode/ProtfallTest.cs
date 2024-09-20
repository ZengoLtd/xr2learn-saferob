using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Linq;
using Zengo.Inventory;

public class ProtfallTest : MonoBehaviour
{

     public int GerPersistentSceneIndex(){
        for (int i = 1; i<1000; i++)
        {
            
            var s = SceneUtility.GetScenePathByBuildIndex(i);
            if(s.Length <=0){
                break;
            }
            if(s.Contains("Persistent")){
                return i;
            }
        }
        return 0;
    }
    [UnityTest]
    public IEnumerator SceneLoadManagerInstanceExistsPasses()
    {
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Scene loaded");
        Assert.IsTrue(SceneLoadManager.Instance != null);
    }

    [UnityTest]
    public IEnumerator TestSceneLoadsPasses()
    {
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Scene loaded");
        SceneLoadManager.Instance.LoadScene("Scenes/Tester");
        Assert.IsTrue(SceneLoadManager.Instance != null);
    }

    [UnityTest]
    public IEnumerator TestItemPickUpHarnessToInventoryUI()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipHarness = GameObject.Find("Equipment UI prompts_new/Harness_equip").GetComponent<Harness>();
        Assert.IsNotNull(equipHarness, "No harness set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipHarness.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().GetType().Equals(equipHarness.GetType()), "This isn't a harness.");
    }
    
    [UnityTest]
    public IEnumerator TestItemPickUpHelmetToInventoryUI()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipHelmet = GameObject.Find("Equipment UI prompts_new/Helmet_equip").GetComponent<Helmet>();
        Assert.IsNotNull(equipHelmet, "No helmet set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipHelmet.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().GetType().Equals(equipHelmet.GetType()), "This isn't a helmet.");
    }
    
    [UnityTest]
    public IEnumerator TestItemPickUpGlovesToInventoryUI()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipGloves = GameObject.Find("Equipment UI prompts_new/Gloves_equip").GetComponent<Gloves>();
        Assert.IsNotNull(equipGloves, "No gloves set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipGloves.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().GetType().Equals(equipGloves.GetType()), "This isn't a gloves.");
    }
    
    [UnityTest]
    public IEnumerator TestItemPickUpGlassesToInventoryUI()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipGlasses = GameObject.Find("Equipment UI prompts_new/Glasses_equip").GetComponent<Glasses>();
        Assert.IsNotNull(equipGlasses, "No glasses set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipGlasses.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().GetType().Equals(equipGlasses.GetType()), "This isn't a glasses.");
    }
    
    [UnityTest]
    public IEnumerator TestItemPickUpShoesToInventory()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipShoes = GameObject.Find("Equipment UI prompts_new/Shoes_equip").GetComponent<Item>();
        Assert.IsNotNull(equipShoes, "No shoes set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipShoes.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().GetType().Equals(equipShoes.GetType()), "This isn't a shoes.");
    }
    
    [UnityTest]
    public IEnumerator TestItemPickUpStrapToInventory()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipStrap = GameObject.Find("Equipment UI prompts_new/Strap_equip").GetComponent<Other>();
        Assert.IsNotNull(equipStrap, "No shoes set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipStrap.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().GetType().Equals(equipStrap.GetType()), "This isn't a shoes.");
    }
    
    [UnityTest]
    public IEnumerator TestItemPickUpRopeToInventory()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipRope = GameObject.Find("Equipment UI prompts_new/Rope_equip").GetComponent<Ropes>();
        Assert.IsNotNull(equipRope, "No rope set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipRope.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(InventoryManager.Instance.HasItem(ItemType.Ropes), "This isn't a rope.");
    }
    
    [UnityTest]
    public IEnumerator TestCheckRopeType()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipRope = GameObject.Find("Equipment UI prompts_new/Rope_equip").GetComponent<Ropes>();
        Assert.IsNotNull(equipRope, "No rope set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipRope.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(InventoryManager.Instance.HasItem(ItemType.Ropes), "This isn't a rope.");

        Assert.IsTrue(playerInventory.slottedItems.Last().GetComponent<Ropes>().descriptor.Equals(equipRope.descriptor), "This isn't that type of rope.");
    }
}
