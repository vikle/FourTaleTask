using UnityEngine;
using ECSCore;

namespace ECSGame.UI
{
    [DisallowMultipleComponent]
    public sealed class HealthBarComponent : EntityActorComponent
    {
        public SpriteProgressBar bar;
    };
}
