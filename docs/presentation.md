# Interview Presentation Guide

Target length: 12 to 15 minutes.

## 1. Problem and user story (1 minute)

Explain that the product helps authenticated users focus on their own tasks. Read the user story from the README and call out the ownership boundary.

## 2. Architecture (3 minutes)

Open `docs/architecture.md` and explain the dependency direction:

- Domain contains rules without framework dependencies.
- Application coordinates use cases against interfaces.
- Infrastructure implements persistence and authentication.
- API translates HTTP and claims into use-case input.
- Angular consumes only API contracts.

Show one command handler and the repository owner predicate.

## 3. TDD and quality (2 minutes)

Open `TaskItemTests` and `CreateTaskHandlerTests`. Explain one red-green cycle and why `TimeProvider` makes the due-date test deterministic. Then show the API integration test as proof of the complete boundary.

## 4. Live demo (4 minutes)

1. Start with `docker compose up --build`.
2. Sign in using the seeded demo account.
3. Create a task with a future due date.
4. Filter the list and edit its status.
5. Delete the task.
6. Show a protected API request returning `401` without a token.
7. Briefly open the OpenAPI document.

## 5. GenAI critical thinking (2 minutes)

Open `docs/genai.md`. Show the prompt and explain two corrections: database-level owner filtering and replacing ambient time with an injected clock.

## 6. Tradeoffs and next steps (2 minutes)

- Add refresh-token rotation and HttpOnly cookies for production.
- Add pagination and server-side filtering for large task lists.
- Add optimistic concurrency with a row version.
- Run API and browser end-to-end tests in CI.
- Use managed secrets and a least-privileged SQL account.

Finish by running the test commands from the README.
