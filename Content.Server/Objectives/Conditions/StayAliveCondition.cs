using Content.Server.Mind.Components;
using Content.Server.Objectives.Interfaces;
using JetBrains.Annotations;
using Robust.Shared.Utility;

namespace Content.Server.Objectives.Conditions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed class StayAliveCondition : IObjectiveCondition
    {
        private Mind.Mind? _mind;

        public IObjectiveCondition GetAssigned(Mind.Mind mind)
        {
            return new StayAliveCondition
            {
                _mind = mind,
            };
        }

        public string Title => Loc.GetString("objective-condition-stay-alive-title");

        public string Description => Loc.GetString("objective-condition-stay-alive-description");

        public SpriteSpecifier Icon => new SpriteSpecifier.Rsi(new ResPath("Objects/Misc/bureaucracy.rsi"), "folder-white");

        public float Progress
        {
            get
            {
                var entMan = IoCManager.Resolve<IEntityManager>();

                if (_mind?.OwnedEntity == null
                    || !entMan.TryGetComponent<MindComponent>(_mind.OwnedEntity, out _))
                    return 0f;

                var agentIsAlive = !_mind.CharacterDeadIC;

                return (agentIsAlive) ? 1f : 0f;
            }
        }

        public float Difficulty => 1.0f;

        public bool Equals(IObjectiveCondition? other)
        {
            return other is StayAliveCondition esc && Equals(_mind, esc._mind);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((StayAliveCondition) obj);
        }

        public override int GetHashCode()
        {
            return _mind != null ? _mind.GetHashCode() : 0;
        }
    }
}
