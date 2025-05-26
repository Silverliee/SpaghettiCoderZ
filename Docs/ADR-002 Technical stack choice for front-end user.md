# ADR-002: Technical stack choice for front-end user interface

## **Status :**

Accepted

## **Context :**

We need a web interface for users interaction. So the team proposes several possibilities for the technical stack, namely:

- React
- Next.js
- Vue.js
- Angular

### **React :**

**Pros :**

- Good documentation
- Industry standard
- Easy recruitment
- Easy to implement
- Large community
- Performance

**Cons :**

- Learning curve
- Often requires dependencies

### **Next.js :**

**Pros :**

- Performance
- Simplified SEO
- Quick configuration and startup

**Cons :**

- Complexity
- Learning curve
- Lack of relevance with project scale
- Framework-imposed limitations

### **Vue.js :**

**Pros :**

- Clear documentation
- Gentle learning curve
- Intuitive syntax (close to HTML/CSS)
- Flexibility
- Performance
- Consistent official ecosystem (Vuex, Vue Router)
- Reduced bundle size

**Cons :**

- Smaller community than React/Angular
- Fewer job opportunities
- Limited third-party library ecosystem
- Fewer resources and tutorials
- Lower enterprise adoption
- Rapid evolution (Vue 2 → Vue 3)

### **Angular :**

**Pros :**

- Complete "out of the box" framework
- TypeScript by default
- Structured and scalable architecture
- Advanced development tools (CLI, DevTools)
- Integrated and simplified testing

**Cons :**

- Learning curve
- High complexity for small projects
- Code verbosity
- Heavier startup performance
- Breaking changes between major versions
- Less flexible ecosystem
- Difficult recruitment (specialized skills)

## **Decision :**

We have decided to opt for React.js for the following reasons:

- Team familiarity with the framework (critical given time constraints)
- Alignment with industry standards to easily recruit or replace when needed
- Large community and good documentation in case of implementation difficulties
- Performance constraints (best market performance)

## **Consequences :**

- **Development Speed:** Faster initial development due to team expertise and reduced learning overhead
- **Maintenance:** Long-term maintainability ensured by widespread adoption and continuous community support
- **Scalability:** Component-based architecture allows for easy scaling and code reusability across the application
- **Hiring:** Simplified recruitment process due to large talent pool familiar with React
- **Risk Mitigation:** Lower technical risk due to mature ecosystem and proven track record in similar projects
- **Dependency Management:** Need to carefully select and maintain additional libraries for routing, state management, and UI components
- **Bundle Size:** Requires attention to code splitting and optimization to maintain performance standards