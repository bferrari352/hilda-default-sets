using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Types;
using FluentAssertions;
using Hilda;
using Hilda.Constants;
using HildaDefaultSetsTests.Utils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests;

public class DefaultSetTestsMCH : DefaultSetTestBase
{
    public DefaultSetTestsMCH(TestBaseFixture testBaseFixture, ITestOutputHelper testOutputHelper) :
        base(testBaseFixture, testOutputHelper)
    {
        JobDefinition = JobData.Machinist.Definition;
        MockService = MockLibrary.MockService<MCHGaugeWrapper, MCHGauge>();
        QueueSize = 12;

        var sets = GetDefaultSets(JobData.Machinist)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }

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

        var priorities = SetConductor!.DeterminePriorities();
        var priorityIds = priorities!.GetActionIds();
        
        priorityIds.Should().Equal(expectedActions.GetActionIds());
    }

    [Theory]
    [InlineData(90)]
    [InlineData(70)]
    public void SingleTarget_Hypercharge_TriggersExpectedActions(int level)
    {
        OutputHelper.WriteLine($"");
        QueueSize = 7;
        MockService!.Setup(a => a.ActionHelper.GetActionRecast((uint) ActionIDs.Hypercharge)).Returns(10);
        MockService!.Setup(a => a.JobGauges.Get<MCHGaugeWrapper, MCHGauge>())
            .Returns(new MCHGaugeWrapper {OverheatTimeRemaining = 8});

        SetupSetConductor(SingleTarget!);
        MockService!.SetupInitial(level);

        var priorities = SetConductor!.DeterminePriorities();
        var priorityIds = priorities!.GetActionIds();

        var expected = new[]
        {
            ActionIDs.HeatBlast, ActionIDs.GaussRound, ActionIDs.HeatBlast, ActionIDs.Ricochet, ActionIDs.HeatBlast,
            ActionIDs.GaussRound, ActionIDs.HeatBlast
        };
        priorityIds.Should().Equal(expected.GetActionIds());
    }
}