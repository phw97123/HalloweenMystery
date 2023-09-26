using Components.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSystem : MonoBehaviour //OwnedGold에서 골드 사용, OwnedGold에서 StoredGold으로 골드 보내기, StoredGold에서 골드 사용 만 가능
{
    public int OwnedGold { get; private set; }
    public int StoredGold { get; private set; }

    public event Action OnChangeOwnedGold; //나중에 UI매니저에서 이것도 연결하기
    public event Action OnChangeStoredGold;

    private void Start()
    {
        OwnedGold = 0;
    }

    public bool Deposit(int change) // OwnedGold에서 StoredGold으로 골드 보내기
    {
        if (ChangeOwnedGold(-change))
        {
            ChangeStoredGold(change);
            return true;
        }
        return false;
    }

    public bool ChangeOwnedGold(int change) // OwnedGold에서 골드 사용할 때는 음수값 대입, 골드아이템 먹었을 때는 양수값 대입하면 됨
    {
        if (change == 0 || OwnedGold < -change)
            return false;

        OwnedGold += change;
        OnChangeOwnedGold?.Invoke();
        return true;
    }

    public bool ChangeStoredGold(int change) // StoredGold에서 골드 사용할 때 음수값 대입
    {
        if (change == 0 || StoredGold < -change)
            return false;

        StoredGold += change;
        OnChangeStoredGold?.Invoke();
        return true;
    }
}
