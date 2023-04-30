using Content.Server.Objectives.Interfaces;
using Content.Server.Station.Components;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Map.Components;
using Robust.Shared.Utility;

namespace Content.Server.Objectives.Conditions
{
    public abstract class MaroonPersonCondition : IObjectiveCondition
    {
        protected IEntityManager EntityManager => IoCManager.Resolve<IEntityManager>();
        protected MobStateSystem MobStateSystem => EntityManager.EntitySysManager.GetEntitySystem<MobStateSystem>();
        protected Mind.Mind? Target;
        public abstract IObjectiveCondition GetAssigned(Mind.Mind mind);

        public string Title
        {
            get
            {
                var targetName = string.Empty;
                var jobName = Target?.CurrentJob?.Name ?? "Unknown";

                if (Target == null)
                    return Loc.GetString("objective-condition-maroon-person-title", ("targetName", targetName), ("job", jobName));

                if (Target.OwnedEntity is {Valid: true} owned)
                    targetName = EntityManager.GetComponent<MetaDataComponent>(owned).EntityName;

                return Loc.GetString("objective-condition-maroon-person-title", ("targetName", targetName), ("job", jobName));
            }
        }

        public string Description => Loc.GetString("objective-condition-maroon-person-description");

        public SpriteSpecifier Icon => new SpriteSpecifier.Rsi(new ResourcePath("Objects/Specific/Medical/Morgue/bodybags.rsi"), "bag_folded");

        private bool IsTargetOnShuttle(TransformComponent targetXform, EntityUid? shuttle)
        {
            if (shuttle == null)
                return false;

            var entMan = IoCManager.Resolve<IEntityManager>();

            if (!entMan.TryGetComponent<MapGridComponent>(shuttle, out var shuttleGrid) ||
                !entMan.TryGetComponent<TransformComponent>(shuttle, out var shuttleXform))
            {
                return false;
            }

            return shuttleXform.WorldMatrix.TransformBox(shuttleGrid.LocalAABB).Contains(targetXform.WorldPosition);
        }
        public float Progress
        {
            get {
                var entMan = IoCManager.Resolve<IEntityManager>();

                if (Target?.OwnedEntity == null
                    || !entMan.TryGetComponent<TransformComponent>(Target.OwnedEntity, out var xform))
                    return 0f;

                var shuttleContainsTarget = false;
                var targetIsAlive = !Target.CharacterDeadIC;
                var targetIsEscaping = true;

                // Any emergency shuttle counts for this objective.
                foreach (var stationData in entMan.EntityQuery<StationDataComponent>())
                {
                    if (IsTargetOnShuttle(xform, stationData.EmergencyShuttle))
                    {
                        shuttleContainsTarget = true;
                        break;
                    }
                }

                if (Target.CharacterDeadIC)
                    return 1f;

                if (!shuttleContainsTarget)
                    return 1f;

                if (shuttleContainsTarget && !targetIsEscaping)
                    return 1f;

                return (shuttleContainsTarget && targetIsAlive && targetIsEscaping) ? 0f : 1f;
            }
        }

        public float Difficulty => 1.5f;

        public bool Equals(IObjectiveCondition? other)
        {
            return other is MaroonPersonCondition kpc && Equals(Target, kpc.Target);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MaroonPersonCondition) obj);
        }

        public override int GetHashCode()
        {
            return Target?.GetHashCode() ?? 0;
        }
    }
}
