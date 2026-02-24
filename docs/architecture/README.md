# Architecture Diagrams

This folder contains the recommended UML/C4 diagrams for the current DeliveryApp backend.

- `deliveryapp-c4-context.puml`: system context view (actors + external system).
- `deliveryapp-c4-container.puml`: container view (API, service, data layers, DB).
- `deliveryapp-class-diagram.puml`: class-level model aligned with the current codebase.
- `deliveryapp-modular-monolith-uml.puml`: target UML for the new modular monolith structure.
- `deliveryapp-backend-complete-class-methods.puml`: complete backend class diagram with refactored specialized services/repositories and method signatures.
- `deliveryapp-sequence-create-order.puml`: sequence diagram for order creation.
- `deliveryapp-sequence-assign-pay-review.puml`: sequence diagram for assign courier, payment and review flow.
- `deliveryapp-order-state-machine.puml`: order lifecycle state machine.
- `deliveryapp-data-model-erd.puml`: logical data model (ERD) of current SQL schema.
- `architecture-document.fr.md`: architecture summary in French (frontend, backend layers, SQL Server, Cosmos target design, interactions).
- `current-state.md`: current architecture state and transition note.
- `modular-monolith-migration.md`: phased migration plan.

Primary reference going forward: `deliveryapp-modular-monolith-uml.puml`.

Render with PlantUML:

```bash
plantuml docs/architecture/*.puml
```
