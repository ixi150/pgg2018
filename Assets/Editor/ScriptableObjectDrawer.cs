using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScriptableObject), true)]
public class ScriptableObjectDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //position = EditorGUI.IndentedRect(position);
        var refRect = position;
        refRect.height = EditorGUIUtility.singleLineHeight;
        position.y += refRect.height;
        position.height -= refRect.height;

        var labelRect = refRect;
        labelRect.width = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15;
        refRect.x += labelRect.width;
        refRect.width -= labelRect.width;

        var popupRect = labelRect;
        popupRect.x = labelRect.width;
        popupRect.width = 16 + EditorGUI.indentLevel * 15;
        var type = property.serializedObject.targetObject.GetType().GetField(property.propertyPath).FieldType;
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetExportedTypes()).Where(e => !e.IsAbstract && type.IsAssignableFrom(e));

        if (property.objectReferenceValue && _isCreated(property.objectReferenceValue))
        {
            property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, property.displayName);
        }
        else
        {
            EditorGUI.LabelField(labelRect, property.displayName);
        }
        var selected = EditorGUI.Popup(popupRect, -1, types.Select(e => e.Name).ToArray());
        if (selected >= 0)
        {
            property.objectReferenceValue = ScriptableObject.CreateInstance(types.ElementAt(selected));
        }

        EditorGUI.PropertyField(refRect, property, GUIContent.none);

        if (property.objectReferenceValue && _isCreated(property.objectReferenceValue) && property.isExpanded)
        {
            EditorGUI.indentLevel++;
            SerializedObject o = new SerializedObject(property.objectReferenceValue);
            var p = o.GetIterator();
            if (p.NextVisible(true)) do
                {
                    position.height = EditorGUI.GetPropertyHeight(p);
                    EditorGUI.PropertyField(position, p, p.isExpanded);
                    position.y += position.height;
                }
                while (p.NextVisible(false));

            o.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
        }

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

    private bool _isCreated(UnityEngine.Object target)
    {
        return string.IsNullOrEmpty(AssetDatabase.GetAssetPath(target));
    }
}
