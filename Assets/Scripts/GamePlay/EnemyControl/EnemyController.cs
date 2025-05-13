using UnityEngine;

public class EnemyController : MonoBehaviour
{ 
    Player player;

    private void Start()
    {
        player ??= FindAnyObjectByType<Player>();
    }

    private void Update()
    {
       
    }

}
