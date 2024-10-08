using Robust.Shared.Prototypes;
using Content.Shared.Dataset;
using Robust.Shared.Random;

namespace Content.Server._Sunrise.NameGenerators;

/// <summary>
/// This handles...
/// </summary>
public sealed class ShuttleNameGeneratorSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly MetaDataSystem _meta = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<ShuttleNameGeneratorComponent, ComponentInit>(OnComponentInit);
    }

    private void OnComponentInit(EntityUid uid, ShuttleNameGeneratorComponent component, ComponentInit args)
    {
        if (!_prototypeManager.TryIndex<LocalizedDatasetPrototype>(component.NameDataset, out var NameDataset))
            return;
        
        if (!TryComp<MetaDataComponent>(uid, out var metaDataComponent))
            return;

        var prefix = _random.Pick(NameDataset.Values);
        _meta.SetEntityName(
            uid,
            (component.EnableFragments ? component.FactionIdentificator + prefix + " - " : component.FactionIdentificator + (component.HasIdentificator ? "" : " ")) +
            metaDataComponent.EntityName.Trim() +
            " " +
            Math.Ceiling(_random.NextFloat(1f, 10f) * 100),
            metaDataComponent,
            false);
    }
}
