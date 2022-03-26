using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Hilda.Conductors.JobDefinitions.RangedDps;
using Hilda.Constants;
using Hilda.Models;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.RangedDPS;

public class DefaultSetTestsBRD : DefaultSetTestBase
{
    public DefaultSetTestsBRD(TestBaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        JobGauge = new JobGaugeBRD();
        JobDefinition = new TestJobDefinitionBRD(new TestJobGaugeBRD((JobGaugeBRD) JobGauge));
        
        var sets = GetDefaultSets(JobData.Bard)?.ToList();
        if (sets == null) return;
        
        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }
    
    [Theory]
    [InlineData(90, true, new[]
    {
        ActionIDs.Stormbite, ActionIDs.TheWanderersMinuet,
        ActionIDs.CausticBite, ActionIDs.RagingStrikes,
        ActionIDs.BurstShot, ActionIDs.EmpyrealArrow,
        ActionIDs.BurstShot, ActionIDs.Bloodletter,
        ActionIDs.BurstShot, ActionIDs.RadiantFinale,
        ActionIDs.BurstShot, ActionIDs.Barrage,
        ActionIDs.RefulgentArrow, ActionIDs.Sidewinder,
        ActionIDs.BurstShot,
        ActionIDs.BurstShot,
        ActionIDs.BurstShot, ActionIDs.EmpyrealArrow,
        ActionIDs.BurstShot, ActionIDs.Bloodletter,
        ActionIDs.BurstShot,
        ActionIDs.IronJaws
    })]
    [InlineData(80, true, new[]
    {
        ActionIDs.Stormbite, ActionIDs.TheWanderersMinuet,
        ActionIDs.CausticBite, ActionIDs.RagingStrikes,
        ActionIDs.BurstShot, ActionIDs.EmpyrealArrow,
        ActionIDs.BurstShot, ActionIDs.Bloodletter,
        ActionIDs.BurstShot, ActionIDs.Barrage,
        ActionIDs.RefulgentArrow, ActionIDs.Sidewinder,
        ActionIDs.BurstShot,
        ActionIDs.BurstShot,
        ActionIDs.BurstShot,
        ActionIDs.BurstShot, ActionIDs.EmpyrealArrow
    })]
    [InlineData(70, true, new[]
    {
        ActionIDs.Stormbite, ActionIDs.TheWanderersMinuet,
        ActionIDs.CausticBite, ActionIDs.RagingStrikes,
        ActionIDs.HeavyShot, ActionIDs.EmpyrealArrow,
        ActionIDs.HeavyShot, ActionIDs.Bloodletter,
        ActionIDs.HeavyShot, ActionIDs.Barrage,
        ActionIDs.RefulgentArrow, ActionIDs.Sidewinder,
        ActionIDs.HeavyShot,
        ActionIDs.HeavyShot,
        ActionIDs.HeavyShot,
        ActionIDs.HeavyShot, ActionIDs.EmpyrealArrow
    })]
    [InlineData(60, true, new[]
    {
        ActionIDs.Windbite, ActionIDs.TheWanderersMinuet,
        ActionIDs.VenomousBite, ActionIDs.RagingStrikes,
        ActionIDs.HeavyShot, ActionIDs.EmpyrealArrow,
        ActionIDs.HeavyShot, ActionIDs.Bloodletter,
        ActionIDs.HeavyShot, ActionIDs.Barrage,
        ActionIDs.StraightShot, ActionIDs.Sidewinder,
        ActionIDs.HeavyShot,
        ActionIDs.HeavyShot,
        ActionIDs.HeavyShot,
        ActionIDs.HeavyShot, ActionIDs.EmpyrealArrow
    })]
    [InlineData(50, true, new[]
    {
        ActionIDs.Windbite, ActionIDs.MagesBallad,
        ActionIDs.VenomousBite, ActionIDs.RagingStrikes,
        ActionIDs.HeavyShot, ActionIDs.Bloodletter,
        ActionIDs.HeavyShot, ActionIDs.Barrage,
        ActionIDs.StraightShot,
        ActionIDs.HeavyShot
    })]
    public void Bard_SingleTarget(int level, bool isBoss, ActionIDs[] expectedActions) =>
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);

    
    [Fact]
    public void CappedRepertoire_ShouldUsePitchPerfect()
    {
        JobGauge = new JobGaugeBRD
        {
            Song = Song.WANDERER,
            SongTimer = 35,
            Repertoire = 3
        };
        JobDefinition = new TestJobDefinitionBRD(new TestJobGaugeBRD((JobGaugeBRD) JobGauge));
        
        SetupSetConductor(SingleTarget!);
        MockService.SetupInitial();
        MockService.Setup(a => a.TargetHelper.IsTargetBossMob()).Returns(false);

        var priorities = SetConductor!.DeterminePriorities();
        
        Assert.Equal(priorities![1].ActionId, (int) ActionIDs.PitchPerfect);
    }

    [Fact]
    public void LosingMinuet_ShouldUseBallad()
    {
        JobGauge = new JobGaugeBRD
        {
            Song = Song.WANDERER,
            SongTimer = 0.5
        };
        JobDefinition = new TestJobDefinitionBRD(new TestJobGaugeBRD((JobGaugeBRD) JobGauge));
        
        SetupSetConductor(SingleTarget!);
        MockService.SetupInitial();
        MockService.Setup(a => a.TargetHelper.IsTargetBossMob()).Returns(false);
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint) ActionIDs.TheWanderersMinuet)).Returns(60);

        var priorities = SetConductor!.DeterminePriorities();
        
        Assert.Equal(priorities![1].ActionId, (int) ActionIDs.MagesBallad);
    }
    
    [Fact]
    public void LosingBallad_ShouldUsePaeon()
    {
        JobGauge = new JobGaugeBRD
        {
            Song = Song.MAGE,
            SongTimer = 0.5
        };
        JobDefinition = new TestJobDefinitionBRD(new TestJobGaugeBRD((JobGaugeBRD) JobGauge));
        
        SetupSetConductor(SingleTarget!);
        MockService.SetupInitial();
        MockService.Setup(a => a.TargetHelper.IsTargetBossMob()).Returns(false);
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint) ActionIDs.TheWanderersMinuet)).Returns(60);
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint) ActionIDs.MagesBallad)).Returns(12);

        
        var priorities = SetConductor!.DeterminePriorities();

        Assert.Equal(priorities![1].ActionId, (int) ActionIDs.ArmysPaeon);
    }

    [Theory]
    [InlineData(90, true, true)]
    [InlineData(80, true, false)]
    [InlineData(70, false, false)]
    public void GainingRequiredSoulVoice_ShouldUseApexBlastCombo(int level, bool shouldApex, bool shouldBlast)
    {
        JobGauge = new JobGaugeBRD
        {
            SoulVoice = 80,
            Song = Song.WANDERER,
            SongTimer = 20
        };
        JobDefinition = new TestJobDefinitionBRD(new TestJobGaugeBRD((JobGaugeBRD) JobGauge));
        
        SetupSetConductor(SingleTarget!);
        MockService.SetupInitial(level);
        MockService.Setup(a => a.TargetHelper.IsTargetBossMob()).Returns(false);
        
        var priorities = SetConductor!.DeterminePriorities();

        Assert.True(priorities![0].ActionId == (int) ActionIDs.ApexArrow == shouldApex);
        Assert.True(priorities[2].ActionId == (int) ActionIDs.BlastArrow == shouldBlast);
    }

    [Theory]
    [InlineData(90, true, 4.5f, 4.5f)]
    [InlineData(80, true, 4.5f, 4.5f)]
    [InlineData(70, true, 4.5f, 4.5f)]
    [InlineData(60, true, 4.5f, 4.5f)]
    [InlineData(50, false, 4.5f, 4.5f)]
    [InlineData(90, false, 4.5f, 0f)]
    [InlineData(90, false, 0f, 4.5f)]
    [InlineData(60, false, 0f, 4.5f)]
    [InlineData(60, false, 4.5f, 0f)]
    [InlineData(90, false, 0f, 0f)]
    [InlineData(60, false, 0f, 0f)]
    public void DamageOverTimeSpells_ShouldBeRefreshed(int level, bool expectedUseIronJaws, float windDotTime, float poisonDotTime)
    {
        SetupSetConductor(SingleTarget!);
        MockService.SetupInitial(level);

        List<StatusEffect> statuses;
        if (level < 64)
        {
            statuses = new List<StatusEffect>
            {
                new()
                {
                    Id = 124,
                    RemainingTime = poisonDotTime
                },
                new()
                {
                    Id = 129,
                    RemainingTime = windDotTime
                }
            };
        }
        else
        {
            statuses = new List<StatusEffect>
            {
                new()
                {
                    Id = 1200,
                    RemainingTime = poisonDotTime
                },
                new()
                {
                    Id = 1201,
                    RemainingTime = windDotTime
                }
            };
        }

        MockService.Setup(a => a.TargetHelper.IsTargetBossMob()).Returns(true);
        MockService.Setup(a => a.TargetHelper.GetCurrentStatuses()).Returns(statuses);
        
        var priorities = SetConductor!.DeterminePriorities();

        var initialActionId = priorities![0].ActionId;
        var secondaryAction = priorities[2].ActionId;
        
        Assert.True(initialActionId == (int) ActionIDs.IronJaws == expectedUseIronJaws);

        if (expectedUseIronJaws) return;
        
        // Both dots down
        if (windDotTime == 0f && poisonDotTime == 0f)
        {
            Assert.True(level < 64 ? initialActionId == (int) ActionIDs.Windbite : initialActionId == (int) ActionIDs.Stormbite);
            Assert.True(level < 64 ? secondaryAction == (int) ActionIDs.VenomousBite : secondaryAction == (int) ActionIDs.CausticBite);
            return;
        }

        // Poison down
        if (poisonDotTime == 0f)
            Assert.True(level < 64 ? initialActionId == (int) ActionIDs.VenomousBite : initialActionId == (int) ActionIDs.CausticBite);
        
        // Wind down
        if (windDotTime == 0f)
            Assert.True(level < 64 ? initialActionId == (int) ActionIDs.Windbite : initialActionId == (int) ActionIDs.Stormbite);
    }
}