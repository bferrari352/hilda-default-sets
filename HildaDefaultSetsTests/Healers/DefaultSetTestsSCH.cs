using System.Linq;
using Hilda.Conductors.JobDefinitions.Healers;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.Healers;

public class DefaultSetTestsSCH : DefaultSetTestBase
{
    public DefaultSetTestsSCH(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeSCH();
        JobDefinition = new TestJobDefinitionSCH(new TestJobGaugeSCH((JobGaugeSCH) JobGauge));
        
        QueueSize = 5;
        
        SetJobSets(JobData.Scholar);
    }
    
    [Theory (Skip = OutOfDate)]
    [InlineData(90, true, new[] {ActionIDs.Biolysis, ActionIDs.AetherflowSCH, ActionIDs.BroilIV, ActionIDs.EnergyDrainSCH, ActionIDs.BroilIV})]
    [InlineData(80, true, new[] {ActionIDs.Biolysis, ActionIDs.AetherflowSCH, ActionIDs.BroilIII, ActionIDs.EnergyDrainSCH, ActionIDs.BroilIII})]
    [InlineData(70, true, new[] {ActionIDs.BioII, ActionIDs.AetherflowSCH, ActionIDs.BroilII, ActionIDs.EnergyDrainSCH, ActionIDs.BroilII})]
    [InlineData(60, true, new[] {ActionIDs.BioII, ActionIDs.AetherflowSCH, ActionIDs.Broil, ActionIDs.EnergyDrainSCH, ActionIDs.Broil})]
    [InlineData(50, true, new[] {ActionIDs.BioII, ActionIDs.AetherflowSCH, ActionIDs.RuinSCH, ActionIDs.EnergyDrainSCH, ActionIDs.RuinSCH})]
    [InlineData(90, false, new[] {ActionIDs.BroilIV, ActionIDs.AetherflowSCH, ActionIDs.BroilIV, ActionIDs.EnergyDrainSCH, ActionIDs.BroilIV})]
    [InlineData(80, false, new[] {ActionIDs.BroilIII, ActionIDs.AetherflowSCH, ActionIDs.BroilIII, ActionIDs.EnergyDrainSCH, ActionIDs.BroilIII})]
    [InlineData(70, false, new[] {ActionIDs.BroilII, ActionIDs.AetherflowSCH, ActionIDs.BroilII, ActionIDs.EnergyDrainSCH, ActionIDs.BroilII})]
    [InlineData(60, false, new[] {ActionIDs.Broil, ActionIDs.AetherflowSCH, ActionIDs.Broil, ActionIDs.EnergyDrainSCH, ActionIDs.Broil})]
    [InlineData(50, false, new[] {ActionIDs.RuinSCH, ActionIDs.AetherflowSCH, ActionIDs.RuinSCH, ActionIDs.EnergyDrainSCH, ActionIDs.RuinSCH})]
    public void Scholar_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}