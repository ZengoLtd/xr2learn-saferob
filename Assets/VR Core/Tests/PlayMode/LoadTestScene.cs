using System.Collections;
using BNG;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Linq;
using Zengo.Inventory;

public class LoadTestScene
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
   /* // A Test behaves as an ordinary method
    [Test]
    public void LoadTestSceneSimplePasses()
    {
       
    }
*/
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
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
    public IEnumerator TestIsTeleportExist()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");

        // Find the TeleportToNext object and its TeleportDestinationMarker component
        var teleportMarker = GameObject.Find("TeleportToNext/TeleportToNextStart/TeleporterBlock/Teleporter").GetComponent<TeleportDestinationMarker>();
        Assert.IsNotNull(teleportMarker, "No teleport marker set.");
        
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator TeleportFromTeleporterToTeleporterPasses()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");

        // Find the TeleportToNext object and its TeleportDestinationMarker component
        var teleportMarker = GameObject.Find("TeleportToNext/TeleportToNextStart/TeleporterBlock/Teleporter").GetComponent<TeleportDestinationMarker>();
        Assert.IsNotNull(teleportMarker, "No teleport marker set.");

        // Find the PlayerTeleport object and set its destination
        var playerTeleport = GameObject.FindObjectOfType<PlayerTeleport>();
        playerTeleport.DestinationObject = teleportMarker;

        // Perform the teleportation
        EventManager.PlayerTeleport(
            teleportMarker.DestinationTransform.position,
            teleportMarker.DestinationTransform.rotation,
            !teleportMarker.ForcePlayerRotation,
            teleportMarker.gameObject
        );

        // Wait for a frame to ensure the teleportation is complete
        yield return null;

        // Verify the player's position is within a certain range of the teleport destination, ignoring the Y-axis
        float tolerance = 0.0f; // Define a tolerance for position comparison
        Vector3 playerPosition = Camera.main.transform.position;
        Vector3 destinationPosition = teleportMarker.DestinationTransform.position;

        // Ignore the Y-axis for comparison
        playerPosition.y = destinationPosition.y;

        Assert.IsTrue(
            Vector3.Distance(playerPosition, destinationPosition) <= tolerance,
            $"Player did not teleport to the correct position. Expected: {destinationPosition}, Actual: {playerPosition}"
        );

        Debug.Log("Teleportation successful");
    }   
    
    [UnityTest]
    public IEnumerator TestItemPickUpToInventory()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipShoes = GameObject.Find("EquipItems/EquipShoes").GetComponent<Shoes>();
        Assert.IsNotNull(equipShoes, "No shoes set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipShoes.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Count.Equals(1), "Item not picked up.");
    }
    
    [UnityTest]
    public IEnumerator TestGlovesPickupToInventory(){
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipGloves = GameObject.Find("EquipItems/EquipGloves").GetComponent<Gloves>();
        Assert.IsNotNull(equipGloves, "No gloves set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipGloves.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().GetType().Equals(equipGloves.GetType()), "This isn't a gloves.");
    }
    
    [UnityTest]
    public IEnumerator TestHelmetPickupToInventory(){
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipHelmet = GameObject.Find("EquipItems/EquipHelmet").GetComponent<Helmet>();
        Assert.IsNotNull(equipHelmet, "No helmet set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipHelmet.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().GetType().Equals(equipHelmet.GetType()), "This isn't a helmet.");
    }
    
    [UnityTest]
    public IEnumerator TestGlassesPickupToInventory(){
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipGlasses = GameObject.Find("EquipItems/EquipGlasses").GetComponent<Glasses>();
        Assert.IsNotNull(equipGlasses, "No glasses set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipGlasses.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().GetType().Equals(equipGlasses.GetType()), "This isn't a glasses.");
    }
    
    [UnityTest]
    public IEnumerator TestShoesPickupToInventory(){
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipShoes = GameObject.Find("EquipItems/EquipShoes").GetComponent<Shoes>();
        Assert.IsNotNull(equipShoes, "No shoes set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipShoes.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().GetType().Equals(equipShoes.GetType()), "This isn't a shoes.");
    }

    [UnityTest]
    public IEnumerator TestItemPickupSwap()
    {
        // Load the initial scene
        yield return SceneManager.LoadSceneAsync(GerPersistentSceneIndex());
        Debug.Log("Initial scene loaded");

        // Load the tester scene
        yield return SceneManager.LoadSceneAsync("Scenes/Tester");
        Debug.Log("Tester scene loaded");
        
        var equipGloves = GameObject.Find("EquipItems/EquipGloves").GetComponent<Gloves>();
        Assert.IsNotNull(equipGloves, "No gloves set.");

        var playerInventory = GameObject.FindObjectOfType<InventoryManager>();
        Assert.IsNotNull(playerInventory, "No inventory manager found.");
        
        // Find the Shoes object and set its destination
        equipGloves.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().name.Equals(equipGloves.name), "This isn't a gloves.");
        
        var equipGlovesSwap = GameObject.Find("EquipItemsSwap/EquipGloves").GetComponent<Gloves>();
        Assert.IsNotNull(equipGlovesSwap, "No gloves set.");
        
        // Find the Shoes object and set its destination
        equipGlovesSwap.GetComponent<OnGrab>().Grabbed();
        Assert.IsTrue(playerInventory.slottedItems.Last().name.Equals(equipGlovesSwap.name), "Not swapped.");
    }
}
