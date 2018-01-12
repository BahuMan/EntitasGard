using Entitas;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

//[CustomEditor(typeof(EntitasInit))]
[CanEditMultipleObjects]
public class EntitasInitEditor : Editor
{
    SerializedProperty contextName;
    SerializedProperty nomnom;
    SerializedProperty components;


    private string[] contextNames;
    private Dictionary<string, ContextInfo> contextByName;


    void OnEnable()
    {
        contextName = serializedObject.FindProperty("contextName");
        nomnom = serializedObject.FindProperty("nomnom");
        components = serializedObject.FindProperty("components");

        contextByName = new Dictionary<string, ContextInfo>();
        contextNames = new string[Contexts.sharedInstance.allContexts.Length];
        for (int ci=0; ci<contextNames.Length; ++ci)
        {
            ContextInfo cinfo = Contexts.sharedInstance.allContexts[ci].contextInfo;
            Debug.Log("Context found: " + cinfo.name);
            contextNames[ci] = cinfo.name;
            contextByName[cinfo.name] = cinfo;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //standard stuff
        EditorGUILayout.PropertyField(contextName);
        EditorGUILayout.PropertyField(nomnom);
        if (nomnom.isArray && nomnom.isExpanded)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Size");
            EditorGUILayout.IntField(nomnom.arraySize, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            for (int n=0; n<nomnom.arraySize; ++n)
            {
                EditorGUILayout.PropertyField(nomnom.GetArrayElementAtIndex(n));
            }
        }

        //custom stuff
        ShowContextName();
        ShowComponentAddButton();
        ShowSerializedComponents();

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowSerializedComponents()
    {
        if (components == null)
        {
            Debug.LogError(" property 'components' not found?");
            return;
        }
        for (int sc=0; sc<components.arraySize; ++sc)
        {
            EditorGUILayout.PropertyField(components.GetArrayElementAtIndex(sc));
        }
    }

    private void ShowContextName()
    {
        EditorGUILayout.BeginHorizontal();

        GUIStyle labelBold = new GUIStyle(GUI.skin.label);
        labelBold.fontStyle = contextName.prefabOverride ? FontStyle.Bold : FontStyle.Normal;
        EditorGUILayout.LabelField("Context:", labelBold, GUILayout.ExpandWidth(false));

        GUIStyle dropBold = new GUIStyle("DropDownButton");
        dropBold.fontStyle = contextName.prefabOverride ? FontStyle.Bold : FontStyle.Normal;
        string showValue = contextName.hasMultipleDifferentValues ? "---" : contextName.stringValue;
        if (EditorGUILayout.DropdownButton(new GUIContent(showValue), FocusType.Keyboard, dropBold, GUILayout.ExpandWidth(true)))
        {
            GenericMenu clist = new GenericMenu();
            for (int c = 0; c < contextNames.Length; ++c)
            {
                clist.AddItem(new GUIContent(contextNames[c]), contextNames[c].Equals(contextName.stringValue), OnContextSelected, contextNames[c]);
            }

            clist.ShowAsContext();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ShowComponentAddButton()
    {

        if (contextName.hasMultipleDifferentValues)
        {
            EditorGUILayout.LabelField("Can't show components for different contexts", GUILayout.ExpandWidth(false));
            return;
        }

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Add Component: ", GUILayout.ExpandWidth(false));
        if (EditorGUILayout.DropdownButton(new GUIContent("<new>"), FocusType.Keyboard, GUILayout.ExpandWidth(true)))
        {
            ContextInfo ci = contextByName[contextName.stringValue];
            GenericMenu clist = new GenericMenu();
            for (int c = 0; c < ci.componentNames.Length ; ++c)
                if (ci.componentTypes[c].IsSerializable)
                    clist.AddItem(new GUIContent(ci.componentNames[c]), false, OnComponentSelected, ci.componentNames[c]);

            clist.ShowAsContext();
        }

        EditorGUILayout.EndHorizontal();

    }

    private Type FindTypeForComponentName(ContextInfo cinfo, string componentName)
    {
        for (int c = 0; c < cinfo.componentNames.Length; ++c)
        {
            if (cinfo.componentNames[c].Equals(componentName))
            {
                return cinfo.componentTypes[c];
            }
        }
        return null;
    }

    private void OnComponentSelected(object strNewComponentName)
    {
        string newComponentName = (string)strNewComponentName;
        ContextInfo cinfo = contextByName[contextName.stringValue];
        Type newComponentType = FindTypeForComponentName(cinfo, newComponentName);
        Debug.Log("Selected component " + newComponentName + "; found = " + (newComponentType != null));
    }


    private void OnContextSelected(object newContextName)
    {
        Debug.Log("Menu item selected");
        if (!((string) newContextName).Equals(contextName.stringValue))
        {
            contextName.stringValue = (string)newContextName;
            nomnom.ClearArray();
            serializedObject.ApplyModifiedProperties();
        }
    }


}
