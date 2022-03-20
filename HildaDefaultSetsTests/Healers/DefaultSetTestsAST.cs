using System.Linq;
using Hilda.Conductors.JobDefinitions.Healers;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.Healers;

public class DefaultSetTestsAST : DefaultSetTestBase
{
    public DefaultSetTestsAST(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeAST();
        JobDefinition = new TestJobDefinitionAST(new TestJobGaugeAST((JobGaugeAST) JobGauge));
        
        QueueSize = 4;

        var sets = GetDefaultSets(JobData.Astrologian)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }
    
    [Theory]
    [InlineData(90, true, new[] {ActionIDs.CombustIII, ActionIDs.FallMalefic, ActionIDs.FallMalefic, ActionIDs.FallMalefic})]
    [InlineData(80, true, new[] {ActionIDs.CombustIII, ActionIDs.MaleficIV, ActionIDs.MaleficIV, ActionIDs.MaleficIV})]
    [InlineData(70, true, new[] {ActionIDs.CombustII, ActionIDs.MaleficIII, ActionIDs.MaleficIII, ActionIDs.MaleficIII})]
    [InlineData(60, true, new[] {ActionIDs.CombustII, ActionIDs.MaleficII, ActionIDs.MaleficII, ActionIDs.MaleficII})]
    [InlineData(50, true, new[] {ActionIDs.CombustII, ActionIDs.Malefic, ActionIDs.Malefic, ActionIDs.Malefic})]
    [InlineData(90, false, new[] {ActionIDs.FallMalefic, ActionIDs.FallMalefic, ActionIDs.FallMalefic, ActionIDs.FallMalefic})]
    [InlineData(80, false, new[] {ActionIDs.MaleficIV, ActionIDs.MaleficIV, ActionIDs.MaleficIV, ActionIDs.MaleficIV})]
    [InlineData(70, false, new[] {ActionIDs.MaleficIII, ActionIDs.MaleficIII, ActionIDs.MaleficIII, ActionIDs.MaleficIII})]
    [InlineData(60, false, new[] {ActionIDs.MaleficII, ActionIDs.MaleficII, ActionIDs.MaleficII, ActionIDs.MaleficII})]
    [InlineData(50, false, new[] {ActionIDs.Malefic, ActionIDs.Malefic, ActionIDs.Malefic, ActionIDs.Malefic})]
    public void Astrologian_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}