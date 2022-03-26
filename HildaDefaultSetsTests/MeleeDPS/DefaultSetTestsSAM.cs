using System.Linq;
using Hilda.Conductors.JobDefinitions.MeleeDps;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.MeleeDPS;

public class DefaultSetTestsSAM : DefaultSetTestBase
{
    public DefaultSetTestsSAM(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeSAM();
        JobDefinition = new TestJobDefinitionSAM(new TestJobGaugeSAM((JobGaugeSAM) JobGauge));

        var sets = GetDefaultSets(JobData.Samurai)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }
    
    [Theory]
    [InlineData(90, true, new[]
    {
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko, ActionIDs.Ikishoten,
        ActionIDs.Kasha, ActionIDs.HissatsuSenei,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.KaeshiSetsugekka,
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko, ActionIDs.HissatsuKaiten,
        ActionIDs.Higanbana,
        ActionIDs.Gekko, ActionIDs.HissatsuKaiten,
        ActionIDs.OgiNamikiri,
        ActionIDs.KaeshiNamikiri, ActionIDs.Shoha,
        ActionIDs.Kasha, ActionIDs.HissatsuShinten,
        ActionIDs.Hakaze,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.KaeshiSetsugekka
    })]
    [InlineData(80, true, new[]
    {
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko, ActionIDs.Ikishoten,
        ActionIDs.Kasha, ActionIDs.HissatsuSenei,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.KaeshiSetsugekka, ActionIDs.HissatsuShinten,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko, ActionIDs.HissatsuKaiten,
        ActionIDs.Higanbana,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko,
        ActionIDs.Hakaze,
        ActionIDs.Shifu,
        ActionIDs.Kasha, ActionIDs.HissatsuShinten,
        ActionIDs.Hakaze,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka, ActionIDs.Shoha,
        ActionIDs.Hakaze
    })]
    [InlineData(70, true, new[]
    {
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko, ActionIDs.Ikishoten,
        ActionIDs.Kasha, ActionIDs.HissatsuShinten,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka, ActionIDs.HissatsuShinten,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko, ActionIDs.HissatsuKaiten,
        ActionIDs.Higanbana,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko,
        ActionIDs.Hakaze,
        ActionIDs.Shifu,
        ActionIDs.Kasha, ActionIDs.HissatsuShinten,
        ActionIDs.Hakaze,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze
    })]
    [InlineData(60, true, new[]
    {
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko,
        ActionIDs.Kasha,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko,
        ActionIDs.Higanbana,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko,
        ActionIDs.Hakaze,
        ActionIDs.Shifu,
        ActionIDs.Kasha,
        ActionIDs.Hakaze,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze
    })]
    [InlineData(50, true, new[]
    {
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko,
        ActionIDs.Kasha,
        ActionIDs.Yukikaze,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko,
        ActionIDs.Higanbana,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko,
        ActionIDs.Hakaze,
        ActionIDs.Shifu,
        ActionIDs.Kasha,
        ActionIDs.Hakaze,
        ActionIDs.Yukikaze,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze
    })]
    [InlineData(90, false, new[]
    {
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko, ActionIDs.Ikishoten,
        ActionIDs.Kasha, ActionIDs.HissatsuSenei,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.KaeshiSetsugekka,
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko, ActionIDs.HissatsuKaiten,
        ActionIDs.OgiNamikiri,
        ActionIDs.KaeshiNamikiri,
        ActionIDs.Kasha, ActionIDs.HissatsuShinten,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.KaeshiSetsugekka, ActionIDs.Shoha,
        ActionIDs.Hakaze
    })]
    [InlineData(80, false, new[]
    {
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko, ActionIDs.Ikishoten,
        ActionIDs.Kasha, ActionIDs.HissatsuSenei,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.KaeshiSetsugekka, ActionIDs.HissatsuShinten,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko, ActionIDs.HissatsuShinten,
        ActionIDs.Hakaze,
        ActionIDs.Shifu,
        ActionIDs.Kasha,
        ActionIDs.Hakaze,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze
    })]
    [InlineData(70, false, new[]
    {
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko, ActionIDs.Ikishoten,
        ActionIDs.Kasha, ActionIDs.HissatsuShinten,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka, ActionIDs.HissatsuShinten,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko, ActionIDs.HissatsuShinten,
        ActionIDs.Hakaze,
        ActionIDs.Shifu,
        ActionIDs.Kasha,
        ActionIDs.Hakaze,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze
    })]
    [InlineData(60, false, new[]
    {
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko,
        ActionIDs.Kasha,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko,
        ActionIDs.Hakaze,
        ActionIDs.Shifu,
        ActionIDs.Kasha,
        ActionIDs.Hakaze,
        ActionIDs.Yukikaze, ActionIDs.HissatsuKaiten,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze
    })]
    [InlineData(50, false, new[]
    {
        ActionIDs.MeikyoShisui,
        ActionIDs.Gekko,
        ActionIDs.Kasha,
        ActionIDs.Yukikaze,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze,
        ActionIDs.Jinpu,
        ActionIDs.Gekko,
        ActionIDs.Hakaze,
        ActionIDs.Shifu,
        ActionIDs.Kasha,
        ActionIDs.Hakaze,
        ActionIDs.Yukikaze,
        ActionIDs.MidareSetsugekka,
        ActionIDs.Hakaze
    })]
    public void Samurai_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}