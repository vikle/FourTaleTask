using UnityEngine;
using ECSCore;

namespace ECSGame.UI
{
    [DisallowMultipleComponent]
    public sealed class DefenceBarComponent : EntityActorComponent
    {
        public SpriteProgressBar bar;
    };
}
