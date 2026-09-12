# AGENTS.md

Instructions for AI coding agents working in this repository.

## What this is

A conference demo for the talk "Dealing with eventual consistency". A movie-ticket
site where the browser talks to the website over SignalR, the website sends a command
to a back-end endpoint over messaging (NServiceBus), and the reply flows back to the
browser over SignalR, mimicking a synchronous experience over asynchronous processing.

It is demo code: correctness and clarity for a live audience matter more than
production hardening. Keep changes small and readable.

## Layout (`src/`)

- `Website/` — ASP.NET Core MVC site. Also hosts an NServiceBus endpoint. Sends
  `SubmitOrder`, converts incoming replies into SignalR messages. `TicketHub` is the
  SignalR hub; `site.js` is the browser client.
- `Server/` — console app hosting the back-end NServiceBus endpoint. Handlers live in
  `Handlers/`, pipeline behaviors in `Behaviors/`.
- `Shared/` — messages (`Commands/`, `Events/`, `Messages/`), entities, and endpoint
  configuration shared by both endpoints.
- `Tests/` — xUnit + NServiceBus.Testing handler tests.

## Build, test, run

```bash
dotnet build src                 # build the solution
dotnet test src/Tests            # run handler tests
```

Run both endpoints together in Rider with the **Demo** compound run configuration
(`Server` + `Website`), or from the CLI:

```bash
dotnet run --project src/Server
dotnet run --project src/Website    # https://localhost:7199, http://localhost:5199
```

Storage is SQLite (EF Core) in a `.database/` folder next to the solution; the message
queue is NServiceBus LearningTransport in `.learningtransport/`. Both are gitignored.

## Conventions and gotchas

- **.NET 10**, **NServiceBus 10.2**. Use a `global.json`-pinned SDK.
- **Handlers use the `[Handler]` attribute**, not `IHandleMessages<T>`. Convention-based
  handlers are NOT discovered by assembly scanning, so every handler must be registered
  explicitly with `endpointConfiguration.AddHandler<T>()`. Add that line when you add a
  handler.
- **Messages are immutable `sealed record`s** classified by namespace convention, not
  `ICommand`/`IEvent` markers: a type is a command/event/message when its namespace ends
  with `Commands`/`Events`/`Messages` (see `EndpointConfigurationExtensions`). Entities
  stay mutable classes since EF Core hydrates them.
- **Data is SQLite via EF Core (`MoviesDbContext`).** Register it with
  `services.AddMoviesDatabase()`. The schema is created from the entity model; movies/reviews
  are seeded in code by `SeedData`, wired through EF Core's `UseSeeding`/`UseAsyncSeeding`
  hooks and triggered when the website calls `EnsureCreatedAsync` on startup. Only the website
  creates/seeds, so the two endpoints don't race on `EnsureCreated`. Guids are stored as
  lower-case text so the seeded ids are deterministic. `Theater`s remain a static list for now.
- **Prefer constructor injection over static/ambient access.** Inject `ILogger<T>` (not
  the NServiceBus `LogManager`) and `TimeProvider` (not `DateTime.Now`). Handlers use
  primary constructors.
- **`IMessageSession` is keyed.** `AddNServiceBusEndpoint` registers `IMessageSession`
  as a keyed singleton (keyed by the endpoint name) to allow multiple endpoints per host.
  The website injects a plain `IMessageSession` into `TicketHub`, so `Website/Program.cs`
  aliases the keyed session as the default registration. Keep that alias if you touch the
  hosting setup.
- **The demo switch is stopping the `Server` endpoint.** The regular booking flow's
  confirmation is driven entirely by the real `OrderedRegularTicket` reply: with the
  back-end down the reply never comes, and after five seconds the panel degrades to
  "we can't confirm your seats yet" (`film-booking.js`). It never fakes success, and
  it never declares failure either — the command is durably queued, so the listener
  stays armed and the very same panel flips to "confirmed" when the endpoint is
  started mid-talk. Do not "fix" that timeout into an error path. The raffle
  (`film-raffle.js`) is the deliberate contrast: its confirmation is composed by the
  website itself (`MovieTickets.ReportOnLottery`) and promises nothing but an email
  after the draw, so it completes happily on a back-end that is down.
- Transport and persistence are `LearningTransport`/`LearningPersistence` — for demos
  only, never production.

## Repository rules

- Do not commit to `master`; work on a feature branch and open a PR.
- Reviews live in browser LocalStorage only, by design. Do not add server-side review
  storage.
