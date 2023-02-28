using System.Linq;
using Content.Server.GameTicking.Rules;
using Content.Server.Mind.Components;
using Content.Server.Objectives.Interfaces;
using Content.Shared.Mobs.Components;
using JetBrains.Annotations;
using Robust.Shared.Random;

namespace Content.Server.Objectives.Conditions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed class KillRandomTraitorCondition : KillPersonCondition
    {
        public override IObjectiveCondition GetAssigned(Mind.Mind mind)
        {
            {
                var entityMgr = IoCManager.Resolve<IEntityManager>();
                var traitors = entityMgr.EntitySysManager.GetEntitySystem<TraitorRuleSystem>().GetOtherTraitorsAliveAndConnected(mind).ToList();

                if (traitors.Count == 0) return new EscapeShuttleCondition{}; //You were made a traitor by admins, and are the first/only.
                return new KillRandomTraitorCondition { Target = IoCManager.Resolve<IRobustRandom>().Pick(traitors).Mind };
            }

        }
    }
}
