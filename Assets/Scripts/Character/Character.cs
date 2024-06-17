using System.Numerics;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    private string characterName;
    private float maxHealth;
    private float currentHealth;
    private BigInteger attackPower;

    [SerializeField] private CharacterStat stat;

    public string CharacterName
    {
        get { return characterName; }
    }
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public BigInteger AttackPower
    {
        get { return attackPower; }
        set { attackPower = value; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }


    private void Awake()
    {
        StatInitialize();
    }

    private void StatInitialize()
    {
        if (stat != null)
        {
            // ĳ���� ���� �ʱ�ȭ
            characterName = stat.characterName;
            maxHealth = stat.maxHealth;
            currentHealth = stat.maxHealth;
            attackPower = new BigInteger(stat.attackPower);
        }
    }
    public string GetFormattedAttackPower()
    {
        return FormatBigInteger(attackPower);
    }

    private string FormatBigInteger(BigInteger value)
    {
        if (value < 10000)
        {
            return value.ToString();
        }

        int suffixIndex = 0;
        BigInteger divisor = new BigInteger(10000);

        while (value >= divisor && suffixIndex < suffixes.Length - 1)
        {
            value /= divisor;
            suffixIndex++;
        }

        string formattedValue = value.ToString("F2");
        return $"{formattedValue}{suffixes[suffixIndex]}";
    }

    private static readonly string[] suffixes = {
        "", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "���ϻ�",
        "�ƽ���", "����Ÿ", "�Ұ�����", "�������", "��", "��"
    };
    public void SetAttackPowerFromString(string attackPowerString)
    {
        attackPower = BigInteger.Parse(attackPowerString);
    }
}
