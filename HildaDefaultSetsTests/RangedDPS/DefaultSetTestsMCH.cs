using System.Linq;
using FluentAssertions;
using Hilda.Conductors.JobDefinitions.RangedDps;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.RangedDPS;

public class DefaultSetTestsMCH : DefaultSetTestBase
{
    public DefaultSetTestsMCH(TestBaseFixture fixture, ITestOutputHelper output) :
        base(fixture, output)
    {
        JobGauge = new JobGaugeMCH();
        JobDefinition = new TestJobDefinitionMCH(new TestJobGaugeMCH((JobGaugeMCH)JobGauge));

        var sets = GetDefaultSets(JobData.Machinist)?.ToList();
        if (sets == null) return;

        SingleTarget = sets.FirstOrDefault(s => s.Name.Equals(DefaultSets.Get(DefaultSets.DisplayType.Single)))?.Priorities;
        MultiTarget = sets.FirstOrDefault(s => s.Name.Equals(DefaultSets.Get(DefaultSets.DisplayType.Multi)))?.Priorities;
    }

    [Theory]
    [InlineData(90, true, new[]
    {
        ActionIDs.Reassemble,
        ActionIDs.AirAnchor, ActionIDs.Reassemble, // Battery - 20
        ActionIDs.Chainsaw, ActionIDs.GaussRound, // Battery - 20
        ActionIDs.Drill, ActionIDs.Ricochet,
        ActionIDs.HeatedSplitShot, ActionIDs.GaussRound,
        ActionIDs.HeatedSlugShot, ActionIDs.Ricochet,
        ActionIDs.HeatedCleanShot, ActionIDs.BarrelStabilizer, // Battery - 10
        ActionIDs.HeatedSplitShot, ActionIDs.Wildfire,
        ActionIDs.HeatedSlugShot, ActionIDs.AutomatonQueen,
        ActionIDs.HeatedCleanShot, ActionIDs.Hypercharge, // Battery - 10
        ActionIDs.HeatBlast, ActionIDs.GaussRound,
        ActionIDs.HeatBlast, ActionIDs.Ricochet,
        ActionIDs.HeatBlast, ActionIDs.GaussRound,
        ActionIDs.HeatBlast, ActionIDs.Ricochet,
        ActionIDs.GaussRound, // this is a minor bug, two weaves (ricochet + gauss) are recommended in a row due to avoiding not showing heat blast when it can't actually be used
        ActionIDs.Drill, ActionIDs.Ricochet,
        ActionIDs.HeatedSplitShot,
        ActionIDs.HeatedSlugShot, ActionIDs.GaussRound,
        ActionIDs.HeatedCleanShot, ActionIDs.Ricochet,
    })]
    [InlineData(80, true, new[]
    {
        ActionIDs.Reassemble,
        ActionIDs.AirAnchor, ActionIDs.GaussRound,
        ActionIDs.Drill, ActionIDs.Ricochet,
        ActionIDs.HeatedSplitShot, ActionIDs.GaussRound,
        ActionIDs.HeatedSlugShot, ActionIDs.Ricochet,
        ActionIDs.HeatedCleanShot, ActionIDs.BarrelStabilizer,
        ActionIDs.HeatedSplitShot, ActionIDs.Wildfire,
        ActionIDs.HeatedSlugShot, ActionIDs.Hypercharge,
        ActionIDs.HeatBlast, ActionIDs.GaussRound,
        ActionIDs.HeatBlast, ActionIDs.Ricochet,
        ActionIDs.HeatBlast, ActionIDs.GaussRound,
        ActionIDs.HeatBlast, ActionIDs.Ricochet,
        ActionIDs.GaussRound,
        ActionIDs.Drill, ActionIDs.Ricochet,
        ActionIDs.HeatedCleanShot,
        ActionIDs.HeatedSplitShot,
        ActionIDs.HeatedSlugShot, ActionIDs.GaussRound,
        ActionIDs.HeatedCleanShot
    })]
    [InlineData(70, true, new[]
    {
        ActionIDs.Reassemble,
        ActionIDs.Drill, ActionIDs.GaussRound,
        ActionIDs.HotShot, ActionIDs.Ricochet,
        ActionIDs.HeatedSplitShot, ActionIDs.BarrelStabilizer,
        ActionIDs.HeatedSlugShot, ActionIDs.Wildfire,
        ActionIDs.HeatedCleanShot, ActionIDs.Hypercharge,
        ActionIDs.HeatBlast, ActionIDs.GaussRound,
        ActionIDs.HeatBlast, ActionIDs.Ricochet,
        ActionIDs.HeatBlast, ActionIDs.GaussRound,
        ActionIDs.HeatBlast, ActionIDs.Ricochet,
        ActionIDs.GaussRound,
        ActionIDs.Drill, ActionIDs.Ricochet,
        ActionIDs.HeatedSplitShot,
        ActionIDs.HeatedSlugShot,
        ActionIDs.HeatedCleanShot,
        ActionIDs.HeatedSplitShot,
        ActionIDs.HeatedSlugShot, ActionIDs.GaussRound
    })]
    [InlineData(60, true, new[]
    {
        ActionIDs.Reassemble,
        ActionIDs.Drill, ActionIDs.GaussRound,
        ActionIDs.HotShot, ActionIDs.Ricochet,
        ActionIDs.HeatedSplitShot, ActionIDs.GaussRound,
        ActionIDs.HeatedSlugShot, ActionIDs.Ricochet,
        ActionIDs.CleanShot,
        ActionIDs.HeatedSplitShot,
        ActionIDs.HeatedSlugShot,
        ActionIDs.CleanShot,
        ActionIDs.Drill,
        ActionIDs.HeatedSplitShot,
        ActionIDs.HeatedSlugShot,
        ActionIDs.CleanShot, ActionIDs.RookAutoturret,
        ActionIDs.HeatedSplitShot, ActionIDs.Wildfire,
        ActionIDs.HeatedSlugShot,
        ActionIDs.Hypercharge,
        ActionIDs.HeatBlast, ActionIDs.GaussRound,
        ActionIDs.HeatBlast, ActionIDs.Ricochet,
        ActionIDs.HeatBlast, ActionIDs.GaussRound,
        ActionIDs.HeatBlast, ActionIDs.Ricochet,
        ActionIDs.GaussRound,
        ActionIDs.HotShot, ActionIDs.Ricochet,
        ActionIDs.Drill,
        ActionIDs.CleanShot,
        ActionIDs.HeatedSplitShot,
        ActionIDs.HeatedSlugShot,
        ActionIDs.CleanShot
    })]
    [InlineData(50, true, new[]
    {
        ActionIDs.Reassemble,
        ActionIDs.HotShot, ActionIDs.GaussRound,
        ActionIDs.SplitShot, ActionIDs.Ricochet,
        ActionIDs.SlugShot, ActionIDs.GaussRound,
        ActionIDs.CleanShot, ActionIDs.Ricochet,
        ActionIDs.SplitShot,
        ActionIDs.SlugShot,
        ActionIDs.CleanShot,
        ActionIDs.SplitShot,
        ActionIDs.SlugShot,
        ActionIDs.CleanShot, ActionIDs.RookAutoturret,
        ActionIDs.SplitShot, ActionIDs.Wildfire,
        ActionIDs.SlugShot, ActionIDs.Hypercharge,
        ActionIDs.HeatBlast, ActionIDs.GaussRound,
        ActionIDs.HeatBlast, ActionIDs.GaussRound,
        ActionIDs.HeatBlast, ActionIDs.Ricochet,
        ActionIDs.HeatBlast, ActionIDs.Ricochet,
        ActionIDs.GaussRound,
        ActionIDs.CleanShot, ActionIDs.Ricochet,
        ActionIDs.HotShot,
        ActionIDs.SplitShot,
        ActionIDs.SlugShot,
        ActionIDs.CleanShot,
    })]
    public void Machinist_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);

    [Theory]
    [InlineData(90, 4, new[] { ActionIDs.Reassemble, ActionIDs.AirAnchor, ActionIDs.Reassemble, ActionIDs.Chainsaw })]
    [InlineData(80, 2, new[] { ActionIDs.Reassemble, ActionIDs.AirAnchor })]
    [InlineData(70, 2, new[] { ActionIDs.Reassemble, ActionIDs.Drill })]
    [InlineData(60, 2, new[] { ActionIDs.Reassemble, ActionIDs.Drill })]
    [InlineData(50, 2, new[] { ActionIDs.Reassemble, ActionIDs.HotShot })]
    public void SingleTarget_Reassemble_TriggersBeforeExpectedAction(int level, int queueSize, ActionIDs[] expectedActions)
    {
        QueueSize = queueSize;

        SetupSetConductor(SingleTarget!);
        MockService!.SetupInitial(level);

        var priorities = SetConductor!.Update(SetConfig);
        var priorityIds = priorities!.GetActionIds();

        priorityIds.Should().Equal(expectedActions.GetActionIds());
    }

    [Theory]
    [InlineData(90)]
    [InlineData(70)]
    public void SingleTarget_Hypercharge_TriggersExpectedActions(int level)
    {
        QueueSize = 11;
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.Hypercharge)).Returns(10);
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.BarrelStabilizer)).Returns(20);

        JobGauge = new JobGaugeMCH
        {
            OverheatTimeRemaining = 7.9 //8 seconds will suggest 6 heat blasts, which won't be possible in reality
        };
        JobDefinition = new TestJobDefinitionMCH(new TestJobGaugeMCH((JobGaugeMCH)JobGauge));

        SetupSetConductor(SingleTarget!);
        MockService.SetupInitial(level);

        var priorities = SetConductor!.Update(SetConfig);
        var priorityIds = priorities!.GetActionIds();

        // 5 heat blasts here and not other rotations because the other rotations have a global cooldown for the action before hypercharge causing it to not make 5 into the overheat window
        var expected = new[]
        {
            ActionIDs.HeatBlast, ActionIDs.GaussRound, ActionIDs.HeatBlast, ActionIDs.Ricochet, ActionIDs.HeatBlast,
            ActionIDs.GaussRound, ActionIDs.HeatBlast, ActionIDs.Ricochet, ActionIDs.HeatBlast, ActionIDs.GaussRound,
            ActionIDs.Reassemble, // no more overheat, normal recommendations
        };
        priorityIds.Should().Equal(expected.GetActionIds());
    }
}