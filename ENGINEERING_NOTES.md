# Engineering Notes

*A living document tracking the architectural decisions, trade-offs, and random moments during the development of the Live C# Playground Collaboration Hub.*

---

##  Entry 1: The Great Frontend Migration - Why Angular over React?

**Context:**
The project initially started with a Blazor Server frontend. It was great for rapid prototyping and keeping everything in C#, but it has downsides for this specific app (high server memory usage per client, constant WebSocket chatter just for UI updates). It’s time to move to a modern SPA (Single Page Application) to offload rendering to the client and improve perceived performance. The ultimate showdown: **React vs. Angular**.

**React - The Cool Kid:**
* *Pros:* Massive ecosystem, literally an npm package for everything. JSX is pretty neat once you get the hang of it, and the job market absolutely loves it. 
* *Cons:* It's *just* a UI library. You have to duct-tape your own framework together. Need routing? Pick a library (React Router). Need HTTP calls? Axios or Fetch. State management? Redux, Zustand, Context API... the decision fatigue is real.

**Angular - The Enterprise Heavyweight:**
* *Pros:* "Batteries included." Out of the box, you get a powerful CLI, robust Routing, an excellent HTTP client (with Interceptors!), reactive forms, and built-in state/reactivity with RxJS and the new Signals. It relies heavily on Dependency Injection, which feels right at home if you're a .NET developer. Plus, TypeScript is a first-class citizen—no configurations needed.
* *Cons:* A steeper learning curve (RxJS can melt your brain initially), and it historically had a lot of boilerplate (though modern Standalone Components fixed a lot of this).

**The Verdict :**
I chose **Angular**. Why?
Because the backend is built with clean architecture, CQRS (MediatR), and solid enterprise patterns in .NET 8. Angular mirrors that structured, opinionated, object-oriented vibe perfectly. I don't want to spend 3 hours picking npm packages just to build a login form🥲 I want a cohesive, robust framework that can handle complex features (like our real-time SignalR collaboration service) cleanly and predictably. 


