using System.Collections.Generic;
using Hilda.Constants;

namespace HildaDefaultSetsTests;

public class SetTestData
{
    public int Level { get; set; }
    public bool IsBoss { get; set; }
    public HashSet<ActionIDs> Priorities { get; set; }
}