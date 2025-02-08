## **Flappy Bird Clone: Unity Project Breakdown**
Part of my Futurae teaching intervention. First student project: Unity recreation of Flappy Bird highlighting core programming concepts and system design.

### Core Scripts & Features

**Player.cs**

- Physics-based bird movement with `Rigidbody`/fake gravity
- Dynamic rotation via for directional responsiveness
- Collision detection for failure states
- Score-trigger logic using collider zones

**ScoreManager.cs**

- Persistent high scores using `PlayerPrefs`
- Top 10 leaderboard with player name input (UI `InputField`)
- Real-time score updates via Events

**GameManager.cs**

- State machine handling UI transitions (Home/Game/Game Over screens)
- Pause functionality with timescale control

**LevelGenerator.cs**

- Procedural pipe generation with adjustable randomization:
  - Height variance
  - Gap spacing
  - Horizontal offsets
- Object pooling for optimized scrolling effect

### Key Programming Takeaways

- **Component separation:** Decoupled UI, gameplay, and data systems
- **Event-driven design:** Used `Event` for UI/score synchronization
- **Physics tuning:** Balanced gravity/jump forces for responsive feel
- **Persistent data:** Local storage implementation for leaderboards

Built in Unity 6 & C# as a project-based learning exercise. Includes bonus leaderboard features and modular architecture for extendability.

[Released to itch.io](https://lacrearthur.itch.io/flappy-bird-futurae)
