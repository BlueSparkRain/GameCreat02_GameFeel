using UnityEngine;

public class GameCol : MonoBehaviour
{
    [Header("本列槽数")]
    [SerializeField] private int colSlotNum;

    [Header("本列孵化器")]
    [SerializeField] private Transform colSpawner;

    [Header("本列容量已满")]
    public bool isColFull;

    [Header("本列最大补充容量")]
    public int maxSpawnerCapcity;


#if UNITY_EDITOR
    [ContextMenu("为本子列创建预制槽")]
    void CreatCol() 
    {
        Vector3  firstSlotPos = colSpawner.position;
        for (int i = 0; i < colSlotNum; i++) 
        {
         //纵向排列，相邻槽间隔距离为2
         GameObject newSlot= Instantiate(Resources.Load<GameObject>("Prefab/Slot"), firstSlotPos+new Vector3(0,-2,0)*(i+1), Quaternion.identity,transform );
        }
    }
#endif

    private void Start()
    {
        
    }







}
