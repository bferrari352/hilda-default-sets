using System.Linq;
using Hilda.Conductors.JobDefinitions.MeleeDps;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.MeleeDPS;

public class DefaultSetTestsDRG : DefaultSetTestBase
{
    public DefaultSetTestsDRG(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeDRG();
        JobDefinition = new TestJobDefinitionDRG(new TestJobGaugeDRG((JobGaugeDRG) JobGauge));

        var sets = GetDefaultSets(JobData.Dragoon)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }

    [Theory]
    [InlineData(90, true, new[]
    {
        ActionIDs.TrueThrust, ActionIDs.LanceCharge,
        ActionIDs.Disembowel, ActionIDs.DragonSight,
        ActionIDs.ChaoticSpring, ActionIDs.Geirskogul,
        ActionIDs.WheelingThrust, ActionIDs.LifeSurge,
        ActionIDs.FangAndClaw, ActionIDs.HighJump,
        ActionIDs.RaidenThrust, ActionIDs.DragonfireDive,
        ActionIDs.VorpalThrust, ActionIDs.MirageDive,
        ActionIDs.HeavensThrust, ActionIDs.LifeSurge,
        ActionIDs.FangAndClaw, ActionIDs.SpineshatterDive,
        ActionIDs.WheelingThrust, ActionIDs.SpineshatterDive,
        ActionIDs.RaidenThrust, ActionIDs.WyrmwindThrust,
        ActionIDs.Disembowel,
        ActionIDs.ChaoticSpring,
        ActionIDs.WheelingThrust,
        ActionIDs.FangAndClaw,
        ActionIDs.RaidenThrust
    })]
    [InlineData(80, true, new[]
    {
        ActionIDs.TrueThrust, ActionIDs.LanceCharge,
        ActionIDs.Disembowel, ActionIDs.DragonSight,
        ActionIDs.ChaosThrust, ActionIDs.Geirskogul,
        ActionIDs.WheelingThrust, ActionIDs.LifeSurge,
        ActionIDs.FangAndClaw, ActionIDs.HighJump,
        ActionIDs.RaidenThrust, ActionIDs.DragonfireDive,
        ActionIDs.VorpalThrust, ActionIDs.MirageDive,
        ActionIDs.FullThrust, ActionIDs.SpineshatterDive,
        ActionIDs.FangAndClaw,
        ActionIDs.WheelingThrust,
        ActionIDs.RaidenThrust
    })]
    [InlineData(70, true, new[]
    {
        ActionIDs.TrueThrust, ActionIDs.LanceCharge,
        ActionIDs.Disembowel, ActionIDs.DragonSight,
        ActionIDs.ChaosThrust, ActionIDs.Geirskogul,
        ActionIDs.WheelingThrust, ActionIDs.LifeSurge,
        ActionIDs.FangAndClaw, ActionIDs.Jump,
        ActionIDs.TrueThrust, ActionIDs.DragonfireDive,
        ActionIDs.VorpalThrust, ActionIDs.MirageDive,
        ActionIDs.FullThrust, ActionIDs.SpineshatterDive,
        ActionIDs.FangAndClaw,
        ActionIDs.WheelingThrust,
        ActionIDs.TrueThrust
    })]
    [InlineData(60, true, new[]
    {
        ActionIDs.TrueThrust, ActionIDs.LanceCharge,
        ActionIDs.Disembowel, ActionIDs.Geirskogul,
        ActionIDs.ChaosThrust, ActionIDs.Jump,
        ActionIDs.WheelingThrust, ActionIDs.DragonfireDive,
        ActionIDs.TrueThrust, ActionIDs.SpineshatterDive,
        ActionIDs.VorpalThrust,
        ActionIDs.FullThrust, ActionIDs.LifeSurge,
        ActionIDs.FangAndClaw,
        ActionIDs.TrueThrust
    })]
    [InlineData(50, true, new[]
    {
        ActionIDs.TrueThrust, ActionIDs.LanceCharge,
        ActionIDs.Disembowel, ActionIDs.Jump,
        ActionIDs.ChaosThrust, ActionIDs.DragonfireDive,
        ActionIDs.TrueThrust, ActionIDs.SpineshatterDive,
        ActionIDs.VorpalThrust, ActionIDs.LifeSurge,
        ActionIDs.FullThrust,
        ActionIDs.TrueThrust
    })]
    public void Dragoon_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}