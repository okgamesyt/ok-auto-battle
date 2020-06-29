using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Manager<PlayerData>
{
    public int Money { get; private set; }

    public System.Action OnUpdate;

    private void Start()
    {
        Money = 250;
    }

    public bool CanAfford(int amount)
    {
        return amount <= Money;
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
        OnUpdate?.Invoke();
    }
}
