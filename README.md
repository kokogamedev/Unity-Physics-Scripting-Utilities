# PsigenVision's Unity Physics Utilities

**Package:** `com.psigenvision.utilities.physics`  
**Version:** 0.1.0  
**Namespace:** `PsigenVision.Utilities.PhysX`

Welcome to **PsigenVision's Unity Physics Utilities**, a growing collection of tools designed to simplify physical simulation setups in Unity. From handling vehicle physics to motion constraints and collision filtering, these utilities can assist in creating rich and reliable physics-based interactions.

---

**Disclaimer:**
> This library reflects my ongoing journey as a developer and learner. The way these utilities are designed has grown over time, so there’s always room for improvement! I welcome feedback, suggestions, and collaboration from the community. It’s how I learn and evolve! 

**Documentation Directory:**
> Rather than going into excessive detail here, I've created dedicated feature-level documentation to describe each toolset included in the package. I encourage you to explore the _Documentation_ folder for specifics! 🚀

---

## Features

Here’s what the **Physics Utilities** package offers:

### 1. **Vehicle Physics Utilities**
Namespace: `PsigenVision.Utilities.Physics.Vehicle`

This category provides scripts designed to streamline vehicle physics in Unity. Whether you're simulating cars, bikes, or other physics objects, these tools help manage pivotal features.

#### Highlight:
- **`WheelHelper`**:
    - Simplifies scaling and raycasting configurations for `RaycastWheel`-based systems.
    - Ensures physics-based vehicles behave predictably and scale correctly.

---

### 2. **Collision Handling Utilities**
Namespace: `PsigenVision.Utilities.Physics.Collisions`

This toolset provides better control over Unity's collision behaviors, making complex scenes with many interacting objects easier to manage.

#### Highlight:
- **`IgnoreSpecificCollisions`**:
    - Dynamically ignore collisions between two specified GameObject layers.
    - Useful for scenarios where logical collision filtering is required at runtime.

---

### 3. **Motion Constraints Utilities**
Namespace: `PsigenVision.Utilities.Physics.Constraints`

Simulate constrained motion mechanics with this toolset, ideal for limb articulation, suspension, or physical pivots requiring limited-motion paths.

#### Highlight:
- **`CircularMotionConstraint`**:
    - Constrain objects to a circular motion around a pivot or allow straight-line movement along configurable axes.
    - Supports dynamic mechanical systems such as robotic arms or vehicle components.

---

## **Known Issues**

The current state of this package includes evolving features as well as components that are still under debugging. Here’s what you should know:

- **`CircularMotionConstraint`:**
  - Currently broken and under debugging.
  - Horizontal displacement for straight-line motion transitions is **not functioning as intended**.
  - Unpredictable behavior occurs when parent objects with non-uniform scaling are used.

---

## Installation


To use the utilities in your Unity project:

1. Install ["com.psigenvision.utilities.native"](https://github.com/kokogamedev/Unity-Native-Scripting-Utilities.git) as a dependency via Git URL or a local Unity folder.
2. Install ["com.psigenvision.utilities.physics"](https://github.com/kokogamedev/Low-Allocation-Tagged-Unions-for-Unity.git) via Git URL or a local Unity folder.
3. Start using the utilities by including the required namespaces in your scripts. For example:
   ```c#
   using PsigenVision.Utilities.PhysX.Vehicle;
   ```

---

## Contributing

I’m always open to ideas and improvements for the library! If you’d like to share your expertise or collaborate, here’s how:
1. Fork the repository.
2. Create a new branch.
3. Submit a pull request describing your changes and their purpose.

---

## Known Issues

Some features of the Physics Utilities package are new or experimental, so your patience is appreciated as I refine them through testing:

### Vehicle Physics Utilities
- The `WheelHelper` script hasn’t been tested extensively at very large scales or nested hierarchies.

### Collision Handling Utilities
- No issues reported so far! 🎉

### Motion Constraints Utilities
- Horizontal displacement for linear motion via `CircularMotionConstraint` requires debugging.
- Behavior under non-uniform scale parents requires investigation.

Your feedback and bug reports would be invaluable—please don’t hesitate to reach out!

---

## Future Work

Exciting things are planned for this package! Here’s a glimpse at what’s on the roadmap:

### Vehicle Physics Utilities
- **Suspension Visualizers**: Better visual debugging for vehicle suspension configurations.
- **Traction Control Logic**: Introduce slip detection and surface-specific adjustments for wheel physics.

### Collision Handling Utilities
- **Dynamic Collision Exemptions**: Add runtime filters for toggling collisions dynamically between multiple groups or exceptions.
- **Collision Layer Debuggers**: A visual editor tool for working directly with Unity’s collision layers.

### Motion Constraints Utilities
- **Spline-Based Motion Constraints**: Add support for spline-guided constrained motion paths.
- **Better Inspector UX**: Develop intuitive inspector controls for setting up pivots, directions, and transitions.

I’m continually learning, so these improvements will come as I refine my skills. Suggestions and help are very welcome!

---

## Issues or Feedback?

I’d love to hear from you! If you have ideas, encounter a bug, or just want to chat about the project, feel free to open an issue or get in touch. 

---

## Final Notes

Thank you for exploring **PsigenVision's Unity Physics Utilities**! This package represents both a tool for Unity developers and a personal journey in learning and growth. I hope it will simplify your workflow and make development in Unity more enjoyable! 🚀

---