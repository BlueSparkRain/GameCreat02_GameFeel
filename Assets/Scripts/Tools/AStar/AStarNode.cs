using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{

    /// <summary>
    /// Ѱ·����
    /// </summary>
    public float F;
    /// <summary>
    /// ��������
    /// </summary>
    public float G;
    /// <summary>
    /// ���յ����
    /// </summary>
    public float H;

    public int X;
    public int Y;

    /// <summary>
    /// ���ڵ�
    /// </summary>
    public AStarNode Father;
    
    public E_AStarNodeType NodeType;
    public AStarNode(int x,int y,E_AStarNodeType type) 
    {
        X = x;
        Y = y;
        NodeType = type;
    }
}

public enum E_AStarNodeType 
{
walkable,obstacable
}
