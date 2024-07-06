using ECSCore;

namespace ECSGame
{
    public sealed class AnimationDataComponent : IComponent
    {
        public int IdleHash { get; set; }
        public int AttackHash { get; set; }
        public int DefenceHash { get; set; }
        public int HealHash { get; set; }
        public int DeathHash { get; set; }
    };
}
