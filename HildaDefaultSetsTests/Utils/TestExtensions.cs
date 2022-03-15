using System.Collections.Generic;
using System.Linq;
using Hilda.Constants;
using Hilda.Models;
using Hilda.Models.RequirementTypes;

namespace HildaDefaultSetsTests.Utils;

public static class TestExtensions
{
    public static IEnumerable<uint> GetActionIds(this IEnumerable<IPriority<JobRequirements>> priorities) =>
        priorities.Select(p => (uint) p.ActionId);

    public static IEnumerable<uint> GetActionIds(this IEnumerable<ActionIDs> actions) =>
        actions.Select(a => (uint) a);
}