using Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Attacks
{
    public class AttackStatusHandler : MonoBehaviour
    {
        public AttackStatus attackStatus;

        public void SetStatus(AttackStatus status, AttackController controller)
        {
            if (!controller.isActiveAndEnabled) return;
            
            attackStatus = status;
        }
    }
}