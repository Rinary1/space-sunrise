using Content.Shared._Sunrise.TicketMachine;
using Content.Shared.Interaction;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Audio;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Prototypes;
using Robust.Shared.Map;
using Robust.Shared.Utility;
using Robust.Server.GameObjects;
using Robust.Shared.Log;
using Robust.Shared.Localization;
using System.Linq;
using System.Numerics;

namespace Content.Server._Sunrise.TicketMachine
{
    public sealed partial class TicketMachineSystem : EntitySystem
    {
        [Dependency] private readonly IEntityManager _entityManager = default!;
        [Dependency] private readonly IRobustRandom _random = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<TicketMachineComponent, InteractHandEvent>(OnInteractHand);
        }

        private void OnInteractHand(EntityUid uid, TicketMachineComponent component, InteractHandEvent args)
        {
            if (!component.Ready)
            {
                // Inform the user that the machine is not ready
                return;
            }

            if (!component.DispenseEnabled)
            {
                // Inform the user that the machine is disabled
                return;
            }

            if (component.TicketNumber >= component.MaxNumber)
            {
                // Inform the user that the ticket supply is depleted
                return;
            }

            if (component.HasTicket(args.User) && !component.Emagged)
            {
                // Inform the user that they already have a ticket
                return;
            }

            // SoundSystem.Play(Filter.Pvs(uid), "/Audio/Machines/terminal_insert_disk.ogg", uid);
            component.TicketNumber++;
            component.AddTicketHolder(args.User);

            var ticket = _entityManager.SpawnEntity("TicketMachineTicket", _entityManager.GetComponent<TransformComponent>(uid).Coordinates);
            component.AddTicket(ticket);

            // Assign ticket number and other properties here
        }

        private void BurnTickets(TicketMachineComponent machine)
        {
            foreach (var ticketUid in machine.Tickets)
            {
                _entityManager.QueueDeleteEntity(ticketUid);
            }
            machine.ClearTickets();
        }

        private void UpdateIconState(EntityUid uid, TicketMachineComponent component)
        {
            if (_entityManager.TryGetComponent(uid, out SpriteComponent? sprite))
            {
                TicketMachineComponent.TicketMachineVisualState state = component.TicketNumber switch
                {
                    <= 49 => TicketMachineComponent.TicketMachineVisualState.ticketmachine_100,
                    <= 99 => TicketMachineComponent.TicketMachineVisualState.ticketmachine_50,
                    _ => TicketMachineComponent.TicketMachineVisualState.ticketmachine_0
                };
                sprite.LayerSetState(0, state.ToString());
            }
        }

        private void HandleMapText(EntityUid uid, TicketMachineComponent component)
        {
            if (_entityManager.TryGetComponent(uid, out MapTextComponent? mapTextComponent))
            {
                if (!component.DispenseEnabled)
                {
                    mapText.OffsetX = 6;
                    mapText.MapText = "<font face='Small Fonts' color='#960b0b'>OFF</font>";
                    return;
                }

                mapText.OffsetX = component.TicketNumber switch
                {
                    <= 9 => 13,
                    <= 99 => 10,
                    _ => 8,
                };
                mapText.MapText = $"<font face='Small Fonts'>{component.TicketNumber}</font>";
            }
        }
    }
}