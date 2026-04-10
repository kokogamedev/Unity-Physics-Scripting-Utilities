# Vehicle Physics Utilities

Enhance Unity's vehicle physics systems with utilities for calculating accurate wheel steering angles, suspension logic, and more.

---

## WheelHelper

The `WheelHelper` class is a utility that calculates **Ackerman steering angles**, which aid in simulating realistic vehicle steering configurations. Ideal for cars, bikes, and other wheeled physics systems.

### Features
- **Ackerman Steering Geometry**: Computes accurate steering angles for left and right wheels based on:
	- **Turn Radius**: The center of rotation for your turning vehicle.
	- **Wheel Base**: The distance between the front and rear axles of your vehicle.
	- **Track Width**: The distance between wheels on the same axle.
- Supports calculations in both **degrees** and **radians**.
- Handles edge cases, such as approximately zero steer angles.

### Example Code

#### 1. Calculating Steering Angles in Degrees
Use this when you want steering angles in degrees for UI or animator-compatible setups:

```c#
using PsigenVision.Utilities.PhysX.Vehicle;

public class VehicleSteeringDemo : MonoBehaviour
{
    private void Start()
    {
        float turnRadius = 10f;
        float wheelBase = 2.5f;
        float trackWidth = 1.5f;

        // Get the left and right Ackerman angles
        (float leftAngle, float rightAngle) = WheelHelper.GetAckermanAnglesFromRadius(turnRadius, wheelBase, trackWidth);

        Debug.Log($"Left Steering Angle: {leftAngle}°, Right Steering Angle: {rightAngle}°");
    }
}
```

#### 2. Calculating Steering Angles in Radians
For math-intensive workflows, radians provide precision and compatibility with low-level APIs:

```c#
using PsigenVision.Utilities.PhysX.Vehicle;

public class VehicleSteeringRadiansDemo : MonoBehaviour
{
    private void Start()
    {
        float turnRadius = 10f;
        float wheelBase = 2.5f;
        float trackWidth = 1.5f;

        // Get the left and right Ackerman angles in radians
        (float leftAngle, float rightAngle) = WheelHelper.GetAckermanAnglesRadiansFromRadius(turnRadius, wheelBase, trackWidth);

        Debug.Log($"Left Steering Angle: {leftAngle} radians, Right Steering Angle: {rightAngle} radians");
    }
}
```

---

### API Reference

#### Methods
Here’s a quick reference for the available methods in `WheelHelper`.

#### 1. **GetAckermanAnglesFromRadius**
Returns the Ackerman steering angles for left and right wheels in **degrees**.

```c#
public static void GetAckermanAnglesFromRadius(
    float turnRadius, 
    float wheelBase, 
    float trackWidth, 
    out (float left, float right) steerAngle
)
```

- **Parameters**:
	- `turnRadius`: The turning radius of the vehicle's center of rotation.
	- `wheelBase`: The distance between the front and rear axles of the vehicle.
	- `trackWidth`: The width of the vehicle's track.
	- `steerAngle`: *(out)* Tuple containing the left and right steering angles in degrees.

---

#### 2. **GetAckermanAnglesRadiansFromRadius**
Returns the Ackerman steering angles for left and right wheels in **radians**.

```c#
public static void GetAckermanAnglesRadiansFromRadius(
    float turnRadius, 
    float wheelBase, 
    float trackWidth, 
    out (float left, float right) steerAngle
)
```

- **Parameters**:
	- `turnRadius`: The turning radius of the vehicle's center of rotation.
	- `wheelBase`: The distance between the front and rear axles of the vehicle.
	- `trackWidth`: The width of the vehicle's track.
	- `steerAngle`: *(out)* Tuple containing the left and right steering angles in radians.

---

#### 3. **GetAckermanAnglesFromTurnAngle**
Calculates the steering angles given a **turn angle** (e.g., from joystick input) as a starting point.

```c#
public static void GetAckermanAnglesFromTurnAngle(
    float turnAngle, 
    float wheelBase, 
    float trackWidth, 
    out (float left, float right) steerAngle, 
    bool checkForZeroAngle = false
)
```

- **Parameters**:
	- `turnAngle`: Turning angle of the vehicle in degrees.
	- `checkForZeroAngle`: Optional flag to handle cases where the angle is approximately zero to avoid rounding errors.

#### Notes:
Check for edge cases like zero turning radius or narrow track widths to avoid physics glitches.

---

### Known Issues
- Does not account for dynamic weight or tire friction during sharp turns. This is purely a **geometrical utility**.

---

### My Final Take

This script fits **perfectly** under **Vehicle Physics** because:
- It’s exclusively targeting vehicle systems like steering and wheels.
- Its real-world application is narrow but valuable for those working on wheeled systems.
- A clean namespace like `PsigenVision.Utilities.Physics.Vehicle` makes it intuitive for end-users.

We now have the documentation ready to slide into the **VehiclePhysics.md** document when needed. Let me know if I should revise or move on to the next script! 🚀