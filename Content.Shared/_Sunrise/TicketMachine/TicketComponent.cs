using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._Sunrise.TicketMachine
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class TicketComponent : Component
    {
        [DataField("ticketNumber")]
        public int TicketNumber { get; set; }

        [DataField("owner")]
        public EntityUid Owner { get; set; }
    }
}