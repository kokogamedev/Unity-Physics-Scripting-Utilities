# Changelog

All notable changes to this project will be documented in this file.

The format adheres to [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/).

---

## [0.1.0] - 2026-04-09

### Added
- Introduced the **Vehicle Physics Utilities**, including:
    - **`WheelHelper`**: Provides scaling and configuration tools for RaycastWheel-based systems.
- Introduced the **Collision Handling Utilities**, including:
    - **`IgnoreSpecificCollisions`**: Dynamically prevents collisions between two GameObject layers.
- Introduced the **Motion Constraints Utilities**, including:
    - **`CircularMotionConstraint`**: Constrains motion to a circular pivot or straight-line axis.

### Known Issues
- **`CircularMotionConstraint`**:
    - Currently in a broken state and under debugging.
    - Horizontal displacement (for straight-line motion) requires further fixes.
    - Unpredictable behavior occurs when parent objects are non-uniformly scaled.

### Documentation
- Created feature documentation files that break down usage and configuration for each utility.
- Added README summarizing features, future work, installation instructions, and known issues.

---

### Todo (Upcoming Work)
- **Future Motion Constraints Features**:
    - Introduce spline-based constraints and multi-axis pivot support.
    - Enhance the `CircularMotionConstraint`'s inspector UI for clearer setup workflows.
- **Vehicle Physics Enhancements**:
    - Add suspension visualizers and advanced traction control systems.
- **Collision Handling Enhancements**:
    - Develop runtime collision exemption tools and layer debugging utilities.
- **Testing**:
    - Create test scenes and examples to validate different features across all utilities.

---

> This marks the first **organized release** of the **PsigenVision Physics Utilities**, translated from combined usability scripts developed over time. Its current form is ready for use but will evolve through testing, refinement, and feature expansion in future updates.

---