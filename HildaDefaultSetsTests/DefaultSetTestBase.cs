using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Hilda;
using Hilda.Conductors;
using Hilda.Conductors.JobDefinitions;
using Hilda.Constants;
using Hilda.Helpers;
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
    private readonly TestBaseFixture _fixture;
    
    protected readonly ITestOutputHelper Output;

    protected Mock<IService> MockService;
    
    protected int QueueSize = 10;
    protected ICombatCamera<JobRequirements>? CombatCamera;
    
    protected JobGauge JobGauge = null!;
    protected IJobDefinition<JobRequirements, JobRequirements> JobDefinition;
    
    
    internal SetConductor<JobRequirements>? SetConductor;
    
    protected IPrioritySet<JobRequirements>? SingleTarget;
    protected IPrioritySet<JobRequirements>? MultiTarget;

    protected DefaultSetTestBase(TestBaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        Output = output;

        MockService = MockLibrary.MockService();
        JobDefinition = new JobDefinition<JobRequirements, JobGauge>();
    }

    protected void SetupSetConductor(IPrioritySet<JobRequirements> set, ICombatCamera<JobRequirements>? combatCamera = null)
    {
        CombatCamera = combatCamera ?? new CombatCamera<JobRequirements>(MockService.Object, JobDefinition);
        SetConductor = new SetConductor<JobRequirements>(MockService.Object, CombatCamera, set, JobDefinition, QueueSize);
    }

    protected IEnumerable<IPrioritySet<JobRequirements>>? GetDefaultSets(JobData job)
    {
        var dataHelper = new DataHelper(MockService.Object);
        return dataHelper.GetDefaultPrioritySets(job);
    }

    protected void SingleTarget_BasicRotation_ReturnsExpectedValues(int level, bool isBoss, ActionIDs[] expectedActions)
    {
        // Setup
        SetupSetConductor(SingleTarget!);
        MockService.SetupInitial(level);
        MockService.Setup(a => a.TargetHelper.IsTargetBossMob()).Returns(isBoss);

        // Get Priorities
        var priorities = SetConductor!.DeterminePriorities();
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
                outputStr += $"::: [Position: {index} -> Expected: ({(ActionIDs) expected}) - Actual: ({(ActionIDs) actual})] :::\n";
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
        LuminaDataAccess.BuildJobActionCache(mockStatusHelper.Object);
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