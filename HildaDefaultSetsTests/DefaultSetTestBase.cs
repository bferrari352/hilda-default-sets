using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Hilda;
using Hilda.Conductors;
using Hilda.Conductors.JobDefinitions;
using Hilda.Constants;
using Hilda.Displays.Configuration;
using Hilda.Displays.InCombat;
using Hilda.Managers;
using Hilda.Models;
using Hilda.Models.RequirementTypes;
using HildaTestUtils;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests;

[Collection(LuminaCollection.Key)]
public class DefaultSetTestBase : IClassFixture<TestBaseFixture>
{
    public const string OutOfDate = "Out of Date";
        
    private readonly TestBaseFixture _fixture;
    
    protected readonly ITestOutputHelper Output;

    protected readonly Mock<IService> MockService;
    
    protected int QueueSize = 10;
    protected ICombatCamera<JobRequirements>? CombatCamera;
    
    protected JobGauge JobGauge = null!;
    protected IJobDefinition<JobRequirements, JobRequirements> JobDefinition;
    protected readonly SetConfig SetConfig;
    
    internal SetConductor<JobRequirements> SetConductor;

    protected Dictionary<string, IPrioritySet<JobRequirements>>? JobSets;
    protected IPrioritySet<JobRequirements>? CurrentSet;

    protected DefaultSetTestBase(TestBaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        Output = output;

        MockService = MockLibrary.MockService();
        JobDefinition = new JobDefinition<JobRequirements, JobGauge>();
        SetConfig = new SetConfig
        {
            ShowOutOfCombat = new (true, ConfigSource.Global)
        };
    }

    protected void SetupSetConductor(IPrioritySet<JobRequirements> set, ICombatCamera<JobRequirements>? combatCamera = null)
    {
        CombatCamera = combatCamera ?? new CombatCamera<JobRequirements>(MockService.Object, JobDefinition);
        var window = new PrioritySetWindow(MockService.Object, new Guid(), "Window");
        SetConductor = new SetConductor<JobRequirements>(MockService.Object, window, CombatCamera, set, JobDefinition, SetConfig);
    }

    protected IEnumerable<Set> GetDefaultSets(JobData job)
    {
        var defaultSets = LuminaDataAccess.GetAllDefaultSets();
        return defaultSets.FindAll(x => x.JobId == job.Id);
    }

    protected void SetJobSets(JobData job)
    {
        JobSets = new();
        var sets = GetDefaultSets(job);
        foreach (var set in sets)
        {
            JobSets[set.Name] = set.Priorities;
        }
    }

    protected void SingleTarget_BasicRotation_ReturnsExpectedValues(int level, bool isBoss, ActionIDs[] expectedActions,
        int spellSpeed = 0, int skillSpeed = 0)
    {
        // Setup
        QueueSize = expectedActions.Length;

        var singleTarget = JobSets?[DefaultSets.Get(DefaultSets.DisplayType.Single)];
        if (singleTarget == null) return;
        CurrentSet = singleTarget;
        
        SetupSetConductor(CurrentSet);
        
        MockService.SetupInitial(level, spellSpeed, skillSpeed);
        MockService.Setup(a => a.TargetHelper.IsTargetBossMob()).Returns(isBoss);

        // Get Priorities
        var priorities = SetConductor.Update(SetConfig);
        var priorityIds = priorities!.GetActionIds().ToList();

        // Validate
        var incorrectActions = new Dictionary<int, (uint, uint)>();
        for (var index = 0; index < expectedActions.Length; index++)
        {
            var expectedId = (uint) expectedActions[index];
            var actualId = priorityIds[index];
            if (!expectedId.Equals(actualId))
            {
                incorrectActions.Add(index, (expectedId, actualId));
            }
        }

        // Logging
        if (incorrectActions.Count > 0)
        {
            var outputStr = $"<< Incorrect Actions (Level:{level}) (IsBossTest:{isBoss}) >>\n";
            foreach (var (index, (expected, actual)) in incorrectActions)
            {
                outputStr += $"::: [Action #{index+1} -> Expected: ({(ActionIDs) expected}) - Actual: ({(ActionIDs) actual})] :::\n";
            }
            Output.WriteLine(outputStr);
        }
        
        // Assert
        priorityIds.Should().Equal(expectedActions.GetActionIds());
    }
}

public class TestBaseFixture : IDisposable
{
    public TestBaseFixture()
    {
        if (LuminaDataAccess.IsCacheReady()) return;
        var mockStatusHelper = MockLibrary.MockStatusHelper();
        var mockDataHelper = MockLibrary.MockDataHelper();
        LuminaDataAccess.BuildJobActionCache(mockStatusHelper.Object, mockDataHelper);
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

[CollectionDefinition(Key)]
public class LuminaCollection : ICollectionFixture<TestBaseFixture> {
    public const string Key = "LuminaCollection";
}