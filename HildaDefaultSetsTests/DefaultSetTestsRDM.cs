using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Types;
using FluentAssertions;
using Hilda;
using Hilda.Constants;
using HildaDefaultSetsTests.Utils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests;

public class DefaultSetTestsRDM : DefaultSetTestBase
{
    public DefaultSetTestsRDM(TestBaseFixture testBaseFixture, ITestOutputHelper testOutputHelper) :
        base(testBaseFixture, testOutputHelper)
    {
        JobDefinition = JobData.RedMage.Definition;
        MockService = MockLibrary.MockService<RDMGaugeWrapper, RDMGauge>();
        QueueSize = 12;

        var sets = GetDefaultSets(JobData.RedMage)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }

    [Theory]
    [InlineData(90, new[] { ActionIDs.JoltII, ActionIDs.VeraeroIII, ActionIDs.Manafication, ActionIDs.EnchantedRiposte,
        ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau, ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte,
        ActionIDs.Verflare, ActionIDs.Scorch, ActionIDs.Resolution, ActionIDs.JoltII })]
    [InlineData(80, new[] { ActionIDs.JoltII, ActionIDs.Veraero, ActionIDs.Manafication, ActionIDs.EnchantedRiposte,
        ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau, ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte,
        ActionIDs.Verflare, ActionIDs.Scorch, ActionIDs.JoltII, ActionIDs.Veraero })]
    [InlineData(70, new[] { ActionIDs.JoltII, ActionIDs.Veraero, ActionIDs.Manafication, ActionIDs.EnchantedRiposte,
        ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau, ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte,
        ActionIDs.Verflare, ActionIDs.JoltII, ActionIDs.Veraero, ActionIDs.JoltII })]
    [InlineData(60, new[] { ActionIDs.Jolt, ActionIDs.Veraero, ActionIDs.Manafication, ActionIDs.EnchantedRiposte,
        ActionIDs.Fleche, ActionIDs.EnchantedZwerchhau, ActionIDs.EnchantedRedoublement, ActionIDs.ContreSixte,
        ActionIDs.Jolt, ActionIDs.Verthunder, ActionIDs.Jolt, ActionIDs.Veraero })]
    [InlineData(50, new[] { ActionIDs.Jolt, ActionIDs.Veraero, ActionIDs.Fleche, ActionIDs.Jolt, ActionIDs.Verthunder,
        ActionIDs.Jolt, ActionIDs.Veraero, ActionIDs.Jolt, ActionIDs.Verthunder, ActionIDs.Jolt, ActionIDs.Veraero,
        ActionIDs.Jolt})]
    [InlineData(30, new[] { ActionIDs.Jolt, ActionIDs.Veraero, ActionIDs.Jolt, ActionIDs.Verthunder, ActionIDs.Jolt,
        ActionIDs.Veraero, ActionIDs.Jolt, ActionIDs.Verthunder, ActionIDs.EnchantedRiposte, ActionIDs.Jolt, ActionIDs.Veraero,
        ActionIDs.Jolt})]
    public void SingleTarget_BasicRotation_ReturnsExpectedValues(int level, ActionIDs[] expectedActions)
    {
        SetupSetConductor(SingleTarget!);
        MockService!.SetupInitial(level);

        var priorities = SetConductor!.DeterminePriorities();
        var priorityIds = priorities!.GetActionIds();
        
        priorityIds.Should().Equal(expectedActions.GetActionIds());
    }

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
        MockService!.Setup(a => a.ActionHelper.GetActionRecast((uint) ActionIDs.Manafication)).Returns(60);
        MockService.SetupSequence(a => a.JobGauges.Get<RDMGaugeWrapper, RDMGauge>())
            .Returns(new RDMGaugeWrapper {BlackMana = 50, WhiteMana = 50, ManaStacks = 0});

        SetupSetConductor(SingleTarget!);
        MockService!.SetupInitial(level);

        var priorities = SetConductor!.DeterminePriorities();
        var priorityIds = priorities!.GetActionIds();
        
        priorityIds.Should().Equal(expectedActions.GetActionIds());
    }
}