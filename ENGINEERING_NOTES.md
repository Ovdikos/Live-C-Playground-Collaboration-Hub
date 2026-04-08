# Engineering Notes

*A living document tracking the architectural decisions, trade-offs, and random moments during the development of the Live C# Playground Collaboration Hub.*

---

##  Entry 1: The Great Frontend Migration - Why Angular over React?

**Context:**
The project initially started with a Blazor Server frontend. It was great for rapid prototyping and keeping everything in C#, but it has downsides for this specific app (high server memory usage per client, constant WebSocket chatter just for UI updates). It’s time to move to a modern SPA (Single Page Application) to offload rendering to the client and improve perceived performance. The ultimate showdown: **React vs. Angular**.

**React:**
* *Pros:* Massive ecosystem, literally an npm package for everything. JSX is pretty neat once you get the hang of it, and the job market absolutely loves it. 
* *Cons:* It's *just* a UI library. You have to duct-tape your own framework together. Need routing? Pick a library (React Router). Need HTTP calls? Axios or Fetch. State management? Redux, Zustand, Context API... the decision fatigue is real.

**Angular:**
* *Pros:* "Batteries included." Out of the box, you get a powerful CLI, robust Routing, an excellent HTTP client (with Interceptors!), reactive forms, and built-in state/reactivity with RxJS and the new Signals. It relies heavily on Dependency Injection, which feels right at home if you're a .NET developer. Plus, TypeScript is a first-class citizen—no configurations needed.
* *Cons:* A steeper learning curve (RxJS can melt your brain initially), and it historically had a lot of boilerplate (though modern Standalone Components fixed a lot of this).

**The Verdict :**
I chose **Angular**. Why?
Because the backend is built with clean architecture, CQRS (MediatR), and solid enterprise patterns in .NET 8. Angular mirrors that structured, opinionated, object-oriented vibe perfectly. I don't want to spend 3 hours picking npm packages just to build a login form🥲 I want a cohesive, robust framework that can handle complex features (like our real-time SignalR collaboration service) cleanly and predictably. 

### Entry 1.1: The JSON Contract Strictness

**Context:**
While prepearing the backend for the Angular migration, I realized a subtle but critical issue. Blazor Server didn't care much about HTTP response body formatting because it operated over a SignalR circuit natively. However, Angular's `HttpClient` is strictly typed and expects valid JSON by default.

**The Problem:**
Several of my API endpoints were returning raw strings for errors (`return BadRequest(ex.Message);`) or empty bodies for success (`return Ok();`). If Angular receives the string "User not found!", it attempts to run `JSON.parse("User not found!")` and throws a parsing exception, masking the actual 400 Bad Request error from my application logic.

**The Solution:**
I refactored all controllers to adhere to a strict JSON contract. 
* All errors are now wrapped in an anonymous object: `return BadRequest(new { message = ex.Message });`.
* All parameter-less success responses return a standard confirmation: `return Ok(new { message = "Success" });`.


### Entry 2: Setting the Foundation - The LIFT Architecture

**Context:**
Initializing a new frontend project is like laying the foundation of a house. If you mess it up, everything you build on top of it will eventually collapse into a spaghetti-code nightmare. Angular is highly opinionated, but it still leaves folder structure up to the developer. 

**The Decision:**
I decided to strictly follow the **LIFT** principle (Locate, Identify, Flat, Try to be DRY) combined with a Feature-Driven architecture.

Here is the blueprint for `src/app`:
* **`/core`**: The brain. Singleton services (`AuthService`, `SignalRService`), Interceptors (for JWT), and Route Guards. If it injects globally, it lives here.
* **`/shared`**: The toolkit. Reusable UI components (like the `CropperModal`), pipes, and directives. These are stupid components—they know nothing about business logic or HTTP calls.
* **`/features`**: The muscle. Organized by business domain (`auth`, `sessions`, `snippets`, `admin`). This keeps related code tightly grouped. If the `sessions` feature is deleted tomorrow, the rest of the app shouldn't even blink.

**Why it matters:**
In Blazor Server, everything was somehow grouped by Pages and shared components in a flat manner. As the SPA grows, lazy loading becomes critical for performance. This Feature-Driven structure ensures that when a user navigates to `/snippets`, Angular only loads the `snippets` feature bundle, keeping the initial bundle size incredibly small and fast.
