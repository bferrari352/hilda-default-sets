using System.Linq;
using Hilda.Conductors.JobDefinitions.Healers;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.Healers;

public class DefaultSetTestsSGE : DefaultSetTestBase
{
    public DefaultSetTestsSGE(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeSGE();
        JobDefinition = new TestJobDefinitionSGE(new TestJobGaugeSGE((JobGaugeSGE) JobGauge));
        
        QueueSize = 6;

        var sets = GetDefaultSets(JobData.Sage)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }
    
    [Theory]
    [InlineData(90, true, new[] {ActionIDs.Eukrasia, ActionIDs.EukrasianDosisIII, ActionIDs.PhlegmaIII, ActionIDs.PhlegmaIII,
        ActionIDs.DosisIII, ActionIDs.DosisIII})]
    [InlineData(80, true, new[] {ActionIDs.Eukrasia, ActionIDs.EukrasianDosisII, ActionIDs.PhlegmaII, ActionIDs.PhlegmaII,
        ActionIDs.DosisII, ActionIDs.DosisII})]
    [InlineData(70, true, new[] {ActionIDs.Eukrasia, ActionIDs.EukrasianDosis, ActionIDs.Phlegma, ActionIDs.Phlegma,
        ActionIDs.Dosis, ActionIDs.Dosis})]
    [InlineData(60, true, new[] {ActionIDs.Eukrasia, ActionIDs.EukrasianDosis, ActionIDs.Phlegma, ActionIDs.Phlegma,
        ActionIDs.Dosis, ActionIDs.Dosis})]
    [InlineData(50, true, new[] {ActionIDs.Eukrasia, ActionIDs.EukrasianDosis, ActionIDs.Phlegma, ActionIDs.Phlegma,
        ActionIDs.Dosis, ActionIDs.Dosis})]
    [InlineData(90, false, new[] {ActionIDs.PhlegmaIII, ActionIDs.PhlegmaIII, ActionIDs.DosisIII, ActionIDs.DosisIII,
        ActionIDs.DosisIII, ActionIDs.DosisIII})]
    [InlineData(80, false, new[] {ActionIDs.PhlegmaII, ActionIDs.PhlegmaII, ActionIDs.DosisII, ActionIDs.DosisII,
        ActionIDs.DosisII, ActionIDs.DosisII})]
    [InlineData(70, false, new[] {ActionIDs.Phlegma, ActionIDs.Phlegma, ActionIDs.Dosis, ActionIDs.Dosis,
        ActionIDs.Dosis, ActionIDs.Dosis})]
    [InlineData(60, false, new[] {ActionIDs.Phlegma, ActionIDs.Phlegma, ActionIDs.Dosis, ActionIDs.Dosis,
        ActionIDs.Dosis, ActionIDs.Dosis})]
    [InlineData(50, false, new[] {ActionIDs.Phlegma, ActionIDs.Phlegma, ActionIDs.Dosis, ActionIDs.Dosis,
        ActionIDs.Dosis, ActionIDs.Dosis})]
    public void Sage_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}