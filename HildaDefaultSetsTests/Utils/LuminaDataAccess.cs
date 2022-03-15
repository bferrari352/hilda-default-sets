using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hilda.Conductors.JobDefinitions.Healers.Actions;
using Hilda.Conductors.JobDefinitions.MagicDps.Actions;
using Hilda.Conductors.JobDefinitions.MeleeDps.Actions;
using Hilda.Conductors.JobDefinitions.RangedDps.Actions;
using Hilda.Conductors.JobDefinitions.Tanks.Actions;
using Hilda.Constants;
using Hilda.Helpers;
using Hilda.Models;
using Lumina.Excel.GeneratedSheets;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace HildaDefaultSetsTests.Utils;

public static class LuminaDataAccess
{
    private const string XivPathVar = "xivinstall";
    private const string SqPackPath = "game/sqpack";

    private static Lumina.GameData? _lumina;
    private static Lumina.GameData Lumina
    {
        get
        {
            if (_lumina != null) return _lumina;
            
            var xivLoc = Environment.GetEnvironmentVariable(XivPathVar);
            var path = Path.Combine(xivLoc!, SqPackPath);
            _lumina = new(path);
            return _lumina;
        }
    }

    #region JobActionCache Data

    private static readonly ConcurrentDictionary<int, uint> ActionsByJobIdCache = new();
    private static readonly ConcurrentDictionary<uint, Dictionary<int, JobAction>> ActionsCache = new();

    #endregion

    #region StatusHelper

    public static Status? GetStatus(uint statusId)
    {
        return Lumina.GetExcelSheet<Status>()?.GetRow(statusId);
    }

    #endregion

    #region JobActionCache

    public static bool IsCacheReady()
    {
        return ActionsCache.Count > 0;
    }

    public static JobAction? GetJobActionFromCacheById(int actionId)
    {
        var jobId = ActionsByJobIdCache.GetValueOrDefault(actionId);
        if (ActionsCache.ContainsKey(jobId) && ActionsCache[jobId].TryGetValue(actionId, out var jobAction))
        {
            return jobAction;
        }

        return null;
    }

    public static Dictionary<int, JobAction>? GetJobActionsFromCacheByJobId(uint jobId) =>
        ActionsCache.GetValueOrDefault(jobId);

    public static void BuildJobActionCache(IStatusHelper mockStatusHelper)
    {
        var allActions = Lumina.GetExcelSheet<Action>();
        var allIndirectActions = Lumina.GetExcelSheet<ActionIndirection>();

        if (allActions == null || allIndirectActions == null) return;

        foreach (var job in JobData.AllJobs)
        {
            var jobActions = allActions.Where(a => a.ClassJob?.Value?.RowId == job.Id).ToList();
            var indirect = allIndirectActions.Where(a => a.ClassJob?.Value?.RowId == job.Id);

            jobActions = jobActions.Concat(indirect
                    .Where(x => x.Name.Value != null)
                    .Select(x => x.Name.Value!))
                .ToList();

            if (job.UpgradesFrom != null)
            {
                var baseActions = allActions.Where(a => a.ClassJob?.Value?.RowId == (uint)job.UpgradesFrom);
                jobActions = jobActions.Concat(baseActions).ToList();

                var baseIndirect = allIndirectActions.Where(a => a.ClassJob?.Value?.RowId == (uint)job.UpgradesFrom);
                jobActions = jobActions.Concat(baseIndirect
                    .Where(x => x.Name.Value != null)
                    .Select(x => x.Name.Value!)).ToList();
            }

            var roleActionIds = new List<uint>();
            switch ((RoleIds)job.RoleId)
            {
                case RoleIds.Tank:
                    roleActionIds = ActionsTank.RoleActions;
                    break;
                case RoleIds.MagicDPS:
                    roleActionIds = ActionsMagic.RoleActions;
                    break;
                case RoleIds.Healer:
                    roleActionIds = ActionsHealer.RoleActions;
                    break;
                case RoleIds.MeleeDPS:
                    roleActionIds = ActionsMelee.RoleActions;
                    break;
                case RoleIds.RangedDPS:
                    roleActionIds = ActionsRanged.RoleActions;
                    break;
                case RoleIds.None:
                    break;
                default:
                    break;
            }

            foreach (var actionId in roleActionIds)
            {
                var roleAction = allActions.GetRow(actionId);
                if (roleAction != null)
                {
                    jobActions.Add(roleAction);
                }
            }

            var actionRequirements = job.Definition?.GetActionRequirements();

            var builtActions = new Dictionary<int, JobAction>();
            foreach (var action in jobActions.OrderBy(a => (string)a.Name))
            {
                ActionRequirements? actionRequirement = null;
                actionRequirements?.TryGetValue(action.RowId, out actionRequirement);
                if (!action.IsPvP)
                {
                    var jobAction = new JobAction(mockStatusHelper, action, (RoleIds)job.RoleId, "", actionRequirement);
                    builtActions.Add((int)action.RowId, jobAction);
                }
            }

            // add to cache
            ActionsCache[job.Id] = builtActions;

            foreach (var (id, _) in builtActions)
            {
                ActionsByJobIdCache[id] = job.Id;
            }

        }
    }

    #endregion
}