using System.Linq;
using Content.Server.Mind.Components;
using Content.Server.Objectives.Interfaces;
using Content.Shared.Mobs.Components;
using JetBrains.Annotations;
using Robust.Shared.Random;

namespace Content.Server.Objectives.Conditions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed class MaroonRandomPersonCondition : MaroonPersonCondition
    {
        public override IObjectiveCondition GetAssigned(Mind.Mind mind)
        {
            var allHumans = EntityManager.EntityQuery<MindComponent>(true).Where(mc =>
            {
                var entity = mc.Mind?.OwnedEntity;

                if (entity == default)
                    return false;

                return EntityManager.TryGetComponent(entity, out MobStateComponent? mobState) &&
                      MobStateSystem.IsAlive(entity.Value, mobState) &&
                       mc.Mind != mind;
            }).Select(mc => mc.Mind).ToList();

            if (allHumans.Count == 0)
                return new DieCondition();

            return new MaroonRandomPersonCondition {Target = IoCManager.Resolve<IRobustRandom>().Pick(allHumans)};
        }
    }
}
