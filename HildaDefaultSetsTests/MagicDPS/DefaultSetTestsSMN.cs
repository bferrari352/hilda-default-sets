using System;
using System.Collections.Generic;
using System.Linq;
using Hilda;
using Hilda.Conductors.JobDefinitions.MagicDps;
using Hilda.Conductors.JobDefinitions.MagicDps.Actions;
using Hilda.Constants;
using Hilda.Models;
using HildaTestUtils;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests.MagicDPS;

public class DefaultSetTestsSMN : DefaultSetTestBase
{
    public DefaultSetTestsSMN(TestBaseFixture fixture, ITestOutputHelper output) :
        base(fixture, output)
    {
        setupJobGauge();

        var sets = GetDefaultSets(JobData.Summoner)?.ToList();
        if (sets == null) return;

        SingleTarget = sets.FirstOrDefault(s => s.Name!.Equals("Single Target"));
        MultiTarget = sets.FirstOrDefault(s => s.Name!.Equals("Multi Target"));
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.SummonCarbuncle, ActionIDs.EnergyDrainSMN,
        ActionIDs.SummonBahamut, ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.SummonTitanII,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.SummonGarudaII,
        ActionIDs.Slipstream,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.SummonIfritII,
        ActionIDs.RubyRite,
        ActionIDs.RubyRite,
        ActionIDs.CrimsonCyclone,
        ActionIDs.CrimsonStrike,
        ActionIDs.RuinIV,
        ActionIDs.RuinIII, //this ruin does not reflect reality
        ActionIDs.RuinIII, ActionIDs.EnergyDrainSMN,
        ActionIDs.SummonPhoenix, ActionIDs.EnkindlePhoenix,
        ActionIDs.FountainOfFire, ActionIDs.Fester,
        ActionIDs.FountainOfFire, ActionIDs.Fester,
        ActionIDs.FountainOfFire,
        ActionIDs.FountainOfFire,
        ActionIDs.FountainOfFire,
        ActionIDs.FountainOfFire,
        ActionIDs.SummonTitanII,
    })]
    public void Summoner_SingleTarget_Full(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.SummonBahamut, ActionIDs.EnergyDrainSMN,
        ActionIDs.AstralImpulse, ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.SummonTitanII,
    })]
    [InlineData(80, false, new[] {
        ActionIDs.SummonBahamut, ActionIDs.EnergyDrainSMN,
        ActionIDs.AstralImpulse,  ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse,  ActionIDs.Deathflare,
        ActionIDs.AstralImpulse,  ActionIDs.Fester,
        ActionIDs.AstralImpulse,  ActionIDs.Fester,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.SummonTitan,
    })]
    [InlineData(70, false, new[] {
        ActionIDs.SummonBahamut, ActionIDs.EnergyDrainSMN,
        ActionIDs.AstralImpulse, ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.SummonTitan,
    })]
    [InlineData(60, false, new[] {
        ActionIDs.DreadwyrmTrance, ActionIDs.EnergyDrainSMN,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.SummonTitan,
    })]
    [InlineData(50, false, new[] {
        ActionIDs.Aethercharge, ActionIDs.EnergyDrainSMN,
        ActionIDs.RuinII, ActionIDs.Fester,
        ActionIDs.RuinII, ActionIDs.Fester,
        ActionIDs.RuinII,
        ActionIDs.RuinII,
        ActionIDs.RuinII,
        ActionIDs.RuinII,
        ActionIDs.SummonTitan,
    })]
    [InlineData(30, false, new[] {
        ActionIDs.Aethercharge, ActionIDs.EnergyDrainSMN,
        ActionIDs.RuinII, ActionIDs.Fester,
        ActionIDs.RuinII, ActionIDs.Fester,
        ActionIDs.RuinII,
        ActionIDs.RuinII,
        ActionIDs.RuinII,
        ActionIDs.RuinII,
        ActionIDs.SummonTopaz,
    })]
    public void Summoner_SingleTarget_Opener(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupJobGauge(jobGauge => jobGauge.IsCarbuncleSummoned = true);

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.SummonTitanII,
    }, 4.5, 0)]
    [InlineData(90, false, new[] {
        ActionIDs.AstralImpulse,
        ActionIDs.SummonTitanII,
    }, 2, 0)]
    [InlineData(90, false, new[] {
        ActionIDs.SummonTitanII,
    }, 2, 2.5)]
    public void Summoner_SingleTarget_CorrectNumberOfTranceRuins(int level, bool isBoss, ActionIDs[] expectedActions, float summonTimerRemaining, float activeGcdRemaining)
    {
        QueueSize = expectedActions.Length;
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.EnkindleBahamut)).Returns(20);
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.Deathflare)).Returns(20);
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.Ruin)).Returns(activeGcdRemaining);
        setupBahamutTrance(jobGauge => jobGauge.SummonTimerRemaining =summonTimerRemaining);
        setupEnergyDrainUsed(jobGauge => jobGauge.AetherflowStacks = 0);

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.SummonBahamut, ActionIDs.EnergyDrainSMN, ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
    })]
    public void Summoner_SingleTarget_Opener_DoubleWeave(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupJobGauge(jobGauge => jobGauge.IsCarbuncleSummoned = true);
        MockService.SetupConfig(config => config.WeaveLimit = 2);

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.AstralImpulse, ActionIDs.EnkindleBahamut, ActionIDs.Deathflare,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.Fester,
    })]
    public void Summoner_SingleTarget_Opener_DoubleWeave_EnergyDrainUsed(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupBahamutTrance();
        setupEnergyDrainUsed();
        MockService.SetupConfig(config => config.WeaveLimit = 2);

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.SummonIfritII,
        ActionIDs.RubyRite,
        ActionIDs.RubyRite,
        ActionIDs.CrimsonCyclone,
        ActionIDs.CrimsonStrike,
    })]
    [InlineData(80, false, new[] {
        ActionIDs.SummonIfrit,
        ActionIDs.RubyRite,
        ActionIDs.RubyRite,
    })]
    [InlineData(70, false, new[] {
        ActionIDs.SummonIfrit,
        ActionIDs.RubyRuinIII,
        ActionIDs.RubyRuinIII,
    })]
    [InlineData(60, false, new[] {
        ActionIDs.SummonIfrit,
        ActionIDs.RubyRuinIII,
        ActionIDs.RubyRuinIII,
    })]
    [InlineData(50, false, new[] {
        ActionIDs.SummonIfrit,
        ActionIDs.RubyRuinII,
        ActionIDs.RubyRuinII,
    })]
    [InlineData(30, false, new[] {
        ActionIDs.SummonIfrit,
        ActionIDs.RubyRuinII,
        ActionIDs.RubyRuinII,
    })]
    public void Summoner_SingleTarget_RubyPhase(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupOpenerUsed(jobGauge => jobGauge.AttunementsReady = SMNAttunementFlags.Ruby);

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.RubyRite,
        ActionIDs.RubyRite,
        ActionIDs.CrimsonCyclone,
        ActionIDs.CrimsonStrike,
    })]
    public void Summoner_SingleTarget_RubyPhase_Opened(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupSummonUsed(SMNAttunementFlags.Ruby);

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.SummonTitanII,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
    })]
    [InlineData(80, false, new[] {
        ActionIDs.SummonTitan,
        ActionIDs.TopazRite,
        ActionIDs.TopazRite,
        ActionIDs.TopazRite,
        ActionIDs.TopazRite,
    })]
    [InlineData(70, false, new[] {
        ActionIDs.SummonTitan,
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
    })]
    [InlineData(60, false, new[] {
        ActionIDs.SummonTitan,
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
    })]
    [InlineData(50, false, new[] {
        ActionIDs.SummonTitan,
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
    })]
    [InlineData(30, false, new[] {
        ActionIDs.SummonTopaz,
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
    })]
    public void Summoner_SingleTarget_TopazPhase(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupOpenerUsed(jobGauge => jobGauge.AttunementsReady = SMNAttunementFlags.Topaz);

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
    })]
    [InlineData(80, false, new[] {
        ActionIDs.TopazRite,
        ActionIDs.TopazRite,
        ActionIDs.TopazRite,
        ActionIDs.TopazRite,
    })]
    [InlineData(70, false, new[] {
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
    })]
    [InlineData(60, false, new[] {
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
        ActionIDs.TopazRuinIII,
    })]
    [InlineData(50, false, new[] {
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
    })]
    [InlineData(30, false, new[] {
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
        ActionIDs.TopazRuinII,
    })]
    public void Summoner_SingleTarget_TopazPhase_Opened(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupSummonUsed(SMNAttunementFlags.Topaz);
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
    })]
    public void Summoner_SingleTarget_MountainBusterOverFester(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupSummonUsed(SMNAttunementFlags.Topaz);
        setupEnergyDrainUsed();
        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.SummonGarudaII,
        ActionIDs.Slipstream,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
    })]
    [InlineData(80, false, new[] {
        ActionIDs.SummonGaruda,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
    })]
    [InlineData(70, false, new[] {
        ActionIDs.SummonGaruda,
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
    })]
    [InlineData(60, false, new[] {
        ActionIDs.SummonGaruda,
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
    })]
    [InlineData(50, false, new[] {
        ActionIDs.SummonGaruda,
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
    })]
    [InlineData(30, false, new[] {
        ActionIDs.SummonEmerald,
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
    })]
    public void Summoner_SingleTarget_EmeraldPhase(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupOpenerUsed(jobGauge => jobGauge.AttunementsReady = SMNAttunementFlags.Emerald);

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.Slipstream,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
    })]
    [InlineData(80, false, new[] {
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
    })]
    [InlineData(70, false, new[] {
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
    })]
    [InlineData(60, false, new[] {
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
        ActionIDs.EmeraldRuinIII,
    })]
    [InlineData(50, false, new[] {
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
    })]
    [InlineData(30, false, new[] {
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
        ActionIDs.EmeraldRuinII,
    })]
    public void Summoner_SingleTarget_EmeraldPhase_Opened(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupSummonUsed(SMNAttunementFlags.Emerald);

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    private void setupJobGauge(Action<JobGaugeSMN>? setup = null, JobGaugeSMN? jobGauge = null)
    {
        if (JobGauge == null)
        {
            JobGauge = new JobGaugeSMN();
        }
        if (jobGauge == null)
        {
            jobGauge = new TestJobGaugeSMN((JobGaugeSMN)JobGauge);
        }
        setup?.Invoke(jobGauge);
        JobGauge = jobGauge;
        JobDefinition = new TestJobDefinitionSMN(new TestJobGaugeSMN(jobGauge));
    }

    private void setupOpenerUsed(Action<JobGaugeSMN>? setup = null)
    {
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.SummonBahamut)).Returns(60);
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.DreadwyrmTrance)).Returns(60);
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.Aethercharge)).Returns(60);
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.EnergyDrainSMN)).Returns(60);
        setupJobGauge(jobGauge =>
        {
            jobGauge.IsCarbuncleSummoned = true;
            setup?.Invoke(jobGauge);
        });
    }

    private void setupSummonUsed(SMNAttunementFlags summon)
    {
        setupOpenerUsed(jobGauge =>
        {
            int? statusId = summon switch
            {
                SMNAttunementFlags.Ruby => StatusesSMN.IfritsFavor.Id,
                SMNAttunementFlags.Emerald => StatusesSMN.GarudasFavor.Id,
                _ => null
            };
            if (statusId != null)
            {
                MockService.SetupPlayerStatus(new StatusEffect
                {
                    Id = statusId.Value,
                    IsPermanent = true,
                });
            }
            jobGauge.Attunement = summon;
            jobGauge.AttunementStacks = summon == SMNAttunementFlags.Ruby ? 2 : 4;
            jobGauge.AttunementDuration = 15;
            jobGauge.SummonTimerRemaining = 6.5;
            jobGauge.AttunementsReady = (SMNAttunementFlags.Ruby | SMNAttunementFlags.Topaz | SMNAttunementFlags.Emerald) & ~summon;
        });
    }

    private void setupBahamutTrance(Action<JobGaugeSMN>? setup = null)
    {
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.SummonBahamut)).Returns(60);
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.SummonPhoenix)).Returns(60);
        setupJobGauge(jobGauge =>
        {
            jobGauge.IsCarbuncleSummoned = true;
            jobGauge.SummonTimerRemaining = 15;
            jobGauge.AttunementsReady = SMNAttunementFlags.All;
            setup?.Invoke(jobGauge);
        });
    }

    private void setupEnergyDrainUsed(Action<JobGaugeSMN>? setup = null)
    {
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.EnergyDrainSMN)).Returns(60);
        setupJobGauge(jobGauge =>
        {
            jobGauge.AetherflowStacks = 2;
            setup?.Invoke(jobGauge);
        });
    }

}