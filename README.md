# Pharmacy-Manager
A microservices application that demonstrates how to use MassTransit with RabbitMQ for the communication between the microservices in order to implement SAGA pattern

## Prerequisites
In order to test the scheduled sending of the **SendOrderNotification** message that is sent by **ScheduleOrderNotificationConsumer** with RabbitMQ, you would need to use RabbitMQ that has *RabbitMQ delayed exchange* plug-in installed in itself. If you are using Docker containers, you can run the following command for starting a RabbitMQ container that has the necessary plug-in:

> docker run -d -p 15672:15672 -p 5672:5672 --name rabbitMQ-with-delay-exchanged masstransit/rabbitmq

## Different scenarios of MassTransit usage in the project
1. Using Automatonymous
  
    In order to test this scenario, you need to send a HTTP Post request to the **submit-order/{id}** endpoint. This will publish an event - **OrderSubmittedEvent** that       will be handled by the state machine - **OrderStateMachine**. The state machine will go through four different states.

2. Using Courier

    The courier is used to implement the routing slip pattern and to achieve execution of distributed transactions with fault compensation that can be used to meet the ACID requirements for the transactions within domain of database transactions. 
    1. This scenario is implemented in two different ways. The first one publishes an event - **OrderTransactionSubmittedEvent** - that is handled by a state machine and the execution is continued by an activity (OrderTransactionSubmittedActivity) in which a message is sent to the **SubmitOrderMessageConsumer**. The routing slip pattern is implemented in this consumer. Every action is separated in a new activity in which there is a compensate mechanism in case of erros with the execution of the whole transaction. In order to test this scenario, you need to send a HTTP Post request to the **submit-order-transaction-in-state-machine/{id}** endpoint.
    2. The second scenario can be tested by sending a HTTP Post request to the **submit-order-transaction/{id}** endpoint. The difference is that the process doesn't go through a state machine and this we lose the advantages and features that the state machine brings to us. In this use case, a **SubmitOrderMessage** is directly sent to the relevant consumer in which the routing slip pattern is implemented.
    
