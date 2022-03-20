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
    
    // TODO: Currently failing --
    [Theory]
    [InlineData(90, true, new[]
    {
        ActionIDs.KeenEdge, ActionIDs.NoMercy, ActionIDs.BrutalShell, ActionIDs.SolidBarrel, ActionIDs.GnashingFang, 
        ActionIDs.JugularRip, ActionIDs.SonicBreak, ActionIDs.Bloodfest, ActionIDs.DoubleDown, ActionIDs.BlastingZone, 
        ActionIDs.SavageClaw, ActionIDs.AbdomenTear, ActionIDs.WickedTalon, ActionIDs.EyeGouge, ActionIDs.BurstStrike,
        ActionIDs.Hypervelocity, ActionIDs.KeenEdge, ActionIDs.BrutalShell, ActionIDs.SolidBarrel, ActionIDs.GnashingFang
    })]
    public void Gunbreaker_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}