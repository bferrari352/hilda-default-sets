using System.Linq;
using Hilda.Conductors.JobDefinitions.Tanks;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.Tanks;

public class DefaultSetTestsGNB : DefaultSetTestBase
{
    public DefaultSetTestsGNB(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeGNB();
        JobDefinition = new TestJobDefinitionGNB(new TestJobGaugeGNB((JobGaugeGNB) JobGauge));
        
        QueueSize = 20;

        var sets = GetDefaultSets(JobData.Gunbreaker)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }
    
    [Theory]
    [InlineData(90, true, new[]
    {
        ActionIDs.KeenEdge, ActionIDs.NoMercy,
        ActionIDs.BrutalShell, ActionIDs.BlastingZone,
        ActionIDs.SolidBarrel, ActionIDs.BowShock,
        ActionIDs.GnashingFang, ActionIDs.JugularRip,
        ActionIDs.SonicBreak, ActionIDs.Bloodfest, 
        ActionIDs.DoubleDown,
        ActionIDs.SavageClaw, ActionIDs.AbdomenTear,
        ActionIDs.WickedTalon, ActionIDs.EyeGouge,
        ActionIDs.BurstStrike, ActionIDs.Hypervelocity,
        ActionIDs.KeenEdge,
        ActionIDs.BrutalShell,
        ActionIDs.SolidBarrel
    })]
    [InlineData(80, true, new[]
    {
        ActionIDs.KeenEdge, ActionIDs.NoMercy,
        ActionIDs.BrutalShell, ActionIDs.BlastingZone,
        ActionIDs.SolidBarrel, ActionIDs.BowShock,
        ActionIDs.GnashingFang, ActionIDs.JugularRip,
        ActionIDs.SonicBreak, ActionIDs.Bloodfest, 
        ActionIDs.BurstStrike,
        ActionIDs.SavageClaw, ActionIDs.AbdomenTear,
        ActionIDs.WickedTalon, ActionIDs.EyeGouge,
        ActionIDs.BurstStrike,
        ActionIDs.KeenEdge,
        ActionIDs.BrutalShell,
        ActionIDs.SolidBarrel,
        ActionIDs.BurstStrike
    })]
    [InlineData(70, true, new[]
    {
        ActionIDs.KeenEdge, ActionIDs.NoMercy,
        ActionIDs.BrutalShell, ActionIDs.DangerZone,
        ActionIDs.SolidBarrel, ActionIDs.BowShock,
        ActionIDs.GnashingFang, ActionIDs.JugularRip,
        ActionIDs.SonicBreak,
        ActionIDs.SavageClaw, ActionIDs.AbdomenTear,
        ActionIDs.WickedTalon, ActionIDs.EyeGouge,
        ActionIDs.KeenEdge,
        ActionIDs.BrutalShell,
        ActionIDs.SolidBarrel,
        ActionIDs.BurstStrike,
        ActionIDs.KeenEdge,
        ActionIDs.BrutalShell,
        ActionIDs.SolidBarrel
    })]
    [InlineData(60, true, new[]
    {
        ActionIDs.KeenEdge, ActionIDs.NoMercy,
        ActionIDs.BrutalShell, ActionIDs.DangerZone,
        ActionIDs.SolidBarrel,
        ActionIDs.GnashingFang,
        ActionIDs.SonicBreak,
        ActionIDs.SavageClaw,
        ActionIDs.WickedTalon,
        ActionIDs.KeenEdge,
        ActionIDs.BrutalShell,
        ActionIDs.SolidBarrel,
        ActionIDs.BurstStrike,
        ActionIDs.KeenEdge,
        ActionIDs.BrutalShell,
        ActionIDs.SolidBarrel,
        ActionIDs.BurstStrike, ActionIDs.DangerZone,
        ActionIDs.KeenEdge,
        ActionIDs.BrutalShell
    })]
    [InlineData(50, true, new[]
    {
        ActionIDs.KeenEdge, ActionIDs.NoMercy,
        ActionIDs.BrutalShell, ActionIDs.DangerZone,
        ActionIDs.SolidBarrel,
        ActionIDs.BurstStrike,
        ActionIDs.KeenEdge,
        ActionIDs.BrutalShell,
        ActionIDs.SolidBarrel,
        ActionIDs.BurstStrike,
        ActionIDs.KeenEdge,
        ActionIDs.BrutalShell,
        ActionIDs.SolidBarrel,
        ActionIDs.BurstStrike,
        ActionIDs.KeenEdge,
        ActionIDs.BrutalShell,
        ActionIDs.SolidBarrel, ActionIDs.DangerZone,
        ActionIDs.BurstStrike,
        ActionIDs.KeenEdge
    })]
    public void Gunbreaker_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}