using System.Linq;
using Hilda.Conductors.JobDefinitions.MeleeDps;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.MeleeDPS;

public class DefaultSetTestsMNK : DefaultSetTestBase
{
    public DefaultSetTestsMNK(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeMNK();
        JobDefinition = new TestJobDefinitionMNK(new TestJobGaugeMNK((JobGaugeMNK) JobGauge));

        var sets = GetDefaultSets(JobData.Monk)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name.Equals(DefaultSets.Get(DefaultSets.DisplayType.Single)))?.Priorities;
        MultiTarget = sets.FirstOrDefault(s => s.Name.Equals(DefaultSets.Get(DefaultSets.DisplayType.Multi)))?.Priorities;
    }
    
    [Theory]
    [InlineData(90, true, new[]
    {
        ActionIDs.DragonKick,
        ActionIDs.TwinSnakes, ActionIDs.RiddleOfFire,
        ActionIDs.Demolish,
        ActionIDs.DragonKick, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick, ActionIDs.RiddleOfWind,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.ElixirField, // Finisher
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.TwinSnakes,
        ActionIDs.DragonKick,
        ActionIDs.Demolish,
        ActionIDs.RisingPhoenix, // Finisher
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine,
        ActionIDs.TwinSnakes,
        ActionIDs.Demolish,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.PhantomRush, // Finisher
        ActionIDs.Bootshine
    })]
    [InlineData(80, true, new[]
    {
        ActionIDs.DragonKick,
        ActionIDs.TwinSnakes, ActionIDs.RiddleOfFire,
        ActionIDs.Demolish,
        ActionIDs.DragonKick, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick, ActionIDs.RiddleOfWind,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.ElixirField, // Finisher
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.TwinSnakes,
        ActionIDs.DragonKick,
        ActionIDs.Demolish,
        ActionIDs.FlintStrike, // Finisher
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine,
        ActionIDs.TwinSnakes,
        ActionIDs.Demolish,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.TornadoKick, // Finisher
        ActionIDs.Bootshine
    })]
    [InlineData(70, true, new[]
    {
        ActionIDs.DragonKick,
        ActionIDs.TwinSnakes, ActionIDs.RiddleOfFire,
        ActionIDs.Demolish,
        ActionIDs.DragonKick, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.ElixirField, // Finisher
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.TwinSnakes,
        ActionIDs.DragonKick,
        ActionIDs.Demolish,
        ActionIDs.FlintStrike, // Finisher
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine,
        ActionIDs.TwinSnakes,
        ActionIDs.Demolish,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.TornadoKick, // Finisher
        ActionIDs.Bootshine
    })]
    [InlineData(60, true, new[]
    {
        ActionIDs.DragonKick,
        ActionIDs.TwinSnakes,
        ActionIDs.Demolish,
        ActionIDs.DragonKick, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.ElixirField, // Finisher
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.TwinSnakes,
        ActionIDs.DragonKick,
        ActionIDs.Demolish,
        ActionIDs.FlintStrike, // Finisher
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine,
        ActionIDs.TwinSnakes,
        ActionIDs.Demolish,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.TornadoKick, // Finisher
        ActionIDs.Bootshine
    })]
    [InlineData(90, false, new[]
    {
        ActionIDs.DragonKick,
        ActionIDs.TwinSnakes, ActionIDs.RiddleOfFire,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick, ActionIDs.RiddleOfWind,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.ElixirField, // Finisher
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.TwinSnakes,
        ActionIDs.DragonKick,
        ActionIDs.Demolish,
        ActionIDs.RisingPhoenix, // Finisher
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine,
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.PhantomRush, // Finisher
        ActionIDs.Bootshine
    })]
    [InlineData(80, false, new[]
    {
        ActionIDs.DragonKick,
        ActionIDs.TwinSnakes, ActionIDs.RiddleOfFire,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick, ActionIDs.RiddleOfWind,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.ElixirField, // Finisher
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.TwinSnakes,
        ActionIDs.DragonKick,
        ActionIDs.Demolish,
        ActionIDs.FlintStrike, // Finisher
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine,
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.TornadoKick, // Finisher
        ActionIDs.Bootshine
    })]
    [InlineData(70, false, new[]
    {
        ActionIDs.DragonKick,
        ActionIDs.TwinSnakes, ActionIDs.RiddleOfFire,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.ElixirField, // Finisher
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.TwinSnakes,
        ActionIDs.DragonKick,
        ActionIDs.Demolish,
        ActionIDs.FlintStrike, // Finisher
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine,
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.TornadoKick, // Finisher
        ActionIDs.Bootshine
    })]
    [InlineData(60, false, new[]
    {
        ActionIDs.DragonKick,
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.ElixirField, // Finisher
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.TwinSnakes,
        ActionIDs.DragonKick,
        ActionIDs.Demolish,
        ActionIDs.FlintStrike, // Finisher
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine,
        ActionIDs.TwinSnakes,
        ActionIDs.SnapPunch,
        ActionIDs.DragonKick,
        ActionIDs.TrueStrike,
        ActionIDs.SnapPunch,
        ActionIDs.Bootshine, ActionIDs.PerfectBalance,
        ActionIDs.DragonKick,
        ActionIDs.Bootshine,
        ActionIDs.DragonKick,
        ActionIDs.TornadoKick, // Finisher
        ActionIDs.Bootshine
    })]
    public void Monk_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    
}