using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject targetComponent;
    public void testfunc(string key)
    {
        GlobalStashes.Backpack.SpawnItemByKey(targetComponent, key);
    }
}
