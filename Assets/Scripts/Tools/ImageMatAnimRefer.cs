using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ImageMatAnimRefer : MonoBehaviour
{
    public Material mat;

    [Header("�ֶ���1")]
    public string FloatFieldName;
    [Header("�ֶ�ֵ")]
    public float FloatFieldValue;
    
    [Header("�ֶ���2")]
    public string FloatFieldName1;
    [Header("�ֶ�ֵ")]
    public float FloatFieldValue1;

    [Header("�ֶ���3")]
    public string Vec2FieldName;
    [Header("�ֶ�ֵ")]
    public Vector2 Vec2FieldValue;
    
    [Header("�ֶ���4")]
    public string Vec2FieldName1;
    [Header("�ֶ�ֵ")]
    public Vector2 Vec2FieldValue1;

    void Update()
    {
        mat.SetFloat(FloatFieldName, FloatFieldValue);
        mat.SetFloat(FloatFieldName1, FloatFieldValue1);
        mat.SetVector(Vec2FieldName, Vec2FieldValue);
        mat.SetVector(Vec2FieldName1, Vec2FieldValue1);

        return;
        //for (int i = 0; i < IntFields.Count; i++)
        //{
        //    intField = IntFields[i];
        //    mat.SetFloat(intField.fieldName, intField.fieldValue);
        //}
        //for (int i = 0; i < Vec2Fields.Count; i++)
        //{
        //    vec2Field = Vec2Fields[i];
        //    mat.SetVector(vec2Field.fieldName, vec2Field.fieldValue);
        //}
    }
}

[Serializable]
public class MatField_IntUnit
{
    [Header("�ֶ���")]
    [SerializeField] public string fieldName;
    [Header("�ֶ�ֵ")]
    [SerializeField] public float fieldValue;
}

[Serializable]
public class MatField_Vec2Unit
{
    [Header("�ֶ���")]
    [SerializeField] public string fieldName;
    [Header("�ֶ�ֵ")]
    [SerializeField] public Vector2 fieldValue;
}