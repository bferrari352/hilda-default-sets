using System;
using System.Collections.Generic;
using Hilda;
using Hilda.Conductors;
using Hilda.Conductors.JobDefinitions;
using Hilda.Constants;
using Hilda.Helpers;
using Hilda.Models;
using Hilda.Models.RequirementTypes;
using HildaDefaultSetsTests.Utils;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace HildaDefaultSetsTests;

[Collection(LuminaCollection.KEY)]
public class DefaultSetTestBase : IClassFixture<TestBaseFixture>
{
    private readonly TestBaseFixture _testBaseFixture;
    
    protected readonly ITestOutputHelper OutputHelper;

    protected Mock<IService>? MockService;
    
    protected int QueueSize = 10;
    protected ICombatCamera<JobRequirements>? CombatCamera;
    protected IJobDefinition<JobRequirements, JobRequirements>? JobDefinition;
    internal SetConductor<JobRequirements>? SetConductor;
    
    protected IPrioritySet<JobRequirements>? SingleTarget;
    protected IPrioritySet<JobRequirements>? MultiTarget;

    protected DefaultSetTestBase(TestBaseFixture testBaseFixture, ITestOutputHelper testOutputHelper)
    {
        _testBaseFixture = testBaseFixture;
        OutputHelper = testOutputHelper;
    }

    protected void SetupSetConductor(IPrioritySet<JobRequirements> set, ICombatCamera<JobRequirements>? combatCamera = null)
    {
        if (MockService == null || JobDefinition == null) return;
        
        CombatCamera = combatCamera ?? new CombatCamera<JobRequirements>(MockService.Object, JobDefinition);
        SetConductor = new SetConductor<JobRequirements>(MockService.Object, CombatCamera, set, JobDefinition, QueueSize);
    }

    protected IEnumerable<IPrioritySet<JobRequirements>>? GetDefaultSets(JobData job)
    {
        if (MockService == null) return null;
        
        var dataHelper = new DataHelper(MockService.Object);
        return dataHelper.GetDefaultPrioritySets(job);
    }
}

public class TestBaseFixture : IDisposable
{
    public TestBaseFixture()
    {
        if (!LuminaDataAccess.IsCacheReady())
        {
            var mockStatusHelper = MockLibrary.MockStatusHelper();
            LuminaDataAccess.BuildJobActionCache(mockStatusHelper.Object);   
        }
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

[CollectionDefinition(KEY)]
public class LuminaCollection : ICollectionFixture<TestBaseFixture> {
    public const string KEY = "LuminaCollection";
}