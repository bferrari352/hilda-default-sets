using System.Linq;
using Hilda.Conductors.JobDefinitions.MeleeDps;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.MeleeDPS;

public class DefaultSetTestsRPR : DefaultSetTestBase
{
    public DefaultSetTestsRPR(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeRPR();
        JobDefinition = new TestJobDefinitionRPR(new TestJobGaugeRPR((JobGaugeRPR) JobGauge));

        var sets = GetDefaultSets(JobData.Reaper)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name.Equals(DefaultSets.Get(DefaultSets.DisplayType.Single)))?.Priorities;
        MultiTarget = sets.FirstOrDefault(s => s.Name.Equals(DefaultSets.Get(DefaultSets.DisplayType.Multi)))?.Priorities;
    }
    
    [Theory]
    [InlineData(90, true, new[]
    {
        ActionIDs.ShadowOfDeath, ActionIDs.ArcaneCircle,
        ActionIDs.SoulSlice, ActionIDs.Gluttony,
        ActionIDs.Gibbet,
        ActionIDs.Gallows,
        ActionIDs.PlentifulHarvest, ActionIDs.Enshroud,
        ActionIDs.VoidReaping,
        ActionIDs.CrossReaping, ActionIDs.LemuresSlice,
        ActionIDs.VoidReaping,
        ActionIDs.CrossReaping, ActionIDs.LemuresSlice,
        ActionIDs.Communio,
        ActionIDs.SoulSlice, ActionIDs.UnveiledGibbet,
        ActionIDs.Gibbet,
        ActionIDs.ShadowOfDeath,
        ActionIDs.Slice,
        ActionIDs.SoulSlice,
        ActionIDs.UnveiledGallows,
        ActionIDs.Gallows,
        ActionIDs.WaxingSlice,
        ActionIDs.InfernalSlice
    })]
    [InlineData(80, true, new[]
    {
        ActionIDs.ShadowOfDeath,
        ActionIDs.SoulSlice, ActionIDs.Gluttony,
        ActionIDs.Gibbet,
        ActionIDs.Gallows,
        ActionIDs.SoulSlice, ActionIDs.UnveiledGibbet,
        ActionIDs.Gibbet,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice,
        ActionIDs.InfernalSlice,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice, ActionIDs.UnveiledGallows,
        ActionIDs.Gallows,
        ActionIDs.ShadowOfDeath,
        ActionIDs.InfernalSlice,
        ActionIDs.SoulSlice, ActionIDs.UnveiledGibbet,
        ActionIDs.Gibbet,
        ActionIDs.Enshroud,
        ActionIDs.VoidReaping,
        ActionIDs.CrossReaping,
        ActionIDs.VoidReaping,
        ActionIDs.CrossReaping,
        ActionIDs.VoidReaping,
        ActionIDs.Slice
    })]
    [InlineData(70, true, new[]
    {
        ActionIDs.ShadowOfDeath,
        ActionIDs.SoulSlice, ActionIDs.BloodStalk,
        ActionIDs.Gibbet,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice,
        ActionIDs.InfernalSlice,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice, ActionIDs.UnveiledGallows,
        ActionIDs.Gallows,
        ActionIDs.InfernalSlice,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice,
        ActionIDs.ShadowOfDeath,
        ActionIDs.InfernalSlice,
        ActionIDs.SoulSlice, ActionIDs.UnveiledGibbet,
        ActionIDs.Gibbet,
        ActionIDs.Slice
    })]
    [InlineData(60, true, new[]
    {
        ActionIDs.ShadowOfDeath,
        ActionIDs.SoulSlice, ActionIDs.BloodStalk,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice,
        ActionIDs.InfernalSlice,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice, ActionIDs.BloodStalk,
        ActionIDs.InfernalSlice,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice,
        ActionIDs.InfernalSlice,
        ActionIDs.Slice, ActionIDs.BloodStalk,
        ActionIDs.ShadowOfDeath,
        ActionIDs.WaxingSlice
    })]
    [InlineData(50, true, new[]
    {
        ActionIDs.ShadowOfDeath,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice,
        ActionIDs.InfernalSlice,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice, ActionIDs.BloodStalk,
        ActionIDs.InfernalSlice,
        ActionIDs.Slice,
        ActionIDs.WaxingSlice,
        ActionIDs.InfernalSlice,
        ActionIDs.Slice, ActionIDs.BloodStalk,
        ActionIDs.WaxingSlice,
        ActionIDs.ShadowOfDeath,
    })]
    public void Reaper_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}