using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace PsigenVision.Utilities.PhysX.Constraints
{
    public class CircularMotionConstraint : MonoBehaviour
    {
        #region Serialized Fields

        /// <summary>
        /// Represents the object that will be constrained to follow a circular motion.
        /// This transform is manipulated to adhere to the rotational path defined by the
        /// configuration of the circular motion constraint.
        /// </summary>
        [Header("Constraint Target Settings")] 
        [SerializeField] private Transform toConstrain;

        #region Serialized Fields - Positions and Reference Frames

        /// <summary>
        /// Represents the pivot point around which the circular motion is constrained.
        /// This transform serves as the center of rotation for the swing arm,
        /// defining the origin of the circular motion path described by the constrained object.
        /// </summary>
        [Header("Swing Arm Configuration")] 
        [SerializeField] private Transform swingArmCenter;

        /// <summary>
        /// Represents the endpoint of the swing arm used in the circular motion constraint.
        /// This transform defines the outer point of the lever arm for rotation and plays
        /// a role in determining the radius of the circular motion path.
        /// </summary>
        [SerializeField] private Transform swingArmEndpoint;

        /// <summary>
        /// Represents the reference Transform used to define the orientation or coordinate system
        /// for the axis of rotation. This frame of reference determines the basis for calculating
        /// angular constraints and transformations applied to the constrained object.
        /// </summary>
        [SerializeField] private Transform rotationAxisReferenceFrame;

        /// <summary>
        /// Represents the reference frame used to define the axis along which
        /// a translation will occur during motion computation. It provides
        /// a local coordinate system that enables calculations relative to
        /// its orientation and position.
        /// </summary>
        [SerializeField] private Transform translationAxisReferenceFrame;

        #endregion

        #region Serialized Fields - Circular Motion Constraint

        /// <summary>
        /// Specifies the direction of the axis around which the object rotates in a circular pattern.
        /// It can represent one of the primary directions: right, up, or forward, and determines the
        /// rotation plane in conjunction with other constraints.
        /// </summary>
        [Header("Circular Motion Constraint")] 
        [SerializeField] private Direction rotationAxisDirection = Direction.Right;

        /// <summary>
        /// Represents the direction of the reference axis for defining the initial zero rotation
        /// of the swing arm relative to the rotational axis. It is used to calculate
        /// the initial angular displacement of the swing arm in the circular motion
        /// system.
        /// </summary>
        [SerializeField] private Direction swingArmZeroRotationDirection = Direction.Forward;

        #endregion

        #region Serialized Fields - Straight Line Movement Constraint

        /// <summary>
        /// Determines whether the circular motion constraint should be replaced with
        /// a straight-line movement mechanism. When set to true, the constrained object's
        /// motion will follow a linear/vertical trajectory rather than a circular one.
        /// </summary>
        [Header("Straight Line Movement Constraint")] [SerializeField]
        private bool convertToStraightLineMechanism = false;

        /// <summary>
        /// Specifies the primary direction of horizontal shift applied to the object
        /// when transitioning from circular motion to a straight-line movement mechanism.
        /// Possible values include right, forward, and up, which determine the axis
        /// along which the shift occurs.
        /// </summary>
        [SerializeField] private Direction horizontalShiftDirection = Direction.Forward;

        /// <summary>
        /// Indicates whether the parent object of the current component has non-unitary scaling applied.
        /// This property affects calculations for compensation of scaled transformations, ensuring
        /// accurate positioning and movement within the constraint system.
        /// </summary>
        [SerializeField] private bool hasScaledParent = false;

        /// <summary>
        /// Represents the scaling factor of the parent object, used to adjust calculations
        /// within the constraint system for cases where the parent object has non-unitary scaling.
        /// This ensures consistent behavior of the constrained motion regardless of the scale
        /// transformations applied to the parent object.
        /// </summary>
        [SerializeField] private float parentScale = 1f;

        #endregion

        #region Serialized Fields - Initialization

        /// <summary>
        /// Indicates whether the circular motion constraints or initial calculations
        /// should be executed automatically during the Awake lifecycle event of the component.
        /// If set to true, the Initialize method is called as soon as the component becomes active.
        /// </summary>
        [Header("Initialization")] [SerializeField] private bool initializeOnAwake = false;

        #endregion
        #endregion

        #region Private Fields - Cached Data/References

        //All Constraints Cached Data

        /// <summary>
        /// Represents the length of the arm or radius constraining the object’s circular motion.
        /// This value determines the distance between the center of rotation and the object,
        /// influencing the magnitude of movement and angular calculations within the circular constraint system.
        /// </summary>
        [SerializeField] private float swingArmLength = 1f;

        /// <summary>
        /// Specifies the initial angular rotation of the swing arm relative to its defined reference axis.
        /// The value is represented in degrees and determines the starting orientation of the swing arm in the
        /// circular motion constraint system.
        /// </summary>
        [SerializeField] private float initialSwingArmRotation = 0f;

        //Circular Motion Constraint Cached Data

        /// <summary>
        /// Specifies the axis of rotation for the object during constrained circular motion.
        /// This vector determines the direction around which the angular displacement occurs.
        /// </summary>
        [SerializeField] private Vector3 rotationAxis = Vector3.right;

        /// <summary>
        /// Stores the initial rotation angle of the swing arm, measured in radians.
        /// This value is calculated based on the swing arm's zero rotation direction
        /// and the configuration of the circular motion constraint.
        /// </summary>
        [SerializeField] private float initialSwingArmRotationInRadians = 0f;

        //Straight Line Mechanism Constraint Cached Data

        /// <summary>
        /// Represents the initial position of the constrained object in world space.
        /// This position is calculated and stored when the initial conditions
        /// for circular motion are derived.
        /// </summary>
        [SerializeField] private Vector3 initialPosition;

        /// <summary>
        /// Stores the initial horizontal displacement position of the constrained object
        /// relative to a reference frame. This value is calculated when establishing
        /// the initial conditions for the straight line motion constraint and helps determine
        /// the object's position relative to the translation axis.
        /// </summary>
        [SerializeField] private Vector3 initialHorizontalDisplacementPosition;

        /// <summary>
        /// Represents the initial horizontal shift in the circular motion constraint, calculated based
        /// on the swing arm length and its initial rotation in radians. This value is derived to
        /// determine the starting horizontal displacement in relation to the motion's configuration.
        /// </summary>
        [SerializeField] private float initialHorizontalShiftUnscaled = 0f;

        /// <summary>
        /// A delegate function that calculates and returns a directional shift vector
        /// based on a provided scale. The direction of the vector is determined by a specified
        /// horizontal shift direction (e.g., forward, right, or up).
        /// Used in constraining straight line movement when deriving positional adjustments
        /// relative to the initial local position of the constrained object.
        /// </summary>
        private Func<float, Vector3> GetShiftVector;

        //Initialization
        private bool isInitialized = false;
        [SerializeField] private bool initialCircularConditionsAlreadyCalculated;
        [SerializeField] private bool initialHorizontalConditionsAlreadyCalculated;

        //Debug
        // [ShowInInspector] private float suspensionTravelDistance;
        // [ShowInInspector] private Vector3 _horizontalShift;
        // [ShowInInspector] private Vector3 _horizontalShiftPos;
        // [ShowInInspector] private float _horizontalShiftDistance;
        // [ShowInInspector] private float _horizontalShiftDistanceActual;
        // [ShowInInspector] private Quaternion _angularQuaternion;
        // [ShowInInspector] private float _angularRotation;

        #endregion

        #region Unity Event Methods

        void Awake()
        {
            if (initializeOnAwake) Initialize();
        }

        #endregion

        #region Methods - Initialization

        /// <summary>
        /// Initializes the circular motion or straight-line movement constraints for the object.
        /// This method calculates and sets up values required for applying either circular motion
        /// constraints or straight-line positional adjustments depending on the configured settings.
        /// Ensures that all necessary dependencies and references are validated and cached for runtime use.
        /// </summary>
        public bool Initialize()
        {
            if (toConstrain == null)
            {
                Debug.LogError("Circular Motion Constrainer has not been assigned a target to constrain");
                return isInitialized = false;
            }

            if (!initialCircularConditionsAlreadyCalculated) DeriveCircularMotionInitialConditions();
            if (!convertToStraightLineMechanism) return isInitialized = true;
            if (translationAxisReferenceFrame == null)
            {
                Debug.LogWarning(
                    "Horizontal shift reference transform int he straight line mechanism circular motion constrainer is null");
                return isInitialized = false;
            }

            if (!initialHorizontalConditionsAlreadyCalculated) DeriveHorizontalShiftingInitialConditions();
            return isInitialized = true;
        }

        /// <summary>
        /// Derives all necessary initial conditions required for applying motion constraints
        /// to the object. This includes determining parameters for both circular motion and,
        /// if enabled, straight-line horizontal movement constraints. The method ensures that
        /// the circular motion parameters are initialized first. If the straight-line movement
        /// mechanism is configured, the corresponding parameters for horizontal shifting are
        /// subsequently derived as well. This method consolidates all pre-runtime calculations
        /// needed for consistent and stable constraint application throughout runtime.
        /// </summary>
        [Button]
        public void DeriveAllInitialConditions(bool bypassInitializationGuards = false)
        {
            DeriveCircularMotionInitialConditions();
            if (!convertToStraightLineMechanism) return;
            DeriveHorizontalShiftingInitialConditions(bypassInitializationGuards);
        }

        /// <summary>
        /// Determines and initializes the required parameters for enforcing circular motion constraints
        /// on the specified object. This method calculates the initial state of the motion system,
        /// including swing arm length, rotation axis, lever arm orientation, and initial rotational offsets.
        /// Additionally, it handles caching of the object's initial position to ensure consistency during runtime.
        /// Once complete, flags circular motion conditions as initialized,
        /// ensuring they are only derived once during runtime.
        /// </summary>
        private void DeriveCircularMotionInitialConditions()
        {
	        swingArmLength = Vector3.Distance(swingArmCenter.position, swingArmEndpoint.position);
            rotationAxis = rotationAxisDirection switch
            {
                Direction.Right => Vector3.right,
                Direction.Up => Vector3.up,
                Direction.Forward => Vector3.forward,
                Direction.Left => Vector3.left,
                Direction.Backward => Vector3.back,
                Direction.Down => Vector3.down,
                _ => Vector3.right
            };

            var leverArmAxis = swingArmZeroRotationDirection switch
            {
                Direction.Right => Vector3.right,
                Direction.Up => Vector3.up,
                Direction.Forward => Vector3.forward,
                Direction.Left => Vector3.left,
                Direction.Backward => Vector3.back,
                Direction.Down => Vector3.down,
                _ => Vector3.right
            };

            initialSwingArmRotation = Vector3.Angle(leverArmAxis,
                rotationAxisReferenceFrame.InverseTransformDirection(
                    swingArmEndpoint.position - swingArmCenter.position));
            initialSwingArmRotationInRadians = initialSwingArmRotation * Mathf.Deg2Rad;
            initialCircularConditionsAlreadyCalculated = true;
            
#if UNITY_EDITOR
            // CRITICAL: Tells Unity to save this change to the scene/prefab
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Calculates and initializes the parameters required to enable horizontal shifting mechanisms
        /// when transitioning from circular motion constraints to straight-line motion. This method determines
        /// the shift direction, computes the initial horizontal shift based on the swing arm's properties and
        /// rotation, and establishes a local displacement reference point relative to the configured
        /// translation axis reference frame. Once complete, flags horizontal conditions as initialized,
        /// ensuring they are only derived once during runtime.
        /// </summary>
        private void DeriveHorizontalShiftingInitialConditions(bool bypassInitializationGuards = false)
        {
            if (bypassInitializationGuards || !initialCircularConditionsAlreadyCalculated) initialPosition = toConstrain.position;
            GetShiftVector = horizontalShiftDirection switch
            {
                Direction.Right => GetRightShift,
                Direction.Up => GetUpShift,
                Direction.Forward => GetForwardShift,
                Direction.Left => GetLeftShift,
                Direction.Backward => GetBackwardShift,
                Direction.Down => GetDownShift,
                _ => GetForwardShift
            };
            initialHorizontalShiftUnscaled = swingArmLength * Mathf.Cos(initialSwingArmRotationInRadians);//swingArmLength - swingArmLength * Mathf.Cos(initialSwingArmRotationInRadians);
            initialHorizontalDisplacementPosition =
                translationAxisReferenceFrame.InverseTransformPoint(initialPosition);
            initialHorizontalConditionsAlreadyCalculated = true;
#if UNITY_EDITOR
            // CRITICAL: Tells Unity to save this change to the scene/prefab
            EditorUtility.SetDirty(this);
#endif
        }

        #endregion

        #region Methods - Public API - Constraint Settings and Application

        /// <summary>
        /// Sets the target Transform to be constrained within the circular motion.
        /// This method assigns a specified Transform that will be subject to the
        /// circular motion constraints defined within the component.
        /// </summary>
        /// <param name="target">The Transform component to be constrained by circular motion constraints.</param>
        public void SetTargetToConstrain(Transform target)
        {
            toConstrain = target;
            initialHorizontalConditionsAlreadyCalculated = initialCircularConditionsAlreadyCalculated = false;
        }

        /// <summary>
        /// Calculate and apply angular and potentially horizontal displacement to the swing arm about its rotation and translation axes, respectively,
        /// required to align the swing arm endpoint's vertical displacement with the vertical travel distance passed in by the user.
        /// This results in a circular motion/swing arm constraint in the case of angular displacement,
        /// and potentially a straight line movement constraint should horizontal displacement also be applied.
        /// </summary>
        /// <param name="endpointVerticalTravelDistance">The vertical distance to apply to the endpoint motion constraint.</param>
        public void ApplyEndpointVerticalTravelDistance(float endpointVerticalTravelDistance)
        {
            // Calculate and apply angular and potentially horizontal displacement to the swing arm about its rotation and translation axes, respectivelly,
            // required to align the swing arm endpoint's vertical displacement with the vertical travel distance passed in by the user.
            // This results in a circular motion/swing arm constraint in the case of angular displacement,
            // and potentially a straight line movement constraint should horizontal displacement also be applied.

            if (!isInitialized) return; //Do not proceed if this component failed to initialize

            //Calculate the final angle of the swing arm about its rotation axis after applying the angular displacement required to align the swing arm endpoint's vertical displacement with that which was passed in by the user
            var finalAngle = Mathf.Asin(Mathf.Sin(initialSwingArmRotationInRadians) +
                                        endpointVerticalTravelDistance / swingArmLength);

            // _angularRotation =  finalAngle * Mathf.Rad2Deg - initialSwingArmRotation;
            // _angularQuaternion = Quaternion.AngleAxis(
            //     _angularRotation, //angular displacement (angle traveled from initial rotation) 
            //     rotationAxis);
            //this.suspensionTravelDistance = suspensionTravelDistance;
            // toConstrain.localRotation = _angularQuaternion;

            this.toConstrain.localRotation =
                Quaternion.AngleAxis( //Apply rotation to the swing arm relative to the vertical distance traveled by its endpoint in its rotational reference frame                                                                                                          
                    finalAngle * Mathf.Rad2Deg -
                    initialSwingArmRotation, //angular displacement (angle traveled from initial swing arm rotation) about the swing arm's rotational axis 
                    rotationAxis);

            if (!this.convertToStraightLineMechanism)
                return; //If this constraint is not designed to be a straight line mechanism, do not continue to calculate or apply a horizontal shift

            // _horizontalShiftDistance = swingArmLength - swingArmLength * Mathf.Cos(finalAngle) - initialHorizontalShift;
            // _horizontalShiftDistanceActual = hasScaledParent
            //     ? _horizontalShiftDistance //calculate the horizontal shift distance in the constrained objects rotational travel relative to its initial horizontal distance
            //       / parentScale //account for non-unitary parent scaling
            //     : _horizontalShiftDistance;
            // _horizontalShift = GetShiftVector( //extract the directional shift necessary to maintain straight line movement 
            //     _horizontalShiftDistanceActual);
            // _horizontalShiftPos = translationAxisReferenceFrame.TransformPoint(_horizontalShift+initialHorizontalDisplacementPosition); 
            // toConstrain.position = _horizontalShiftPos;

            this.toConstrain.position = transformedShiftedPosition =
                translationAxisReferenceFrame
                    .TransformPoint( //convert the calculated local shifted horizontal position from the translation axis's reference frame to that of world space
                        (untransformedShiftedPosition = (untransformedShiftVector =
                             GetShiftVector( //extract the directional shift vector within the translation axis's reference selected by the user for straight line movement (translational axis used to maintain straight line movement of the swing arm endpoint) 
                                 GetHorizontalShiftDistance())) //Calculate the horizontal shift distance needed along the translation axis to maintain vertical straight line movement while attached to the rotating swing arm, optionally accounting for parent non-unitary scaling
                         + initialHorizontalDisplacementPosition) //Apply horizontal shift relative to the constrained object's initial position in the translation axis's reference frame
                    );
            suspensionCompressionDistance = endpointVerticalTravelDistance;
            //Calculate the horizontal shift distance in the constrained objects rotational travel relative to its initial horizontal distance (in world space)
            float GetHorizontalShiftDistanceUnscaled() =>
                unscaledShiftedDistance = swingArmLength * Mathf.Cos(finalAngle) - initialHorizontalShiftUnscaled;//swingArmLength - swingArmLength * Mathf.Cos(finalAngle) - initialHorizontalShiftUnscaled;

            //Calculate the horizontal shift distance in the constrained bjects rotational travel relative to its initial horizontal distance, optionally accounting for parent non-unitary scaling
            float GetHorizontalShiftDistance() => processedShiftDistance = hasScaledParent
                ? GetHorizontalShiftDistanceUnscaled() / parentScale
                : GetHorizontalShiftDistanceUnscaled();
        }

        [ShowInInspector] private float unscaledShiftedDistance;
        [ShowInInspector] private float processedShiftDistance;
        [ShowInInspector] private Vector3 untransformedShiftVector;
        [ShowInInspector] private Vector3 untransformedShiftedPosition;
        [ShowInInspector] private Vector3 transformedShiftedPosition;
        [ShowInInspector] private float suspensionCompressionDistance;

        #endregion

        #region Methods - Private API - Translational/Straight Line Mechanism

        /// <summary>
        /// Calculates a forward shift vector based on the given scale.
        /// This method creates a vector pointing forward along the Z-axis, scaled by the input.
        /// Useful for determining horizontal movement in the forward direction.
        /// </summary>
        /// <param name="scale">The scaling factor that determines the magnitude of the horizontal shift forward.</param>
        /// <returns>A Vector3 representing the scaled forward adjustment along the Z-axis.</returns>
        private Vector3 GetForwardShift(float scale) => scale * Vector3.forward;

        /// <summary>
        /// Calculates a rightward shift vector based on the given scale.
        /// This method creates a vector pointing to the right along the X-axis, scaled by the input.
        /// Useful for determining horizontal movement in the rightward direction.
        /// </summary>
        /// <param name="scale">The scaling factor that determines the magnitude of the horizontal shift to the right.</param>
        /// <returns>A Vector3 representing the scaled rightward adjustment along the X-axis.</returns>
        private Vector3 GetRightShift(float scale) => scale * Vector3.right;

        /// <summary>
        /// Calculates a upward shift vector based on the given scale.
        /// This method creates a vector pointing upward along the Y-axis, scaled by the input.
        /// Useful for determining horizontal movement in the upward direction.
        /// </summary>
        /// <param name="scale">The scaling factor that determines the magnitude of the horizontal shift upward.</param>
        /// <returns>A Vector3 representing the scaled upward adjustment along the Y-axis.</returns>
        private Vector3 GetUpShift(float scale) => scale * Vector3.up;

        /// <summary>
        /// Calculates a backward shift vector based on the given scale.
        /// This method creates a vector pointing backward along the negative Z-axis, scaled by the input.
        /// Useful for determining horizontal movement in the backward direction.
        /// </summary>
        /// <param name="scale">The scaling factor that determines the magnitude of the horizontal shift backward.</param>
        /// <returns>A Vector3 representing the scaled backward adjustment along the negative Z-axis.</returns>
        private Vector3 GetBackwardShift(float scale) => scale * Vector3.back;

        /// <summary>
        /// Calculates a leftward shift vector based on the given scale.
        /// This method creates a vector pointing to the left along the negative X-axis, scaled by the input.
        /// Useful for determining horizontal movement in the leftward direction.
        /// </summary>
        /// <param name="scale">The scaling factor that determines the magnitude of the horizontal shift to the left.</param>
        /// <returns>A Vector3 representing the scaled leftward adjustment along the negative X-axis.</returns>
        private Vector3 GetLeftShift(float scale) => scale * Vector3.left;

        /// <summary>
        /// Calculates a downward shift vector based on the given scale.
        /// This method creates a vector pointing upward along the negative Y-axis, scaled by the input.
        /// Useful for determining horizontal movement in the downward direction.
        /// </summary>
        /// <param name="scale">The scaling factor that determines the magnitude of the horizontal shift downward.</param>
        /// <returns>A Vector3 representing the scaled downward adjustment along the negative Y-axis.</returns>
        private Vector3 GetDownShift(float scale) => scale * Vector3.down;

        #endregion

        private enum Direction
        {
            Right,
            Forward,
            Up,
            Left,
            Backward,
            Down
        }
    }
}
