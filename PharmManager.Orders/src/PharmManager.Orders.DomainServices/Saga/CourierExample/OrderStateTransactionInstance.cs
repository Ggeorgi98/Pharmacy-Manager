using MassTransit;

namespace PharmManager.Orders.DomainServices.Saga.CourierExample
{
    public class OrderStateTransactionInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public State CurrentState { get; set; }

        public Guid OrderId { get; set; }
    }
}
