using UnityEngine;

public class DropManager : SingletonMonoBehaviour<DropManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        EnemyCharacter.OnEnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDisable()
    {
        EnemyCharacter.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void HandleEnemyDestroyed()
    {
        // �÷��̾� ĳ���͸� ã��, ���� ��� �޼��� ȣ��
        if (PlayerCharacter.Instance != null)
        {
            DropRewards(PlayerCharacter.Instance);
        }
    }

    public void DropRewards(PlayerCharacter player)
    {
        int goldAmount = CalculateGold();
        int experienceAmount = CalculateExperience();

        player.AddGold(goldAmount);
        player.AddExperience(experienceAmount);
    }


    private int CalculateGold()
    {
        // �������� ������ ����Ͽ� �ּ�ġ�� �ִ�ġ ����
        int minGold = StageManager.Instance.stageNum * 50;
        int maxGold = StageManager.Instance.stageNum * 100;

        return Random.Range(minGold, maxGold);
    }

    private int CalculateExperience()
    {
        int minExp = StageManager.Instance.stageNum * 10;
        int maxExp = StageManager.Instance.stageNum * 20;

        return Random.Range(minExp, maxExp);
    }
}
