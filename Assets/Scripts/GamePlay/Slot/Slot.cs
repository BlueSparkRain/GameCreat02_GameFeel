using UnityEngine;

public class Slot : MonoBehaviour
{
    public E_SlotType type;
    public int mapIndex;
    public SubCol selfColumn;

    public Vector2Int NodeIndex;

    public virtual void Init(SubCol col) 
    {
        selfColumn=col;
    }

    public void GetPathFindIndex( Vector2Int nodeIndex) 
    {
        NodeIndex=nodeIndex;
    }
}
