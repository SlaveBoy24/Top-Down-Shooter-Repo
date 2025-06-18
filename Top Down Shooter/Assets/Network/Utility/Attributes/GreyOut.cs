using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
 using UnityEditor;
#endif
using UnityEngine;
namespace Project.Utility.Attributes
{
	public class GreyOut : PropertyAttribute
	{
	}
	// Start is called before the first frame update
#if UNITY_EDITOR
 [CustomPropertyDrawer(typeof(GreyOut))]
 public class GreyOutDrawer: PropertyDrawer{
	 public override float GetPropertyHeight(SerializedProperty property, GUIContent lebel){
		 return EditorGUI.GetPropertyHeight(property, lebel, true);
	 }
 
 public override void OnGUI(Rect position, SerializedProperty property, GUIContent lebel){
	 GUI.enabled = false;
	 EditorGUI.PropertyField(position, property, lebel, true);
	 GUI.enabled = true;
 }
 
}
#endif
}