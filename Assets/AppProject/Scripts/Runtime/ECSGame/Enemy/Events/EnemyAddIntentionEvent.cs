using ECSCore;
using Game;

namespace ECSGame
{
    public sealed class EnemyAddIntentionEvent : IEvent
    {
        public CardEffect effect;
    };
}
