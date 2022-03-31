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

Some examples of composed adapters
- Events are a major application. Allow response to service actions without service knowing about subscribers, or subscribers knowing about each other 
- Migration: composite to read from original store while writing to both until new store is ready
- Caching: Can wrap a port with a caching decorator without changing service or the implementing adapter
- Retry / failure policy: again added as a decorator
- Transport/protocol: requests to a port/service can be moved out of process without service or adapter changing, only a new decorator (and maybe a new client) 
- Authorization

## Tasks
- [x] make readme explaining demonstrated ideas
- [x] Demonstrate a composition root
- [x] Demonstrate injecting request context without caller knowing the information
  - [x] restrict recipes by user
  - [x] add basic recipe create and list
  - [x] Add authentication decorator
- [ ] Decide if I want to deal with validating recipe actions that don't include a userId
- [ ] Demonstrate a service instantiated two ways in a single DI chain
- [ ] Demonstrate some component reuse (idea: migration, backup, scheduled tasks)
- [ ] Demonstrate pure composition root
  - [ ] arguably accomplished by tests
  - I don't have to use the root. I could just demonstrate the equivalent registration in the UI. Since I only use the manager,
  I can just do the pure composition root in the factory
- [ ] Separate out adapter tests?


note: now that UI is done, most of the other cases should be fairly simple. I don't actually need to implement much of anything, just some some composition cases.
I can rely on naming to communicate what could be done
- IDEA: I don't have to use

It'd be good to show more interaction with accessor / utilities, but I'm not really feeling the chat service anymore. It's too much work for now.
I think for now
- [ ] Close out this round with notifiers
  - [ ] Search index
  - [ ] Email notification
  - [ ] composite notifier
- [ ] Maybe write a tool (migration?) or some other client usecase
