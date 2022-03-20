using System.Linq;
using FluentAssertions;
using Hilda.Conductors.JobDefinitions.Healers;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.Healers;

public class DefaultSetTestsWHM : DefaultSetTestBase
{
    public DefaultSetTestsWHM(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeWHM();
        JobDefinition = new TestJobDefinitionWHM(new TestJobGaugeWHM((JobGaugeWHM) JobGauge));
        
        QueueSize = 4;

        var sets = GetDefaultSets(JobData.WhiteMage)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }

    [Theory]
    [InlineData(90, true, new[] {ActionIDs.Dia, ActionIDs.GlareIII, ActionIDs.GlareIII, ActionIDs.GlareIII})]
    [InlineData(80, true, new[] {ActionIDs.Dia, ActionIDs.Glare, ActionIDs.Glare, ActionIDs.Glare})]
    [InlineData(70, true, new[] {ActionIDs.AeroII, ActionIDs.StoneIV, ActionIDs.StoneIV, ActionIDs.StoneIV})]
    [InlineData(60, true, new[] {ActionIDs.AeroII, ActionIDs.StoneIII, ActionIDs.StoneIII, ActionIDs.StoneIII})]
    [InlineData(50, true, new[] {ActionIDs.AeroII, ActionIDs.StoneII, ActionIDs.StoneII, ActionIDs.StoneII})]
    [InlineData(90, false, new[] {ActionIDs.GlareIII, ActionIDs.GlareIII, ActionIDs.GlareIII, ActionIDs.GlareIII})]
    [InlineData(80, false, new[] {ActionIDs.Glare, ActionIDs.Glare, ActionIDs.Glare, ActionIDs.Glare})]
    [InlineData(70, false, new[] {ActionIDs.StoneIV, ActionIDs.StoneIV, ActionIDs.StoneIV, ActionIDs.StoneIV})]
    [InlineData(60, false, new[] {ActionIDs.StoneIII, ActionIDs.StoneIII, ActionIDs.StoneIII, ActionIDs.StoneIII})]
    [InlineData(50, false, new[] {ActionIDs.StoneII, ActionIDs.StoneII, ActionIDs.StoneII, ActionIDs.StoneII})]
    public void WhiteMage_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);

    [Theory]
    [InlineData(3, new[] {ActionIDs.AfflatusMisery, ActionIDs.Dia, ActionIDs.GlareIII, ActionIDs.GlareIII})]
    [InlineData(2, new[] {ActionIDs.Dia, ActionIDs.GlareIII, ActionIDs.GlareIII, ActionIDs.GlareIII})]
    [InlineData(1, new[] {ActionIDs.Dia, ActionIDs.GlareIII, ActionIDs.GlareIII, ActionIDs.GlareIII})]
    public void SingleTarget_BloodLilies_ShouldTriggerAfflatusMisery(int bloodLilyCount, ActionIDs[] expectedActions)

    {
        JobGauge = new JobGaugeWHM
        {
            BloodLilies = bloodLilyCount
        };
        JobDefinition = new TestJobDefinitionWHM(new TestJobGaugeWHM((JobGaugeWHM) JobGauge));

        SetupSetConductor(SingleTarget!);
        MockService.SetupInitial();
        MockService.Setup(a => a.TargetHelper.IsTargetBossMob()).Returns(true);
        
        var priorities = SetConductor!.DeterminePriorities();
        var priorityIds = priorities!.GetActionIds();
        
        priorityIds.Should().Equal(expectedActions.GetActionIds());
    }
}