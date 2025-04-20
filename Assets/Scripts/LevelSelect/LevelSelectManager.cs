using UnityEngine;

public class LevelSelectManager : MonoSingleton<LevelSelectManager>
{
    UIManager uiManager;
    protected override void InitSelf()
    {
        base.InitSelf();
        if (uiManager == null)
            uiManager = UIManager.Instance;
    }
    public void LockRemindShow()
    {
        uiManager.ShowPanel<LockPanel>(null, true);
    }
}
