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
    /// ���ݵ�ͼ�еĲ۵����ͳ�ʼ��·���ڵ�
    /// </summary>
    /// <param name="colNum"></param>
    /// <param name="rowNum"></param>
    /// <param name="slotType"></param>
    public void InitMap(int colNum, int rowNum, E_AStarNodeType[,] slotType)
    {
        MapW = colNum;
        MapH = rowNum;
        Debug.Log(" �У�" + colNum + "�У�" + rowNum);

        aStarNodes = new AStarNode[colNum, rowNum];

        for (int i = 0; i < colNum; ++i)//++i��i++��forѭ���м����޲�𣬵���ǰ���������ô�����������ʱ����
        {
            for (int j = 0; j < rowNum; ++j)
            {
                AStarNode node = new AStarNode(i, j, slotType[i,j]);
                aStarNodes[i, j] = node;
            }
        }

        //Debug.Log("2-6�ǣ�"+aStarNodes[2,6].NodeType);
    }

    public List<AStarNode> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        //�����յ㲻�Ϸ�ֱ�ӷ���
        if (IsBeyondBoardCheck(startPos.x, startPos.y) || IsBeyondBoardCheck(endPos.x, endPos.y))
        {
            return null;
        }
        startNode = aStarNodes[startPos.x, startPos.y];
        endNode = aStarNodes[endPos.x, endPos.y];
        //Debug.Log("��"+startNode.X+" "+ startNode.Y + " ��"+endNode.X + " " + endNode.Y);
        //��ʼ�ͽ����㲻�����ϰ���
        if (startNode.NodeType == E_AStarNodeType.obstacable || endNode.NodeType == E_AStarNodeType.obstacable)
        { 
            Debug.Log("���Ϸ�����");
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
                        //8����
                        //GetNodeCheck(x, y, 1.4f, startNode, endNode);
                        //4����
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
                //�ҵ��յ��ˣ���
                List<AStarNode> correctNodes = new List<AStarNode>();
                correctNodes.Add(endNode);
                while (endNode.Father != null)
                {
                    correctNodes.Add(endNode.Father);
                    endNode = endNode.Father;
                }
                correctNodes.Reverse();
                //Debug.Log("Ѱ��·�������");
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
        //�����߽�
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
    /// �ҵ�������С�Ľڵ�
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
