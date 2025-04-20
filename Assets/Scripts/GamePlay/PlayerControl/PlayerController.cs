using UnityEngine;
public class PlayerController : MonoBehaviour
{ 
    public Player player;
    private P_MoveCommand _moveCommand;
    private P_ColorationCommand _colorationCommand;

    [Header("�ƶ�������Ч")]
    public ParticleSystem movePartical;

    void Start()
    {
        player ??= GetComponent<Player>();
        _moveCommand = new P_MoveCommand(player);
        _colorationCommand = new P_ColorationCommand(player);
    }

    void Update()
    {
        if (player.CanAct)
        {
            DoMove();
            DoCorloration();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * player.targetSquareChecckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * player.targetSquareChecckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * player.targetSquareChecckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * player.targetSquareChecckDistance);
    }

    private void DoMove()
    {
        if (PlayerInputManager.Instance.MoveUp)
        {
            if(player.isSwaping)
                return;
            player.targetDir = E_TargetDir.��;
            movePartical.transform.eulerAngles = new Vector3(90,-90,-90);
            movePartical?.Play();
            _moveCommand.Excute();
        }
        else if (PlayerInputManager.Instance.MoveDown)
        {
            player.targetDir = E_TargetDir.��;
            movePartical.transform.eulerAngles = new Vector3(-90, -90, -90);
            movePartical?.Play();
            _moveCommand.Excute();
        }
        else if (PlayerInputManager.Instance.MoveLeft)
        {
            player.targetDir = E_TargetDir.��;
            movePartical.transform.eulerAngles = new Vector3(180, -90, -90);
            movePartical?.Play();
            _moveCommand.Excute();
        }
        else if (PlayerInputManager.Instance.MoveRight)
        {
            player.targetDir = E_TargetDir.��;
            movePartical.transform.eulerAngles = new Vector3(0, -90, -90);
            movePartical?.Play();
            _moveCommand.Excute();
        }
    }
    
    private void DoCorloration()
    {
        if (PlayerInputManager.Instance.ColorationUp)
        {
            player.targetDir = E_TargetDir.��;
            _colorationCommand.Excute();
        }
        else if (PlayerInputManager.Instance.ColorationDown)
        {
            player.targetDir = E_TargetDir.��;
            _colorationCommand.Excute();
        }
        else if (PlayerInputManager.Instance.ColorationLeft)
        {
            player.targetDir = E_TargetDir.��;
            _colorationCommand.Excute();
        }
        else if (PlayerInputManager.Instance.ColorationRight)
        {
            player.targetDir = E_TargetDir.��;
            _colorationCommand.Excute();        
        }
    }
}
