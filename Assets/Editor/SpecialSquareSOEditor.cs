using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpecialSquareSO))]
public class SpecialSquareSOEditor : Editor
{
    private SerializedProperty _specialTypeProp;
    private SerializedProperty _specialSpriteProp;
    private SerializedProperty _descriptionProp;

    // 新增字段的属性
    private SerializedProperty _taskTriggerIndex;

    private void OnEnable()
    {
        // 获取所有属性的引用
        _specialTypeProp = serializedObject.FindProperty("specialType");
        _specialSpriteProp = serializedObject.FindProperty("specialSprite");
        _descriptionProp = serializedObject.FindProperty("description");

        _taskTriggerIndex = serializedObject.FindProperty("taskTriggerIndex");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 绘制基础属性
        EditorGUILayout.PropertyField(_specialTypeProp);
        EditorGUILayout.PropertyField(_specialSpriteProp);
        EditorGUILayout.PropertyField(_descriptionProp);

        EditorGUILayout.Space(10);

        // 根据枚举类型显示不同内容
        E_SpecialSquareType currentType = (E_SpecialSquareType)_specialTypeProp.enumValueIndex;

        switch (currentType)
        {
            case E_SpecialSquareType.消融收集:
                //EditorGUILayout.PropertyField(_explosionRadiusProp, new GUIContent("爆炸范围"));
                break;

            case E_SpecialSquareType.触发消除:
                EditorGUILayout.PropertyField(_taskTriggerIndex, new GUIContent("触发任务编号"));
                break;

            case E_SpecialSquareType.传送:
                //EditorGUILayout.PropertyField(_healAmountProp, new GUIContent("治疗量"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
