using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScriptableObject), true)]
public class ScriptableObjectDrawer : PropertyDrawer
{
    private bool _canCache;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        _canCache = true;
        var indent = EditorGUI.indentLevel;
        var indentOffset = 15 * indent;
        EditorGUI.indentLevel = 0;

        var refRect = position;
        refRect.height = EditorGUIUtility.singleLineHeight;
        position.y += refRect.height;
        position.height -= refRect.height;

        var labelRect = refRect;
        labelRect.width = EditorGUIUtility.labelWidth;
        refRect.x += labelRect.width;
        refRect.width -= labelRect.width;
        labelRect.width -= indentOffset;
        labelRect.x += indentOffset;

        var popupRect = labelRect;
        popupRect.x = labelRect.width + labelRect.x - 16;
        popupRect.width = 16;
        string fieldPath = property.propertyPath;
        if (property.propertyPath.EndsWith("]"))
        {
            fieldPath = fieldPath.Remove(fieldPath.LastIndexOf('.'));
            fieldPath = fieldPath.Remove(fieldPath.LastIndexOf('.'));
        }
        var type = property.serializedObject.targetObject.GetType().GetField(property.isArray ? property.arrayElementType : fieldPath, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).FieldType;
        if (type.IsArray) type = type.GetElementType();
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) type = type.GetGenericArguments()[0];
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetExportedTypes()).Where(e => !e.IsAbstract && type.IsAssignableFrom(e));
        var selected = EditorGUI.Popup(popupRect, -1, types.Select(e => e.Name).ToArray());
        if (selected >= 0)
        {
            property.objectReferenceValue = ScriptableObject.CreateInstance(types.ElementAt(selected));
        }

        EditorGUI.PropertyField(refRect, property, GUIContent.none);

        if (property.objectReferenceValue && _isCreated(property.objectReferenceValue) && property.isExpanded)
        {
            EditorGUI.indentLevel = indent+1;
            SerializedObject o = new SerializedObject(property.objectReferenceValue);
            var p = o.GetIterator();
            EditorGUI.BeginChangeCheck();
            if (p.NextVisible(true)) do
            {
                position.height = EditorGUI.GetPropertyHeight(p);
                if (EditorGUI.PropertyField(position, p, p.isExpanded)) _canCache = false;
                position.y += position.height;
            }
            while (p.NextVisible(false));
            if(EditorGUI.EndChangeCheck()) o.ApplyModifiedProperties();
            EditorGUI.indentLevel = 0;
        }

        if (property.objectReferenceValue && _isCreated(property.objectReferenceValue))
        {
            if (EditorGUI.Foldout(labelRect, property.isExpanded, property.displayName))
            {
                _canCache = false;
                property.isExpanded = true;
            }
            else
            {
                property.isExpanded = false;
            }
        }
        else
        {
            EditorGUI.LabelField(labelRect, property.displayName);
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var h = base.GetPropertyHeight(property, label);
        if (!property.objectReferenceValue || !_isCreated(property.objectReferenceValue) || !property.isExpanded) return h;

        SerializedObject o = new SerializedObject(property.objectReferenceValue);
        var p = o.GetIterator();
        if (p.NextVisible(true)) do
        {
            var pH = EditorGUI.GetPropertyHeight(p, p.isExpanded);
            h += pH;
        }
        while (p.NextVisible(false));
        return h;
    }

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        return !property.objectReferenceValue || _canCache;
    }

    private bool _isCreated(UnityEngine.Object target)
    {
        return string.IsNullOrEmpty(AssetDatabase.GetAssetPath(target));
    }
}
