# ADR-001: User and Parking Spot Entity Modeling

## Status

Accepted

## Context

As part of developing the parking reservation system, we need to define the data structure for the two main entities of the system: users and parking spots.

The system must manage 60 parking spots organized in 6 rows (A to F) of 10 spots each, with specific constraints for electric/hybrid vehicles. Users have different roles with distinct permissions and reservation capabilities.

We need to decide on the modeling of these entities to ensure:

- Ease of use for non-technical users
- Management of different roles and permissions
- Management of physical parking constraints
- Reservation traceability

## Decision

We adopt the following models:

**User**

- Last Name (string, required)
- First Name (string, required)
- Role (enum: Employee, Secretary, Manager, required)
- Contact (string, required - professional email)

**Parking Spot**

- Identifier (string, required, unique)
- Electric Charger (boolean, required)

This minimalist modeling focuses on the essential attributes for the reservation system operation.

## Consequences

**Advantages:**

- Simple and understandable model for all users
- Facilitates validation of business constraints (electric vehicles on rows A and F)
- Clearly defined roles enabling straightforward permission management
- Spot identifier directly mapped to physical parking organization
- Single contact avoids multi-channel management complexity

**Disadvantages:**

- Model may require future extensions (e.g., phone number, vehicle information)
- No intrinsic email format validation in the model
- Absence of user's vehicle information (size, fuel type)

**Technical implications:**

- Application-side validation necessary for spot identifier and email format
- Enum for roles facilitates permission management in code
- Boolean for electric charger allows simple queries to filter compatible spots