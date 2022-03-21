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
        ActionIDs.SummonBahamut, ActionIDs.Fester, //TODO: prioritize enkindle+deathflare over fester in default set
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse, //TODO: This is one too many
        ActionIDs.SummonTitanII,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.TopazRite, ActionIDs.MountainBuster,
        ActionIDs.SummonIfritII,
        ActionIDs.RubyRite,
        ActionIDs.RubyRite,
        ActionIDs.CrimsonCyclone,
        ActionIDs.CrimsonStrike,
        ActionIDs.SummonGarudaII,
        ActionIDs.Slipstream,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.EmeraldRite,
        ActionIDs.RuinIV, ActionIDs.EnergyDrainSMN, // TODO: This test is failing. Ruin IV should be used before Energy Drain + Summon Phoenix.
        ActionIDs.RuinIII,
        ActionIDs.SummonPhoenix, ActionIDs.Fester,
        ActionIDs.FountainOfFire, ActionIDs.Fester,
        ActionIDs.FountainOfFire, ActionIDs.EnkindlePhoenix,
        ActionIDs.FountainOfFire,
        ActionIDs.FountainOfFire,
        ActionIDs.FountainOfFire,
        ActionIDs.FountainOfFire,
        ActionIDs.FountainOfFire, //TODO: This is one too many
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
        ActionIDs.AstralImpulse, ActionIDs.Fester, //TODO: prioritize enkindle+deathflare over fester in default set
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse, //TODO: This is one too many
        ActionIDs.SummonTitanII,
    })]
    [InlineData(80, false, new[] {
        ActionIDs.SummonBahamut, ActionIDs.EnergyDrainSMN,
        ActionIDs.AstralImpulse, ActionIDs.Fester, //TODO: prioritize enkindle+deathflare over fester in default set
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse, //TODO: This is one too many
        ActionIDs.SummonTitan,
    })]
    [InlineData(70, false, new[] {
        ActionIDs.SummonBahamut, ActionIDs.EnergyDrainSMN,
        ActionIDs.AstralImpulse, ActionIDs.Fester, //TODO: prioritize enkindle+deathflare over fester in default set
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse, //TODO: This is one too many
        ActionIDs.SummonTitan,
    })]
    [InlineData(60, false, new[] {
        ActionIDs.DreadwyrmTrance, ActionIDs.EnergyDrainSMN,
        ActionIDs.AstralImpulse, ActionIDs.Fester, //TODO: prioritize deathflare over fester in default set
        ActionIDs.AstralImpulse, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse,
        ActionIDs.AstralImpulse, //TODO: This is one too many
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
        ActionIDs.RuinII, //TODO: This is one too many
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
        ActionIDs.RuinII, //TODO: This is one too many
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
        ActionIDs.SummonIfritII,
        ActionIDs.RubyRite,
        ActionIDs.RubyRite,
        ActionIDs.CrimsonCyclone,
        ActionIDs.CrimsonStrike,
    })]
    //TODO add other levels
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
    //TODO - this test needs to be fixed at the priority set level
    public void Summoner_SingleTarget_RubyPhase_Opened(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupOpenerUsed(jobGauge =>
        {
            //TODO make this setup code reusable across summons
            var favor = new StatusEffect()
            {
                Id = (int)StatusIdsSMN.IfritsFavor,
                IsPermanent = true,
            };
            // TODO make status setup reusable
            MockService.Setup(a => a.PlayerCharacterHelper.GetCurrentStatuses()).Returns(new List<StatusEffect>() { favor });
            MockService.Setup(a => a.PlayerCharacterHelper.GetStatus((uint)StatusIdsSMN.GarudasFavor)).Returns(favor);
            jobGauge.Attunement = SMNAttunementFlags.Ruby;
            jobGauge.AttunementStacks = 4;
            jobGauge.AttunementDuration = 15000;
            jobGauge.SummonTimerRemaining = 7000;
            jobGauge.AttunementsReady = SMNAttunementFlags.Topaz | SMNAttunementFlags.Emerald;
        });

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
    //TODO add other levels
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
    public void Summoner_SingleTarget_TopazPhase_Opened(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupOpenerUsed(jobGauge =>
        {
            //Titan does not get a favor on summoning
            jobGauge.Attunement = SMNAttunementFlags.Topaz;
            jobGauge.AttunementStacks = 4;
            jobGauge.AttunementDuration = 15000;
            jobGauge.SummonTimerRemaining = 7000;
            jobGauge.AttunementsReady = SMNAttunementFlags.Ruby | SMNAttunementFlags.Emerald;
        });
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
    public void Summoner_SingleTarget_EmeraldPhase_Opened(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupOpenerUsed(jobGauge =>
        {
            //TODO make this setup code reusable across summons
            var favor = new StatusEffect()
            {
                Id = (int)StatusIdsSMN.GarudasFavor,
                IsPermanent = true,
            };
            // TODO make status setup reusable
            MockService.Setup(a => a.PlayerCharacterHelper.GetCurrentStatuses()).Returns(new List<StatusEffect>() { favor });
            MockService.Setup(a => a.PlayerCharacterHelper.GetStatus((uint)StatusIdsSMN.GarudasFavor)).Returns(favor);
            jobGauge.Attunement = SMNAttunementFlags.Emerald;
            jobGauge.AttunementStacks = 4;
            jobGauge.AttunementDuration = 15000;
            jobGauge.SummonTimerRemaining = 7000;
            jobGauge.AttunementsReady = SMNAttunementFlags.Ruby | SMNAttunementFlags.Topaz;
        });

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.SummonBahamut, ActionIDs.EnergyDrainSMN, ActionIDs.Fester,
        ActionIDs.AstralImpulse, ActionIDs.Fester, ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse, ActionIDs.Deathflare,
    })]
    public void Summoner_SingleTarget_Fester_NoLimits(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupJobGauge(jobGauge => jobGauge.IsCarbuncleSummoned = true);
        setupConfig(config => config.WeaveLimit = 2);

        SingleTarget_BasicRotation_ReturnsExpectedValues(level, isBoss, expectedActions);
    }

    [Theory]
    [InlineData(90, false, new[] {
        ActionIDs.AstralImpulse,  ActionIDs.Fester, ActionIDs.EnkindleBahamut,
        ActionIDs.AstralImpulse,  ActionIDs.Fester, ActionIDs.Deathflare,
    })]
    public void Summoner_SingleTarget_PerfectPingDoubleFester_EnergyDrainUsed(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        QueueSize = expectedActions.Length;
        setupBahamutTrance();
        setupEnergyDrainUsed();
        setupConfig(config => config.WeaveLimit = 2);

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
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.EnergyDrainSMN)).Returns(60);
        setupJobGauge(jobGauge =>
        {
            jobGauge.IsCarbuncleSummoned = true;
            setup?.Invoke(jobGauge);
        });
    }

    private void setupBahamutTrance(Action<JobGaugeSMN>? setup = null)
    {
        MockService.Setup(a => a.ActionHelper.GetActionRecast((uint)ActionIDs.SummonBahamut)).Returns(60);
        setupJobGauge(jobGauge =>
        {
            jobGauge.IsCarbuncleSummoned = true;
            jobGauge.SummonTimerRemaining = 15000;
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

    private void setupConfig(Action<ConfigurationSettings> setup)
    {
        var config = new Hilda.ConfigurationSettings();
        setup(config);
        MockService.Setup(a => a.Configuration.GlobalConfiguration).Returns(config);
    }

}