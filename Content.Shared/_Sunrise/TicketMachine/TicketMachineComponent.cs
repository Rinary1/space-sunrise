using System.Collections.Generic;
using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Shared._Sunrise.TicketMachine
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class TicketMachineComponent : Component
    {
        public enum TicketMachineVisualState
        {
            inactive,
            ticketmachine_0,
            ticketmachine_50,
            ticketmachine_100
        }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("ticketNumber")]
        public int TicketNumber { get; set; } = 0;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("currentNumber")]
        public int CurrentNumber { get; set; } = 0;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("maxNumber")]
        public int MaxNumber { get; set; } = 100;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("cooldown")]
        public float Cooldown { get; set; } = 50f;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("ready")]
        public bool Ready { get; set; } = true;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("dispenseEnabled")]
        public bool DispenseEnabled { get; set; } = true;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("emagged")]
        public bool Emagged { get; set; } = false;

        [ViewVariables]
        public List<EntityUid> TicketHolders { get; } = new();

        [ViewVariables]
        public List<EntityUid> Tickets { get; } = new();

        public bool HasTicket(EntityUid user) => TicketHolders.Contains(user);

        public void AddTicketHolder(EntityUid user) => TicketHolders.Add(user);

        public void AddTicket(EntityUid ticket) => Tickets.Add(ticket);

        public void ClearTickets()
        {
            TicketHolders.Clear();
            Tickets.Clear();
        }
    }
}