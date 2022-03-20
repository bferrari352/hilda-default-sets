using System.Linq;
using Hilda.Conductors.JobDefinitions.Tanks;
using Hilda.Constants;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.Tanks;

public class DefaultSetTestsPLD : DefaultSetTestBase
{
    public DefaultSetTestsPLD(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugePLD();
        JobDefinition = new TestJobDefinitionPLD(new TestJobGaugePLD((JobGaugePLD) JobGauge));
        
        QueueSize = 20;

        var sets = GetDefaultSets(JobData.Paladin)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }
    
    [Theory]
    [InlineData(90, true, new[]
    {
        ActionIDs.FastBlade, ActionIDs.FightOrFlight, ActionIDs.RiotBlade, ActionIDs.CircleOfScorn, ActionIDs.GoringBlade,
        ActionIDs.Expiacion, ActionIDs.FastBlade, ActionIDs.Requiescat, ActionIDs.HolySpirit, ActionIDs.HolySpirit,
        ActionIDs.HolySpirit, ActionIDs.HolySpirit, ActionIDs.Confiteor, ActionIDs.BladeOfFaith, ActionIDs.BladeOfTruth,
        ActionIDs.BladeOfValor, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.CircleOfScorn
    })]
    [InlineData(80, true, new[]
    {
        ActionIDs.FastBlade, ActionIDs.FightOrFlight, ActionIDs.RiotBlade, ActionIDs.CircleOfScorn, ActionIDs.GoringBlade,
        ActionIDs.SpiritsWithin, ActionIDs.FastBlade, ActionIDs.Requiescat, ActionIDs.HolySpirit, ActionIDs.HolySpirit,
        ActionIDs.HolySpirit, ActionIDs.HolySpirit, ActionIDs.Confiteor, ActionIDs.FastBlade, ActionIDs.RiotBlade,
        ActionIDs.RoyalAuthority, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.CircleOfScorn
    })]
    [InlineData(70, true, new[]
    {
        ActionIDs.FastBlade, ActionIDs.FightOrFlight, ActionIDs.RiotBlade, ActionIDs.CircleOfScorn, ActionIDs.GoringBlade,
        ActionIDs.SpiritsWithin, ActionIDs.FastBlade, ActionIDs.Requiescat, ActionIDs.HolySpirit, ActionIDs.HolySpirit,
        ActionIDs.HolySpirit, ActionIDs.HolySpirit, ActionIDs.HolySpirit, ActionIDs.FastBlade, ActionIDs.RiotBlade,
        ActionIDs.RoyalAuthority, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.CircleOfScorn
    })]
    [InlineData(60, true, new[]
    {
        ActionIDs.FastBlade, ActionIDs.FightOrFlight, ActionIDs.RiotBlade, ActionIDs.CircleOfScorn, ActionIDs.GoringBlade,
        ActionIDs.SpiritsWithin, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.FastBlade,
        ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority,
        ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.CircleOfScorn, ActionIDs.FastBlade
    })]
    [InlineData(50, true, new[]
    {
        ActionIDs.FastBlade, ActionIDs.FightOrFlight, ActionIDs.RiotBlade, ActionIDs.CircleOfScorn, ActionIDs.RageOfHalone,
        ActionIDs.SpiritsWithin, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RageOfHalone, ActionIDs.FastBlade,
        ActionIDs.RiotBlade, ActionIDs.RageOfHalone, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RageOfHalone,
        ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RageOfHalone, ActionIDs.CircleOfScorn, ActionIDs.FastBlade
    })]
    [InlineData(90, false, new[]
    {
        ActionIDs.FastBlade, ActionIDs.FightOrFlight, ActionIDs.RiotBlade, ActionIDs.CircleOfScorn, ActionIDs.RoyalAuthority,
        ActionIDs.Expiacion, ActionIDs.FastBlade, ActionIDs.Requiescat, ActionIDs.HolySpirit, ActionIDs.HolySpirit,
        ActionIDs.HolySpirit, ActionIDs.HolySpirit, ActionIDs.Confiteor, ActionIDs.BladeOfFaith, ActionIDs.BladeOfTruth,
        ActionIDs.BladeOfValor, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.CircleOfScorn
    })]
    [InlineData(80, false, new[]
    {
        ActionIDs.FastBlade, ActionIDs.FightOrFlight, ActionIDs.RiotBlade, ActionIDs.CircleOfScorn, ActionIDs.RoyalAuthority,
        ActionIDs.SpiritsWithin, ActionIDs.FastBlade, ActionIDs.Requiescat, ActionIDs.HolySpirit, ActionIDs.HolySpirit,
        ActionIDs.HolySpirit, ActionIDs.HolySpirit, ActionIDs.Confiteor, ActionIDs.FastBlade, ActionIDs.RiotBlade,
        ActionIDs.RoyalAuthority, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.CircleOfScorn
    })]
    [InlineData(70, false, new[]
    {
        ActionIDs.FastBlade, ActionIDs.FightOrFlight, ActionIDs.RiotBlade, ActionIDs.CircleOfScorn, ActionIDs.RoyalAuthority,
        ActionIDs.SpiritsWithin, ActionIDs.FastBlade, ActionIDs.Requiescat, ActionIDs.HolySpirit, ActionIDs.HolySpirit,
        ActionIDs.HolySpirit, ActionIDs.HolySpirit, ActionIDs.HolySpirit, ActionIDs.FastBlade, ActionIDs.RiotBlade,
        ActionIDs.RoyalAuthority, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.CircleOfScorn
    })]
    [InlineData(60, false, new[]
    {
        ActionIDs.FastBlade, ActionIDs.FightOrFlight, ActionIDs.RiotBlade, ActionIDs.CircleOfScorn, ActionIDs.RoyalAuthority,
        ActionIDs.SpiritsWithin, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.FastBlade,
        ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority,
        ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RoyalAuthority, ActionIDs.CircleOfScorn, ActionIDs.FastBlade
    })]
    [InlineData(50, false, new[]
    {
        ActionIDs.FastBlade, ActionIDs.FightOrFlight, ActionIDs.RiotBlade, ActionIDs.CircleOfScorn, ActionIDs.RageOfHalone,
        ActionIDs.SpiritsWithin, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RageOfHalone, ActionIDs.FastBlade,
        ActionIDs.RiotBlade, ActionIDs.RageOfHalone, ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RageOfHalone,
        ActionIDs.FastBlade, ActionIDs.RiotBlade, ActionIDs.RageOfHalone, ActionIDs.CircleOfScorn, ActionIDs.FastBlade
    })]
    public void Paladin_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
}