using System.Linq;
using Hilda.Conductors.JobDefinitions.Tanks;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.Tanks;

public class DefaultSetTestsWAR : DefaultSetTestBase
{
    public DefaultSetTestsWAR(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeWAR();
        JobDefinition = new TestJobDefinitionWAR(new TestJobGaugeWAR((JobGaugeWAR) JobGauge));
        
        QueueSize = 14;

        var sets = GetDefaultSets(JobData.Warrior)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }

    [Theory]
    [InlineData(90, true, new[] {ActionIDs.HeavySwing, ActionIDs.Infuriate, ActionIDs.Maim, ActionIDs.Upheaval, ActionIDs.StormsEye,
        ActionIDs.InnerRelease, ActionIDs.InnerChaos, ActionIDs.PrimalRend, ActionIDs.FellCleave, ActionIDs.FellCleave,
        ActionIDs.FellCleave, ActionIDs.Infuriate, ActionIDs.InnerChaos, ActionIDs.HeavySwing})]
    [InlineData(80, true, new[] {ActionIDs.HeavySwing, ActionIDs.Infuriate, ActionIDs.Maim, ActionIDs.Upheaval, ActionIDs.StormsEye,
        ActionIDs.InnerRelease, ActionIDs.InnerChaos, ActionIDs.FellCleave, ActionIDs.FellCleave,
        ActionIDs.FellCleave, ActionIDs.Infuriate, ActionIDs.InnerChaos, ActionIDs.HeavySwing, ActionIDs.Maim})]
    [InlineData(70, true, new[] {ActionIDs.HeavySwing, ActionIDs.Infuriate, ActionIDs.Maim, ActionIDs.Upheaval, ActionIDs.StormsEye,
        ActionIDs.InnerRelease, ActionIDs.FellCleave, ActionIDs.FellCleave, ActionIDs.FellCleave, ActionIDs.FellCleave,
        ActionIDs.Infuriate, ActionIDs.FellCleave, ActionIDs.HeavySwing, ActionIDs.Maim})]
    [InlineData(60, true, new[] {ActionIDs.HeavySwing, ActionIDs.Infuriate, ActionIDs.Maim, ActionIDs.Berserk, ActionIDs.FellCleave,
        ActionIDs.Infuriate, ActionIDs.FellCleave, ActionIDs.StormsEye, ActionIDs.HeavySwing, ActionIDs.Maim, ActionIDs.StormsEye,
        ActionIDs.HeavySwing, ActionIDs.Maim, ActionIDs.StormsEye})]
    [InlineData(50, true, new[] {ActionIDs.HeavySwing, ActionIDs.Infuriate, ActionIDs.Maim, ActionIDs.Berserk, ActionIDs.InnerBeast,
        ActionIDs.Infuriate, ActionIDs.InnerBeast, ActionIDs.StormsEye, ActionIDs.HeavySwing, ActionIDs.Maim, ActionIDs.StormsEye,
        ActionIDs.HeavySwing, ActionIDs.Maim, ActionIDs.StormsEye})]
    public void Warrior_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}