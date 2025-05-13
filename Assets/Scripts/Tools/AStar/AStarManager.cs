using System.Collections.Generic;
using UnityEngine;

public class AStarManager : BaseSingleton<AStarManager>
{
    private List<AStarNode> openList = new List<AStarNode>();
    private List<AStarNode> closeList = new List<AStarNode>();

    public int MapW;
    public int MapH;

    public AStarNode[,] aStarNodes;

    AStarNode startNode;
    AStarNode endNode;

    /// <summary>
    /// 根据地图中的槽的类型初始化路径节点
    /// </summary>
    /// <param name="colNum"></param>
    /// <param name="rowNum"></param>
    /// <param name="slotType"></param>
    public void InitMap(int colNum, int rowNum, E_AStarNodeType[,] slotType)
    {
        MapW = colNum;
        MapH = rowNum;
        Debug.Log(" 列：" + colNum + "行：" + rowNum);

        aStarNodes = new AStarNode[colNum, rowNum];

        for (int i = 0; i < colNum; ++i)//++i和i++在for循环中计算无差别，但是前置自增不用创建和销毁临时变量
        {
            for (int j = 0; j < rowNum; ++j)
            {
                AStarNode node = new AStarNode(i, j, slotType[i,j]);
                aStarNodes[i, j] = node;
            }
        }

        //Debug.Log("2-6是："+aStarNodes[2,6].NodeType);
    }

    public List<AStarNode> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        //起点和终点不合法直接返回
        if (IsBeyondBoardCheck(startPos.x, startPos.y) || IsBeyondBoardCheck(endPos.x, endPos.y))
        {
            return null;
        }
        startNode = aStarNodes[startPos.x, startPos.y];
        endNode = aStarNodes[endPos.x, endPos.y];
        //Debug.Log("起："+startNode.X+" "+ startNode.Y + " 终"+endNode.X + " " + endNode.Y);
        //开始和结束点不能是障碍点
        if (startNode.NodeType == E_AStarNodeType.obstacable || endNode.NodeType == E_AStarNodeType.obstacable)
        { 
            Debug.Log("不合法！！");
            return null;
        }
        openList.Clear();
        closeList.Clear();

        startNode.Father = null;
        startNode.F = 0;
        startNode.G = 0;
        startNode.H = 0;
        closeList.Add(startNode);

        while (true)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int x = startNode.X + i;
                    int y = startNode.Y + j;

                    if (i == 0 && j == 0)
                        continue;
                    if (i != 0 && j != 0)
                        //8方向
                        //GetNodeCheck(x, y, 1.4f, startNode, endNode);
                        //4方向
                        continue;
                    else
                        GetNodeCheck(x, y, 1f, startNode, endNode);
                }
            }
            if (openList.Count == 0)
                return null;

            closeList.Add(FindMinCostNode());
            startNode = FindMinCostNode();
            openList.Remove(startNode);

            if (startNode == endNode)
            {
                //找到终点了！！
                List<AStarNode> correctNodes = new List<AStarNode>();
                correctNodes.Add(endNode);
                while (endNode.Father != null)
                {
                    correctNodes.Add(endNode.Father);
                    endNode = endNode.Father;
                }
                correctNodes.Reverse();
                //Debug.Log("寻找路径：完毕");
                return correctNodes;
            }
        }
    }

    bool IsBeyondBoardCheck(int x, int y)
    {
        if (x < 0 || x >= MapW || y < 0 || y >= MapH)
        {
            return true;
        }
        return false;
    }

    AStarNode currentNode;

    void GetNodeCheck(int x, int y, float g, AStarNode father, AStarNode end)
    {
        //超出边界
        if (IsBeyondBoardCheck(x, y))
            return;
        currentNode = aStarNodes[x, y];
        if (currentNode == null ||
        currentNode.NodeType == E_AStarNodeType.obstacable ||
        openList.Contains(currentNode) ||
        closeList.Contains(currentNode))
            return;
        currentNode.Father = father;

        currentNode.G = father.G + g;
        currentNode.H = Mathf.Abs(end.X - x) + Mathf.Abs(end.Y - y);
        currentNode.F = 0;

        openList.Add(currentNode);
    }


    AStarNode minNode;
    float minFCost;
    /// <summary>
    /// 找到消耗最小的节点
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    AStarNode FindMinCostNode()
    {
        if (openList == null || openList.Count == 0)
            throw new System.InvalidOperationException("List is empty.");

        minFCost = openList[0].F;
        minNode = openList[0];
        for (int i = 1; i < openList.Count; i++)
        {
            if (openList[i].F < minFCost)
            {
                minFCost = openList[i].F;
                minNode = openList[i];
            }
        }
        return minNode;
    }
}
