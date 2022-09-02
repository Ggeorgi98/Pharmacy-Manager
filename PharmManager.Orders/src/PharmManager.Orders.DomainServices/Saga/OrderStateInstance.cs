using MassTransit;

namespace PharmManager.Orders.DomainServices.Saga
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public State CurrentState { get; set; }

        public Guid OrderId { get; set; }
    }
}
