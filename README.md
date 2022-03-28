## Motivations

This repository is meant to demonstrate several ideas around Dependency Inversion.
This includes demonstrating constructor injection as well as benefits of Dependency Inversion over Dependency Injection


## Constructor Injection Concepts

- Constructor injection does not couple services to 2nd+ order dependencies
  - To this end, demonstrate composition roots
- Constructor injection (over service locator) allows components to have no knowledge of DI libraries / containers
  - Consequence: reduced coupling across system. components agnostic to injection container of caller.
- Constructor injection allows intuitive reuse of components within a system or outside of it
  - in particular, code does not need to be examined to determine dependencies of a component in any context. Required dependencies or alternative instantiations are clearly defined (again, without exposing a service to 2nd+ order dependencies)
- Constructor injection allows alternative configurations of the same service within the same dependency hierarchy, or even on a single service. 
  - This cannot be achieved with service locator without the consuming service supplying additional differentiation between required configuration

Some benefits of constructor injection over service locator that are not demonstrated
- Greatly reduced odds of accidental circular dependencies compared to service locator
- Option for pure composition approach (hand-written composition root), which allows the compiler to catch any composition issues right away (e.g. missed registrations, changes in dependencies, captive dependencies)


## Dependency Inversion > Dependency Injection
This repository also aims to demonstrate how Dependency Injection != Dependency Inversion, and benefits that can be gained from Dependency Inversion.

To clarify the difference
- Dependency Injection -> dependency instances constructed externally and provided to a component
- Dependency Inversion -> *Abstractions belong to the callers.* Implementations are created in separate and unreferenced assemblies then generally composed together by a third-party consumer

Benefits of Dependency Inversion (over injection)
- Breaks semantic tension between callers and consumers
- Increases information hiding and independence of services, containing change propagation through the system
- More likely to result in generally reusable services
- Easier to compose new behavior into a service rather than modifying the service
  - Easier to use a service multiple ways without modifying the service
- Makes services the lowest member of assembly dependency chains. Improves ability of services to adapt to contexts of new callers (langauge versions, framework versions, etc)
  - easier to isolate difficult migrations to adapters only referenced in the top-level client, maximizing portability of the most code
- Leads to better application of the Open-Closed principle



