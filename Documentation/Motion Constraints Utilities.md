# Motion Constraints

The Motion Constraints feature provides tools for simulating constrained motion and mechanical relationships in Unity. These systems are ideal for scenarios requiring precise constrained movements, such as vehicle suspensions, robotic arms, or articulated objects.

---

## Scripts in This Feature

- **CircularMotionConstraint**:
	- Implements circular motion and optional straight-line transitions.
	- Used for systems where rotation and movement need to be physically constrained.

---

## CircularMotionConstraint

### **Purpose**
The `CircularMotionConstraint` component constrains a target GameObject to:
1. Rotate around a pivot point (circular motion).
2. Optionally, move along a straight-line axis.
   This behavior simulates realistic mechanical motion, such as lever arms or constrained suspension systems.

### **Serialized Fields**
1. **Main Target**
	- `toConstrain (Transform)`: The target object to apply constraints to.

2. **Circular Motion Settings**
	- `swingArmCenter (Transform)`: The pivot for circular motion.
	- `swingArmEndpoint (Transform)`: The endpoint defining the lever arm’s radius.
	- `rotationAxisDirection (Enum)`: Axis of rotation (Right, Up, or Forward).
	- `swingArmZeroRotationDirection (Enum)`: Baseline zero direction for the arm.

3. **Straight-Line Settings**
	- `convertToStraightLineMechanism (bool)`: Enables straight-line motion.
	- `horizontalShiftDirection (Enum)`: Configurable axis for movement.

---

### **Usage**
1. Add the script to a GameObject in your scene.
2. Assign the `swingArmCenter`, `swingArmEndpoint`, and other serialized fields in the Inspector.
3. (Optional) Enable straight-line transition by toggling `convertToStraightLineMechanism`.

At runtime:
- Call `ApplyEndpointVerticalTravelDistance(float verticalTravelDistance)` to apply vertical motion while respecting constraints.
- Use methods like `Initialize()` to set up calculations dynamically.

---

### **Potential Applications**
1. **Vehicle Suspensions**:
	- Constrain wheels or arms to stay within limited motion paths.
2. **Robotics**:
	- Simulate arm articulation and constrained rotations.
3. **Mechanical Systems**:
	- Any object requiring precise circular or linear motion.

### **Known Issues as of v0.1.0**
- Currently broken and under debugging.
- Horizontal displacement for straight-line motion transitions is **not functioning as intended**.
- Unpredictable behavior occurs when parent objects with non-uniform scaling are used.

## Future Work

Exciting things are planned for this package! Here’s a glimpse at what’s on the roadmap:

### Collision Handling Utilities
- **Dynamic Collision Exemptions**: Add runtime filters for toggling collisions dynamically between multiple groups or exceptions.
- **Collision Layer Debuggers**: A visual editor tool for working directly with Unity’s collision layers.