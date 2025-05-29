using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SubcolCustomSpecialSquare))]
public class SubcolCustomSpecialSquareEditor : Editor
{
    private SerializedProperty _specialTypeProp;
    private SerializedProperty _subColIndex;
    private SerializedProperty _index;

    // �����ֶε�����
    private SerializedProperty _taskTriggerIndex;

    private void OnEnable()
    {
        // ��ȡ�������Ե�����
        _specialTypeProp = serializedObject.FindProperty("specialType");
        _subColIndex = serializedObject.FindProperty("subColIndex");
        _index = serializedObject.FindProperty("index");

        _taskTriggerIndex = serializedObject.FindProperty("taskTriggerIndex");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // ���ƻ�������
        EditorGUILayout.PropertyField(_specialTypeProp);
        EditorGUILayout.PropertyField(_subColIndex);
        EditorGUILayout.PropertyField(_index);

        EditorGUILayout.Space(10);

        // ����ö��������ʾ��ͬ����
        E_SpecialSquareType currentType = (E_SpecialSquareType)_specialTypeProp.enumValueIndex;

        switch (currentType)
        {
            case E_SpecialSquareType.�����ռ�:
                //EditorGUILayout.PropertyField(_explosionRadiusProp, new GUIContent("��ը��Χ"));
                break;

            case E_SpecialSquareType.��������:
                EditorGUILayout.PropertyField(_taskTriggerIndex, new GUIContent("����������"));
                break;

            case E_SpecialSquareType.����:
                //EditorGUILayout.PropertyField(_healAmountProp, new GUIContent("������"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
