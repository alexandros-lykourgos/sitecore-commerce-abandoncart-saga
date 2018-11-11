# sitecore-commerce-abandoncart-saga
Handle Sitecore Commerce Abandon Cart using Saga (State machine approach)

There are two solutions:

Foundation.Commerce.AbandonCarts.Engine: This is a plug for Sitecore Commerce which will interact for different operations i.e Add cart/update cart 
and Order creation.
SagaApp: It has mainly two applications as well. 
AbandonCartStateMachine: is for the saga server which will register all ABS with State machine
AbandonCartApi: This is a web api that will be contacted via Commerce engine and then it will publish message to ABS
