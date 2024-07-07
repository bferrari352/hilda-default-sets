using System.Linq;
using Hilda.Conductors.JobDefinitions.Tanks;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.Tanks;

public class DefaultSetTestsDRK : DefaultSetTestBase
{
    public DefaultSetTestsDRK(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeDRK();
        JobDefinition = new TestJobDefinitionDRK(new TestJobGaugeDRK((JobGaugeDRK) JobGauge));
        
        QueueSize = 20;
        SetJobSets(JobData.DarkKnight);
    }
    
    [Theory (Skip = OutOfDate)]
    [InlineData(90, true, new[]
    {
        ActionIDs.HardSlash, ActionIDs.BloodWeapon,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfShadow,
        ActionIDs.Souleater, ActionIDs.Delirium,
        ActionIDs.BloodSpiller, ActionIDs.LivingShadow,
        ActionIDs.BloodSpiller, ActionIDs.EdgeOfShadow,
        ActionIDs.BloodSpiller, ActionIDs.SaltedEarth,
        ActionIDs.BloodSpiller, ActionIDs.SaltAndDarkness,
        ActionIDs.HardSlash, ActionIDs.CarveAndSplit,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfShadow,
        ActionIDs.Souleater, ActionIDs.Shadowbringer,
        ActionIDs.BloodSpiller, ActionIDs.Shadowbringer
    })]
    [InlineData(80, true, new[]
    {
        ActionIDs.HardSlash, ActionIDs.BloodWeapon,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfShadow,
        ActionIDs.Souleater, ActionIDs.Delirium,
        ActionIDs.BloodSpiller, ActionIDs.LivingShadow,
        ActionIDs.BloodSpiller, ActionIDs.EdgeOfShadow,
        ActionIDs.BloodSpiller, ActionIDs.SaltedEarth,
        ActionIDs.BloodSpiller, ActionIDs.CarveAndSplit,
        ActionIDs.HardSlash, ActionIDs.EdgeOfShadow,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfShadow,
        ActionIDs.Souleater, ActionIDs.EdgeOfShadow
    })]
    [InlineData(70, true, new[]
    {
        ActionIDs.HardSlash, ActionIDs.BloodWeapon,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfDarkness,
        ActionIDs.Souleater, ActionIDs.Delirium,
        ActionIDs.BloodSpiller, ActionIDs.SaltedEarth,
        ActionIDs.BloodSpiller, ActionIDs.EdgeOfDarkness,
        ActionIDs.BloodSpiller, ActionIDs.CarveAndSplit,
        ActionIDs.BloodSpiller, ActionIDs.EdgeOfDarkness,
        ActionIDs.HardSlash, ActionIDs.EdgeOfDarkness,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfDarkness,
        ActionIDs.Souleater
    })]
    [InlineData(60, true, new[]
    {
        ActionIDs.HardSlash, ActionIDs.BloodWeapon,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfDarkness,
        ActionIDs.Souleater, ActionIDs.SaltedEarth,
        ActionIDs.HardSlash, ActionIDs.CarveAndSplit,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfDarkness,
        ActionIDs.Souleater, ActionIDs.EdgeOfDarkness,
        ActionIDs.HardSlash, ActionIDs.EdgeOfDarkness,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfDarkness,
        ActionIDs.Souleater,
        ActionIDs.HardSlash,
        ActionIDs.SyphonStrike,
        ActionIDs.Souleater
    })]
    [InlineData(50, true, new[]
    {
        ActionIDs.HardSlash, ActionIDs.BloodWeapon,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfDarkness,
        ActionIDs.Souleater, ActionIDs.EdgeOfDarkness,
        ActionIDs.HardSlash, ActionIDs.EdgeOfDarkness,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfDarkness,
        ActionIDs.Souleater,
        ActionIDs.HardSlash,
        ActionIDs.SyphonStrike, ActionIDs.EdgeOfDarkness,
        ActionIDs.Souleater, 
        ActionIDs.HardSlash,
        ActionIDs.SyphonStrike,
        ActionIDs.Souleater,
        ActionIDs.HardSlash
    })]
    public void DarkKnight_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}