# ADR-003: Technical stack choice for reservation API

## **Status :**

Accepted

## **Context :**

We need a central system capable of managing business cases related to reservations. The team has proposed to go with an API as it is more relevant for business implementation as well as for communication with our user interface. In terms of technical stack, the team has proposed:

- Spring API
- .NET API
- Express.js API

### **Spring API :**

**Pros :**

- Mature and robust framework
- Excellent enterprise support
- Strong dependency injection and IoC
- Comprehensive security features
- Large Java developer community
- Proven scalability for complex applications
- Rich ecosystem and third-party integrations

**Cons :**

- Steep learning curve
- Heavy configuration overhead
- Slower startup times
- Higher memory consumption
- Verbose code structure
- Complex for simple projects

### **.NET API :**

**Pros :**

- Strong Microsoft ecosystem integration
- Excellent performance and optimization
- Built-in security and authentication
- Comprehensive tooling and IDE support
- Cross-platform compatibility (.NET Core)
- Strong typing and compile-time error checking
- Enterprise-grade features out of the box

**Cons :**

- Microsoft ecosystem dependency
- Licensing costs for some components
- Smaller open-source community compared to Java
- Learning curve for non-Microsoft developers
- Potential vendor lock-in
- Higher hosting costs on non-Windows platforms

### **Express.js API :**

**Pros :**

- Lightweight and fast development
- Large Node.js community
- Easy learning curve
- Flexible and unopinionated
- Excellent for rapid prototyping
- JSON-native handling
- Cost-effective hosting options

**Cons :**

- Limited built-in features
- Callback hell and async complexity
- Less suitable for CPU-intensive tasks
- Weaker typing (unless using TypeScript)
- Security requires additional configuration
- Performance limitations for high-load applications
- Rapid ecosystem changes

## **Decision :**

We have decided to opt for .NET API for the following reasons:

- Team expertise with the framework (critical given project requirements and timeline constraints)
- Reduced development risk due to existing knowledge and experience
- Faster implementation timeline leveraging current team skills
- Lower training and onboarding costs for new team members

## **Consequences :**

- **Development Velocity:** Accelerated development timeline due to team's existing expertise and familiarity with .NET ecosystem
- **Code Quality:** Higher code quality and fewer bugs expected due to team's experience with the framework and strong typing system
- **Maintenance:** Simplified long-term maintenance with team's deep understanding of .NET architecture and troubleshooting
- **Performance:** Optimal application performance leveraging team's knowledge of .NET optimization techniques and best practices
- **Integration:** Seamless integration with existing Microsoft-based infrastructure and tools already in use
- **Security:** Robust security implementation using team's experience with .NET security features and enterprise-grade authentication
- **Scalability:** Effective scaling strategies based on team's previous experience with .NET applications under load
- **Technical Debt:** Minimal technical debt accumulation due to adherence to established .NET patterns and practices known by the team
- **Knowledge Transfer:** Reduced dependency on external consultants or extensive documentation for knowledge sharing within the team
- **Risk Mitigation:** Lower project risk due to predictable development patterns and team's ability to anticipate and resolve .NET-specific challenges