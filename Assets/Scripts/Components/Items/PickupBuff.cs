using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBuff : PickupItem
{
    //[SerializeField] private List<CharacterStats> statsModifier;
    protected override void OnPickedUp(GameObject receiver)
    {
        //CharacterStatsHandler statsHandler = receiver.GetComponent<CharacterStatsHandler>();
        //foreach (CharacterStats stat in statsModifier)
        //{
        //    statsHandler.AddStatModifier(stat);
        //}
        // 스탯 추가하는 코드 넣기 (2초후에 다시 스탯수정리스트에서 제거)
    }
}
