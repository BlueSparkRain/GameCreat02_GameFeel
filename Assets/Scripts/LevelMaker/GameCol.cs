using UnityEngine;

public class GameCol : MonoBehaviour
{
    [Header("���в���")]
    [SerializeField] private int colSlotNum;

    [Header("���з�����")]
    [SerializeField] private Transform colSpawner;

    [Header("������������")]
    public bool isColFull;

    [Header("������󲹳�����")]
    public int maxSpawnerCapcity;


#if UNITY_EDITOR
    [ContextMenu("Ϊ�����д���Ԥ�Ʋ�")]
    void CreatCol() 
    {
        Vector3  firstSlotPos = colSpawner.position;
        for (int i = 0; i < colSlotNum; i++) 
        {
         //�������У����ڲۼ������Ϊ2
         GameObject newSlot= Instantiate(Resources.Load<GameObject>("Prefab/Slot"), firstSlotPos+new Vector3(0,-2,0)*(i+1), Quaternion.identity,transform );
        }
    }
#endif

    private void Start()
    {
        
    }







}
