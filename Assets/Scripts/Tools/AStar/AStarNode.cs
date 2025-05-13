using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{

    /// <summary>
    /// 寻路消耗
    /// </summary>
    public float F;
    /// <summary>
    /// 距起点距离
    /// </summary>
    public float G;
    /// <summary>
    /// 距终点距离
    /// </summary>
    public float H;

    public int X;
    public int Y;

    /// <summary>
    /// 父节点
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
