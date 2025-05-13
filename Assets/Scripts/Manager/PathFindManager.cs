using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFindManager : MonoSingleton<PathFindManager>
{
    #region Ѱ·Info
    [Header("Ѱ·��Զ��")]
    public int PathFind_MaxCol;
    [Header("Ѱ·��Զ��")]
    public int PathFind_MaxRow;
    public E_AStarNodeType[,] slotType;
    #endregion

    AStarManager astarManager;

    SquareController playerSquareController;

    //���е���������Ѱ�ҵ�ͼ�ϵ�������Ϣ

    public Vector2Int GetAStarNodeIndex(SquareController controller) 
    {
       return controller.square.slot.NodeIndex;
    } 
    
    public Vector2Int GetPlayerNodeIndex() 
    {
       playerSquareController ??= FindAnyObjectByType<PlayerController>().GetComponent<SquareController>();
       return playerSquareController.square.slot.NodeIndex;
    }

    protected override void InitSelf()
    {
        base.InitSelf();
        astarManager = AStarManager.Instance;
    }

    public List<E_CustomDir> GetPathFindCommands(Vector2Int startPos, Vector2Int endPos)
    {
        List<E_CustomDir> commandDirs = new List<E_CustomDir>();

        List<AStarNode> nodes = astarManager.FindPath(startPos, endPos);
        if (nodes != null)
        {
            for (int i = 1; i < nodes.Count; ++i)
            {
                if (nodes[i].X > nodes[i - 1].X)
                    commandDirs.Add(E_CustomDir.��);
                else if (nodes[i].X < nodes[i - 1].X)
                    commandDirs.Add(E_CustomDir.��);
                else if (nodes[i].Y > nodes[i - 1].Y)
                    commandDirs.Add(E_CustomDir.��);
                else
                    commandDirs.Add(E_CustomDir.��);
                //Debug.Log(commandDirs.Last());
            }

        }
        //Debug.Log("·�������б�"+ commandDirs.Count);
        return commandDirs;

    }

    private void Update()
    {
      
    }

}
