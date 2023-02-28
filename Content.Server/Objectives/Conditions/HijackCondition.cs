using Content.Server.Cuffs.Components;
using Content.Server.Mind.Components;
using Content.Server.Objectives.Interfaces;
using Content.Server.Station.Components;
using Content.Server.Traitor;
using JetBrains.Annotations;
using Robust.Shared.Map.Components;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Utility;

namespace Content.Server.Objectives.Conditions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed class HijackCondition : IObjectiveCondition
    {
        private Mind.Mind? _mind;

        public IObjectiveCondition GetAssigned(Mind.Mind mind)
        {
            return new EscapeShuttleCondition {
                _mind = mind,
            };
        }

        public string Title => Loc.GetString("objective-condition-hijack-shuttle-title");

        public string Description => Loc.GetString("objective-condition-hijack-shuttle-description");

        public SpriteSpecifier Icon => new SpriteSpecifier.Rsi(new ResourcePath("Objects/Weapons/Melee/e_sword.rsi"), "icon");

        private bool IsAgentOnShuttle(TransformComponent agentXform, EntityUid? shuttle)
        {
            if (shuttle == null)
                return false;

            var entMan = IoCManager.Resolve<IEntityManager>();

            if (!entMan.TryGetComponent<MapGridComponent>(shuttle, out var shuttleGrid) ||
                !entMan.TryGetComponent<TransformComponent>(shuttle, out var shuttleXform))
            {
                return false;
            }

            return shuttleXform.WorldMatrix.TransformBox(shuttleGrid.LocalAABB).Contains(agentXform.WorldPosition);
        }

        private bool IsEveryoneAliveOnShuttleATraitor(TransformComponent traitorXform, EntityUid? shuttle)
        {
            if (shuttle == null)
                return false;

            var entMan = IoCManager.Resolve<IEntityManager>(<TraitorRole>(_mind.CharacterDeadIC));

            if (!entMan.TryGetComponent<MapGridComponent>(shuttle, out var shuttleGrid) ||
                !entMan.TryGetComponent<TransformComponent>(shuttle, out var shuttleXform))
            {
                return false;
            }
            if

            return shuttleXform.WorldMatrix.TransformBox(shuttleGrid.LocalAABB).Contains(traitorXform.WorldPosition);
        }
        public float Progress
        {
            get {
                var entMan = IoCManager.Resolve<IEntityManager>();

                if (_mind?.OwnedEntity == null
                    || !entMan.TryGetComponent<TransformComponent>(_mind.OwnedEntity, out var xform))
                    return 0f;

                var shuttleContainsAgent = false;
                var agentIsAlive = !_mind.CharacterDeadIC;
                var shuttleContainsOnlyTraitors = false;

                // Any emergency shuttle counts for this objective.
                foreach (var stationData in entMan.EntityQuery<StationDataComponent>())
                {
                    if (IsAgentOnShuttle(xform, stationData.EmergencyShuttle)) {
                        shuttleContainsAgent = true;
                        break;
                    }
                }

                foreach (var stationData in entMan.EntityQuery<StationDataComponent>())
                {
                    if (IsEveryoneAliveOnShuttleATraitor(xform, stationData.EmergencyShuttle)) {
                        shuttleContainsOnlyTraitors = true;
                        break;
                    }
                }
                return (shuttleContainsAgent && agentIsAlive && shuttleContainsOnlyTraitors) ? 1f : 0f;
            }
        }

        public float Difficulty => 5f;

        public bool Equals(IObjectiveCondition? other)
        {
            return other is HijackCondition esc && Equals(_mind, esc._mind);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((HijackCondition) obj);
        }

        public override int GetHashCode()
        {
            return _mind != null ? _mind.GetHashCode() : 0;
        }
    }
}
