using Hilda;
using Hilda.Conductors.JobDefinitions;
using Hilda.Conductors.JobDefinitions.MagicDps;
using Hilda.Conductors.JobDefinitions.MeleeDps;
using Hilda.Conductors.JobDefinitions.RangedDps;
using Hilda.Conductors.JobDefinitions.Tanks;

namespace HildaDefaultSetsTests.Utils;


public record TestJobGaugeBLM : JobGaugeBLM
{
    public TestJobGaugeBLM(JobGaugeBLM original) : base(original)
    {
    }

    public override JobGauge FromDalamud(IService service)
    {
        return new TestJobGaugeBLM(this);
    }
}

internal class TestJobDefinitionBLM : JobDefinitionBLM
{
    private readonly TestJobGaugeBLM _dalamudValues;

    public TestJobDefinitionBLM(TestJobGaugeBLM dalamudValues)
    {
        _dalamudValues = dalamudValues;
    }

    public override JobGauge Gauge => _dalamudValues;
}

public record TestJobGaugeRDM : JobGaugeRDM
{
    public TestJobGaugeRDM() { }
    public TestJobGaugeRDM(JobGaugeRDM original) : base(original) { }

    public override JobGauge FromDalamud(IService service)
    {
        return new TestJobGaugeRDM(this);
    }
}

internal class TestJobDefinitionRDM : JobDefinitionRDM
{
    private readonly TestJobGaugeRDM _dalamudValues;

    public TestJobDefinitionRDM(TestJobGaugeRDM dalamudValues)
    {
        _dalamudValues = dalamudValues;
    }

    public override JobGauge Gauge => _dalamudValues;
}

public record TestJobGaugeMNK : JobGaugeMNK
{
    public TestJobGaugeMNK(JobGaugeMNK original) : base(original)
    {
    }

    public override JobGauge FromDalamud(IService service)
    {
        return new TestJobGaugeMNK(this);
    }
}

internal class TestJobDefinitionMNK : JobDefinitionMNK
{
    private readonly TestJobGaugeMNK _dalamudValues;

    public TestJobDefinitionMNK(TestJobGaugeMNK dalamudValues)
    {
        _dalamudValues = dalamudValues;
    }

    public override JobGauge Gauge => _dalamudValues;
}

public record TestJobGaugeDRG : JobGaugeDRG
{
    public TestJobGaugeDRG(JobGaugeDRG original) : base(original)
    {
    }

    public override JobGauge FromDalamud(IService service)
    {
        return new TestJobGaugeDRG(this);
    }
}

internal class TestJobDefinitionDRG : JobDefinitionDRG
{
    private readonly TestJobGaugeDRG _dalamudValues;

    public TestJobDefinitionDRG(TestJobGaugeDRG dalamudValues)
    {
        _dalamudValues = dalamudValues;
    }

    public override JobGauge Gauge => _dalamudValues;
}

public record TestJobGaugePLD : JobGaugePLD
{
    public TestJobGaugePLD(JobGaugePLD original) : base(original)
    {
    }

    public override JobGauge FromDalamud(IService service)
    {
        return new TestJobGaugePLD(this);
    }
}

internal class TestJobDefinitionPLD : JobDefinitionPLD
{
    private readonly TestJobGaugePLD _dalamudValues;

    public TestJobDefinitionPLD(TestJobGaugePLD dalamudValues)
    {
        _dalamudValues = dalamudValues;
    }

    public override JobGauge Gauge => _dalamudValues;
}

public record TestJobGaugeMCH : JobGaugeMCH
{
    public TestJobGaugeMCH(JobGaugeMCH original) : base(original)
    {
    }

    public override JobGauge FromDalamud(IService service)
    {
        return new TestJobGaugeMCH(this);
    }
}

internal class TestJobDefinitionMCH : JobDefinitionMCH
{
    private readonly TestJobGaugeMCH _dalamudValues;

    public TestJobDefinitionMCH(TestJobGaugeMCH dalamudValues)
    {
        _dalamudValues = dalamudValues;
    }

    public override JobGauge Gauge => _dalamudValues;
}