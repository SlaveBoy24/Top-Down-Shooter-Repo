using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Clouth", menuName = "TestCloth/Clouth", order = 3)]
public class ClothModel : ScriptableObject
{
    public Mesh objectMesh;
    public Material[] materials;
    public List<int> bone = new List<int>();
    public string objectName;
}

