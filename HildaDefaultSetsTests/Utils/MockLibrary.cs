using Dalamud.Game.ClientState.JobGauge.Types;
using Hilda;
using Hilda.Caches;
using Hilda.Helpers;
using Hilda.Models;
using Hilda.Models.RequirementTypes;
using Moq;

namespace HildaDefaultSetsTests.Utils;

public static class MockLibrary
{
    public static Mock<PriorityContext> PriorityContext()
    {
        return new Mock<PriorityContext>();
    }

    public static Mock<JobAction> CreateJobAction(uint actionId)
    {
        var mock = new Mock<JobAction>
        {
            Object =
            {
                Id = (int) actionId
            }
        };
        return mock;
    }
    
    public static Mock<IService> MockService()
    {
        var mock = new Mock<IService>();
        mock.Setup(a => a.PluginInterface).Returns(MockPluginInterface().Object);
        mock.Setup(a => a.StatusHelper).Returns(MockStatusHelper().Object);
        mock.Setup(a => a.JobActionCache).Returns(MockJobActionCache(mock.Object.StatusHelper).Object);
        mock.Setup(a => a.UiBuilder).Returns(MockUIBuilder().Object);
        mock.Setup(a => a.ActionHelper).Returns(MockActionHelper().Object);
        mock.Setup(a => a.Configuration).Returns(MockConfiguration().Object);
        mock.Setup(a => a.PlayerCharacterHelper).Returns(MockPlayerCharacterHelper().Object);
        mock.Setup(a => a.TargetHelper).Returns(MockTargetHelper().Object);
        mock.Setup(a => a.EnemyListHelper).Returns(MockEnemyListHelper().Object);
        mock.Setup(a => a.WeaveHelper).Returns(new WeaveHelper());
        mock.Setup(a => a.ManaHelper).Returns(MockManaHelper().Object);
        
        return mock;
    }
    
    public static Mock<IService> MockService<T, TU>() where T: IGauge<TU>, new() where TU: JobGaugeBase
    {
        var mock = MockService();
        mock.Setup(a => a.JobGauges).Returns(MockJobGauges<T, TU>().Object);
        
        return mock;
    }

    public static void SetupInitial(this Mock<IService> service, int level = 90)
    {
        service.Setup(a => a.PlayerCharacterHelper.GetLevel()).Returns(level);
        service.Setup(a => a.PlayerCharacterHelper.GetCurrentMp()).Returns(10000);
        service.Setup(a => a.PlayerCharacterHelper.GetSpellSpeed()).Returns(830);
        service.Setup(a => a.PlayerCharacterHelper.GetSkillSpeed()).Returns(400);
        service.Setup(a => a.ManaHelper.PlayerMana).Returns(10000);
        service.Setup(a => a.ManaHelper.GetSnapshot()).Returns(new RegisteredManaHelper());
    }

    #region IService Mocks

    public static Mock<IJobGauges> MockJobGauges<T, TU>() where T: IGauge<TU>, new() where TU: JobGaugeBase
    {
        var mock = new Mock<IJobGauges>();
        
        mock.Setup(a => a.Get<T, TU>()).Returns(new T());

        return mock;
    }
    
    public static Mock<IManaHelper> MockManaHelper()
    {
        var mock = new Mock<IManaHelper>();

        return mock;
    }

    public static Mock<IWeaveHelper> MockWeaveHelper()
    {
        var mock = new Mock<IWeaveHelper>();
        mock.Setup(x => x.OnActionUsed(It.IsAny<JobAction>())).Callback((JobAction action) =>
        {
            if (action.IsWeavable)
            {
                mock.Setup(a => a.WeavesUsed).Returns(1);
            }
        });

        return mock;
    }
    
    public static Mock<IEnemyListHelper> MockEnemyListHelper()
    {
        var mock = new Mock<IEnemyListHelper>();

        return mock;
    }
    
    public static Mock<ITargetHelper> MockTargetHelper()
    {
        var mock = new Mock<ITargetHelper>();

        return mock;
    }
    
    public static Mock<IConfiguration> MockConfiguration()
    {
        var mock = new Mock<IConfiguration>();

        return mock;
    }

    public static Mock<IPlayerCharacterHelper> MockPlayerCharacterHelper()
    {
        var mock = new Mock<IPlayerCharacterHelper>();

        return mock;
    }
    
    public static Mock<IUiBuilder> MockUIBuilder()
    {
        var mock = new Mock<IUiBuilder>();

        return mock;
    }

    public static Mock<IPluginInterface> MockPluginInterface()
    {
        var mock = new Mock<IPluginInterface>();
        mock.Setup(x => x.GetPluginConfigDirectory()).Returns("directory");

        return mock;
    }

    public static Mock<IJobActionCache> MockJobActionCache(IStatusHelper mockStatusHelper)
    {
        var mock = new Mock<IJobActionCache>();
        mock.Setup(x => x.GetJobActionFromCacheById(It.IsAny<int>()))
            .Returns((int actionId) => LuminaDataAccess.GetJobActionFromCacheById(actionId));

        mock.Setup(x => x.GetJobActionsFromCacheByJobId(It.IsAny<uint>()))
            .Returns((uint jobId) => LuminaDataAccess.GetJobActionsFromCacheByJobId(jobId));

        return mock;
    }

    public static Mock<IStatusHelper> MockStatusHelper()
    {
        var mock = new Mock<IStatusHelper>();
        mock.Setup(x => x.GetStatus(It.IsAny<uint>())).Returns((uint statusId) =>
            LuminaDataAccess.GetStatus(statusId));

        return mock;
    }

    public static Mock<IActionHelper> MockActionHelper()
    {
        var mock = new Mock<IActionHelper>();
        mock.Setup(x => x.HasResources(It.IsAny<uint>())).Returns(true);

        return mock;
    }

    #endregion


    #region Combat Camera

    public static Mock<ICombatCamera<JobRequirements>> MockCombatCamera()
    {
        var mock = new Mock<ICombatCamera<JobRequirements>>();

        return mock;
    }
    
    #endregion
}