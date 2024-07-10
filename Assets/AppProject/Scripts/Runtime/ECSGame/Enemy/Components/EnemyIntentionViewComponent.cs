using UnityEngine;
using ECSCore;
using TMPro;

namespace ECSGame
{
    [DisallowMultipleComponent]
    public sealed class EnemyIntentionViewComponent : EntityActorComponent
    {
        public TMP_Text text;
    };
}
