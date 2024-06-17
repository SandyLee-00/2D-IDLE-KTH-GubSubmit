using UnityEngine;

public class AuotManager : SingletonMonoBehaviour<AuotManager>
{
    public bool IsAutoModeOn { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    public void AutoOnOffAbleCheck()
    {
        if (ShopManager.Instance.IsAutoAttackMode)
        {
            OnOff();
        }
        else
        {
            ErrorMessageText.Instance.ShowErrorMessage("���� ��带 �����ϼ���");
        }
    }

    private void OnOff()
    {
        if (!IsAutoModeOn)
        {
            IsAutoModeOn = true;
            Debug.Log("������ Ȱ��ȭ");
            ErrorMessageText.Instance.ShowErrorMessage("������ Ȱ��ȭ");
        }
        else
        {
            IsAutoModeOn = false;
            ErrorMessageText.Instance.ShowErrorMessage("������ ��Ȱ��ȭ");
        }
    }
}
