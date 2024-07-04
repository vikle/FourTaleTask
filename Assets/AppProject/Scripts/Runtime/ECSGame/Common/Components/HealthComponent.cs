using ECSCore;
using UnityEngine;

namespace ECSGame
{
    [DisallowMultipleComponent]
    public sealed class HealthComponent : EntityActorComponent
    {
        [Range(0f, 1000f)]public float maxHealth = 1000f;
        [Range(0f, 1000f)]public float currentHealth = 1000f;
    };
}
