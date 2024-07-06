using System.Linq;
using FluentAssertions;
using Hilda.Conductors.JobDefinitions.MagicDps;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.MagicDPS;

public class DefaultSetTestsRDM : DefaultSetTestBase
{
    public DefaultSetTestsRDM(TestBaseFixture fixture, ITestOutputHelper output) :
        base(fixture, output)
    {
        JobGauge = new JobGaugeRDM();
        JobDefinition = new TestJobDefinitionRDM(new TestJobGaugeRDM((JobGaugeRDM) JobGauge));
        
        QueueSize = 12;

        var sets = GetDefaultSets(JobData.RedMage)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name.Equals(DefaultSets.Get(DefaultSets.DisplayType.Single)))?.Priorities;
        MultiTarget = sets.FirstOrDefault(s => s.Name.Equals(DefaultSets.Get(DefaultSets.DisplayType.Multi)))?.Priorities;
    }

    [Theory]
    [InlineData(90, false, new[] { ActionIDs.JoltII, ActionIDs.VeraeroIII, ActionIDs.Manafication, ActionIDs.EnchantedRiposte,
        ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau, ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte,
        ActionIDs.Verflare, ActionIDs.Scorch, ActionIDs.Resolution, ActionIDs.JoltII })]
    [InlineData(80, false, new[] { ActionIDs.JoltII, ActionIDs.Veraero, ActionIDs.Manafication, ActionIDs.EnchantedRiposte,
        ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau, ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte,
        ActionIDs.Verflare, ActionIDs.Scorch, ActionIDs.JoltII, ActionIDs.Veraero })]
    [InlineData(70, false, new[] { ActionIDs.JoltII, ActionIDs.Veraero, ActionIDs.Manafication, ActionIDs.EnchantedRiposte,
        ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau, ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte,
        ActionIDs.Verflare, ActionIDs.JoltII, ActionIDs.Veraero, ActionIDs.JoltII })]
    [InlineData(60, false, new[] { ActionIDs.Jolt, ActionIDs.Veraero, ActionIDs.Manafication, ActionIDs.EnchantedRiposte,
        ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau, ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte,
        ActionIDs.Jolt, ActionIDs.Verthunder, ActionIDs.Jolt, ActionIDs.Veraero })]
    [InlineData(50, false, new[] { ActionIDs.Jolt, ActionIDs.Veraero, ActionIDs.Fleche, ActionIDs.Jolt, ActionIDs.Verthunder,
        ActionIDs.Jolt, ActionIDs.Veraero, ActionIDs.Jolt, ActionIDs.Verthunder, ActionIDs.Jolt, ActionIDs.Veraero,
        ActionIDs.Jolt})]
    [InlineData(30, false, new[] { ActionIDs.Jolt, ActionIDs.Veraero, ActionIDs.Jolt, ActionIDs.Verthunder, ActionIDs.Jolt,
        ActionIDs.Veraero, ActionIDs.Jolt, ActionIDs.Verthunder, ActionIDs.EnchantedRiposte, ActionIDs.Jolt, ActionIDs.Veraero,
        ActionIDs.Jolt})]
    public void RedMage_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);

    [Theory]
    [InlineData(90, 8, new[] { ActionIDs.EnchantedRiposte, ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau,
        ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte, ActionIDs.Verholy, ActionIDs.Scorch, ActionIDs.Resolution })]
    [InlineData(80, 7, new[] { ActionIDs.EnchantedRiposte, ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau,
        ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte, ActionIDs.Verholy, ActionIDs.Scorch })]
    [InlineData(70, 6, new[] { ActionIDs.EnchantedRiposte, ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau,
        ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte, ActionIDs.Verholy })]
    [InlineData(60, 4, new[] { ActionIDs.EnchantedRiposte, ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau,
        ActionIDs.EnchantedRedoublement })]
    public void SingleTarget_Manafication_ShouldTriggerCombo(int level, int queueSize, ActionIDs[] expectedActions)
    {
        QueueSize = queueSize;
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint) ActionIDs.Manafication)).Returns(60);

        JobGauge = new JobGaugeRDM
        {
            BlackMana = 50,
            WhiteMana = 50,
            ManaStacks = 0
        };
        JobDefinition = new TestJobDefinitionRDM(new TestJobGaugeRDM((JobGaugeRDM) JobGauge));

        SetupSetConductor(SingleTarget!);
        MockService.SetupInitial(level);

        var priorities = SetConductor!.Update(SetConfig);
        var priorityIds = priorities!.GetActionIds();
        
        priorityIds.Should().Equal(expectedActions.GetActionIds());
    }
}