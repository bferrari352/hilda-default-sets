using System.Linq;
using Hilda.Conductors.JobDefinitions.MeleeDps;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.MeleeDPS;

public class DefaultSetTestsNIN : DefaultSetTestBase
{
    public DefaultSetTestsNIN(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeNIN();
        JobDefinition = new TestJobDefinitionNIN(new TestJobGaugeNIN((JobGaugeNIN) JobGauge));

        var sets = GetDefaultSets(JobData.Ninja)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }
    
    [Theory]
    [InlineData(90, true, new[]
    {
        ActionIDs.Huraijin, ActionIDs.Mug, // 45
        ActionIDs.Ten, ActionIDs.Chi, ActionIDs.Jin,
        ActionIDs.Suiton, ActionIDs.Kassatsu,
        ActionIDs.SpinningEdge, ActionIDs.Bunshin, // 45+5 -> 0
        ActionIDs.GustSlash, // 5 + 5
        ActionIDs.AeolianEdge, ActionIDs.TrickAttack, // 10 + 15 + 5
        ActionIDs.PhantomKamaitachi, ActionIDs.DreamWithinADream, // 30 + 10 + 5
        ActionIDs.ChiZero, ActionIDs.JinZero,
        ActionIDs.HyoshoRanyu,
        ActionIDs.Ten, ActionIDs.Chi,
        ActionIDs.Raiton, ActionIDs.TenChiJin,
        ActionIDs.TenZero, // Fuma Shuriken
        ActionIDs.ChiZero, // Raiton
        ActionIDs.JinZero, ActionIDs.Meisui, //Suiton -> Purge Suiton 45 + 50
        ActionIDs.FleetingRaiju, ActionIDs.Bavacakra, // 95 + 5 -> 50
        ActionIDs.FleetingRaiju, ActionIDs.Bavacakra, // 50 + 5 -> 5
        ActionIDs.Ten, ActionIDs.Chi,
        ActionIDs.Raiton,
        ActionIDs.FleetingRaiju, // 5 + 5
        ActionIDs.SpinningEdge // 10 + 5
    })]
    [InlineData(80, true, new[]
    {
        ActionIDs.Huraijin, ActionIDs.Mug,
        ActionIDs.Ten, ActionIDs.Chi, ActionIDs.Jin,
        ActionIDs.Suiton, ActionIDs.Kassatsu,
        ActionIDs.SpinningEdge, ActionIDs.Bunshin,
        ActionIDs.GustSlash,
        ActionIDs.AeolianEdge, ActionIDs.TrickAttack,
        ActionIDs.ChiZero, ActionIDs.JinZero,
        ActionIDs.HyoshoRanyu, ActionIDs.DreamWithinADream,
        ActionIDs.Ten, ActionIDs.Chi,
        ActionIDs.Raiton, ActionIDs.TenChiJin,
        ActionIDs.TenZero,
        ActionIDs.ChiZero,
        ActionIDs.JinZero, ActionIDs.Meisui,
        ActionIDs.SpinningEdge, ActionIDs.Bavacakra,
        ActionIDs.GustSlash,
        ActionIDs.AeolianEdge, ActionIDs.Bavacakra,
        ActionIDs.Ten, ActionIDs.Chi,
        ActionIDs.Raiton,
        ActionIDs.SpinningEdge
    })]
    [InlineData(70, true, new[]
    {
        ActionIDs.Huraijin, ActionIDs.Mug,
        ActionIDs.Ten, ActionIDs.Chi, ActionIDs.Jin,
        ActionIDs.Suiton, ActionIDs.Kassatsu,
        ActionIDs.SpinningEdge, ActionIDs.Bavacakra,
        ActionIDs.GustSlash,
        ActionIDs.AeolianEdge, ActionIDs.TrickAttack,
        ActionIDs.ChiZero, ActionIDs.JinZero,
        ActionIDs.Hyoton, ActionIDs.DreamWithinADream,
        ActionIDs.Ten, ActionIDs.Chi,
        ActionIDs.Raiton,
        ActionIDs.TenChiJin,
        ActionIDs.TenZero,
        ActionIDs.ChiZero,
        ActionIDs.JinZero,
        ActionIDs.SpinningEdge,
        ActionIDs.GustSlash,
        ActionIDs.AeolianEdge,
        ActionIDs.Ten, ActionIDs.Chi,
        ActionIDs.Raiton,
        ActionIDs.SpinningEdge
    })]
    [InlineData(60, true, new[]
    {
        ActionIDs.Huraijin, ActionIDs.Mug,
        ActionIDs.Ten, ActionIDs.Chi, ActionIDs.Jin,
        ActionIDs.Suiton, ActionIDs.Kassatsu,
        ActionIDs.SpinningEdge,
        ActionIDs.GustSlash,
        ActionIDs.AeolianEdge, ActionIDs.TrickAttack,
        ActionIDs.ChiZero, ActionIDs.JinZero,
        ActionIDs.Hyoton, ActionIDs.DreamWithinADream,
        ActionIDs.Ten, ActionIDs.Chi,
        ActionIDs.Raiton,
        ActionIDs.SpinningEdge,
        ActionIDs.GustSlash,
        ActionIDs.AeolianEdge,
        ActionIDs.Ten, ActionIDs.Chi,
        ActionIDs.Raiton,
        ActionIDs.SpinningEdge
    })]
    [InlineData(50, true, new[]
    {
        ActionIDs.Jin, ActionIDs.Chi, ActionIDs.Ten,
        ActionIDs.Huton, ActionIDs.Mug,
        ActionIDs.Ten, ActionIDs.Chi, ActionIDs.Jin,
        ActionIDs.Suiton, ActionIDs.Kassatsu,
        ActionIDs.SpinningEdge, ActionIDs.Assassinate,
        ActionIDs.GustSlash,
        ActionIDs.AeolianEdge, ActionIDs.TrickAttack,
        ActionIDs.ChiZero, ActionIDs.JinZero,
        ActionIDs.Hyoton,
        ActionIDs.SpinningEdge,
        ActionIDs.GustSlash,
        ActionIDs.AeolianEdge,
        ActionIDs.Chi, ActionIDs.Ten,
        ActionIDs.Katon,
        ActionIDs.SpinningEdge
    })]
    public void Ninja_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}