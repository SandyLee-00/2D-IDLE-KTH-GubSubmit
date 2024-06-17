using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : SingletonMonoBehaviour<ShopManager>
{
    public bool IsAutoGoldActive
    {
        get { return PlayerCharacter.Instance.IsAutoGoldActive; }
        private set { PlayerCharacter.Instance.IsAutoGoldActive = value; }
    }
    public bool IsAutoAttackMode
    {
        get { return PlayerCharacter.Instance.IsAutoAttackMode; }
        private set { PlayerCharacter.Instance.IsAutoAttackMode = value; }
    }

    [SerializeField] private GameObject destroyButtonAutoGoldBtn;
    [SerializeField] private GameObject destroyButtonAutoAttackBtn;

    protected override void Awake()
    {
        base.Awake();
    }

    public void BuyAutoGoldMachine()
    {
        if (PlayerCharacter.Instance.IsAutoGoldActive)
        {
            ErrorMessageText.Instance.ShowErrorMessage("�̹� ���� �߽��ϴ�");
            destroyButtonAutoGoldBtn.SetActive(false); // ��ư ��Ȱ��ȭ
            return;
        }

        if (!PlayerCharacter.Instance.IsAutoGoldActive && PlayerCharacter.Instance.GetGold() >= 30000)
        {
            PlayerCharacter.Instance.AddGold(-30000);
            IsAutoGoldActive = true;
            destroyButtonAutoGoldBtn.SetActive(false); // ��ư ��Ȱ��ȭ
        }
        else
        {
            ErrorMessageText.Instance.ShowErrorMessage("��尡 �����մϴ�.");
        }
    }

    public void BuyAutoAttackMod()
    {
        if (PlayerCharacter.Instance.IsAutoAttackMode)
        {
            ErrorMessageText.Instance.ShowErrorMessage("�̹� ���� �߽��ϴ�");
            destroyButtonAutoAttackBtn.SetActive(false); // ��ư ��Ȱ��ȭ
            return;
        }

        if (!PlayerCharacter.Instance.IsAutoAttackMode && PlayerCharacter.Instance.GetGold() >= 300000)
        {
            PlayerCharacter.Instance.AddGold(-300000);
            IsAutoAttackMode = true;
            destroyButtonAutoAttackBtn.SetActive(false); // ��ư ��Ȱ��ȭ
        }
        else
        {
            ErrorMessageText.Instance.ShowErrorMessage("��尡 �����մϴ�.");
        }
    }
}
