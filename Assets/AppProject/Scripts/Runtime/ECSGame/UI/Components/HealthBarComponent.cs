using ECSCore;
using UnityEngine;
using Game;

namespace ECSGame.UI
{
    [DisallowMultipleComponent]
    public sealed class HealthBarComponent : EntityActorComponent
    {
        [SerializeField]HealthBar m_bar;
        public HealthBar Bar => m_bar;
    };
}
