# Task Manager Domain Context

## Purpose

The application allows an authenticated user to manage only their own tasks.

## Core language

- **User**: an authenticated account managed by ASP.NET Core Identity.
- **Task item**: a piece of work owned by one user.
- **Task status**: the lifecycle state of a task item.
- **Due date**: the deadline associated with a task item.

## Current business rules

- A task item must belong to an identified user.
- A title is required and cannot exceed 200 characters.
- A description cannot exceed 2,000 characters.
- A new task item starts in the `Pending` status.
- A completed task item is immutable: its details and lifecycle status cannot be changed.
- Temporal due-date validation belongs to the application use case so it can
  compare against an injected clock without introducing ambient time into the domain entity.

## Confirmed test seams

Tests observe behavior only through these public boundaries:

1. Domain entities and value objects through their public creation and behavior methods.
2. Application use cases through their public handler interfaces.
3. Persistence through repository contracts against SQL Server.
4. HTTP behavior through the public API endpoints.
5. Frontend HTTP services and routed user flows through their public Angular APIs.

The first vertical slice covers successful creation of a task item through the
public `TaskItem.Create` method.
