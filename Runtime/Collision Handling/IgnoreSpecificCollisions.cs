using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using PsigenVision.Utilities.Combinatorics;

namespace PsigenVision.Utilities.PhysX.Collisions
{
    /// <summary>
    /// A Unity MonoBehaviour class designed to control and customize collision interactions between specific sets of colliders.
    /// Provides methods for collecting colliders from parent transforms, initializing states, and enabling or checking the status of collision ignoring processes.
    /// </summary>
    public class IgnoreSpecificCollisions : MonoBehaviour
    {
#if UNITY_EDITOR
        /// <summary>
        /// An array of transforms representing parent objects whose child colliders will be collected
        /// to define entities that will ignore specific collision interactions.
        /// </summary>
        [Tooltip("The parents of the colliders that the user wants to ignore the collisions of other colliders")]
        [SerializeField]
        private Transform[] toDoIgnoringParents;

        /// <summary>
        /// An array of parent transforms whose child colliders will be gathered
        /// and designated to serve as the entities that will be ignored during collision detection processes.
        /// </summary>
        [Tooltip("The parents of the colliders that the user wants to be ignored by certain colliders")]
        [SerializeField]
        private Transform[] toBeIgnoredParents;

        /// <summary>
        /// A flag indicating whether the collection of colliders is currently being executed.
        /// Used to prevent multiple simultaneous executions of the collider collection process.
        /// </summary>
        private bool colliderCollectingInProgress = false;

        /// <summary>
        /// Collects all colliders from the specified parent transforms and categorizes them based on their associations for specific collision handling.
        /// This involves iterating through the provided parent transform arrays representing "to do ignoring" and "to be ignored", gathering their child colliders,
        /// and preparing the final categorized collections to be used for collision rules. The method executes as a coroutine to support asynchronous processing
        /// and avoids blocking the main thread during potentially time-consuming collider collection and grouping operations.
        /// </summary>
        /// <returns>
        /// An IEnumerator representing the coroutine execution. The method enables asynchronous handling of collider collection and categorization,
        /// supporting smoother execution during operations involving significant amounts of colliders.
        /// </returns>
        public IEnumerator CollectColliders()
        {
            if (colliderCollectingInProgress == true) yield break;
            if (toDoIgnoringParents == null || toDoIgnoringParents.Length == 0) yield break;
            colliderCollectingInProgress = true;
            var newToDoIgnoring = new List<Collider>();
            var newToBeIgnored = new List<Collider>();
            //Collect all colliders contained in the parents provided to represent who will be doing the ignoring
            for (var ignoringIndex = 0; ignoringIndex < toDoIgnoringParents.Length; ignoringIndex++)
            {
                var currentParent = toDoIgnoringParents[ignoringIndex];
                if (currentParent == null) continue;
                var colliders = currentParent.GetComponentsInChildren<Collider>();
                yield return new WaitForSeconds(1f);

                if (colliders == null || colliders.Length == 0) continue;
                for (var j = 0; j < colliders.Length; j++)
                {
                    newToDoIgnoring.Add(colliders[j]);
                    yield return null;
                }

                yield return null;
            }

            for (var ignoredIndex = 0; ignoredIndex < toBeIgnoredParents.Length; ignoredIndex++)
            {
                var currentParent = toBeIgnoredParents[ignoredIndex];
                if (currentParent == null) continue;
                var colliders = currentParent.GetComponentsInChildren<Collider>();
                yield return new WaitForSeconds(1f);

                if (colliders == null || colliders.Length == 0) continue;
                for (var j = 0; j < colliders.Length; j++)
                {
                    newToBeIgnored.Add(colliders[j]);
                    yield return null;
                }

                yield return null;
            }

            yield return Combinator.PrepareForUniqueComparisonBetweenCoroutine(newToDoIgnoring, newToBeIgnored,
                AssignResult);
            colliderCollectingInProgress = false;

            void AssignResult((Collider[] onlyA, Collider[] onlyB, Collider[] both) result)
            {
                toDoIgnoring = result.onlyA;
                toBeIgnored = result.onlyB;
                toBeIgnoredInBoth = result.both;
            }
        }
#endif

        /// <summary>
        /// An array of colliders designated to act as the ignoring group during collision detection.
        /// These colliders will be configured to ignore interactions with specified target colliders.
        /// </summary>
        [Tooltip("An array of ordinary colliders that will be ignoring collisions with the other colliders.")]
        [SerializeField]
        private Collider[] toDoIgnoring;

        /// <summary>
        /// An array of colliders that are designated to be ignored during collision detection.
        /// This group of colliders will not register collisions with other specified colliders.
        /// </summary>
        [Tooltip("An array of ordinary colliders that should be ignored when detecting collisions.")] [SerializeField]
        private Collider[] toBeIgnored;

        /// <summary>
        /// An array of colliders that are mutually ignored in collision detection.
        /// These colliders will ignore collisions with each other, ensuring no interaction between them during collision events.
        /// </summary>
        [FormerlySerializedAs("toBeIgnored")]
        [Tooltip("An array of ordinary colliders that should be ignored by all when detecting collisions.")]
        [SerializeField]
        private Collider[] toBeIgnoredInBoth;

        /// <summary>
        /// A flag indicating whether the initialization process for the collision management system has been completed.
        /// This property returns true if the system has been successfully initialized, otherwise false.
        /// </summary>
        public bool IsInitialized => isInitialized;

        /// <summary>
        /// Indicates whether the system allows specified collision pairs to be ignored.
        /// When enabled, collisions between designated sets of colliders will not be processed,
        /// effectively managing collision behaviors in the context of the application's requirements.
        /// </summary>
        public bool CanIgnore => allowIgnoreCollisions;

        /// <summary>
        /// Represents the current state indicating whether collisions are being actively ignored.
        /// Returns true if the process of ignoring designated collisions between specified collider groups is ongoing.
        /// </summary>
        public bool IgnoreInProgress => collisionsCurrentlyIgnored;

        /// <summary>
        /// The collider component associated with the GameObject to which this script is attached.
        /// Used to manage and manipulate collision behavior specific to this GameObject.
        /// </summary>
        private Collider thisCollider;

        /// <summary>
        /// Indicates whether the component should automatically initialize during the Awake phase of the Unity lifecycle.
        /// </summary>
        [Tooltip("Whether the component should initialize during the Awake phase of the Unity lifecycle")]
        [SerializeField]
        private bool initializeOnAwake = false;

        /// <summary>
        /// Determines whether collisions will be automatically ignored for specified colliders
        /// during the Awake phase of the Unity lifecycle.
        /// </summary>
        [Tooltip("Whether collisions should automatically be ignored at the Awake phase of the Unity lifecycle")]
        [SerializeField]
        private bool ignoreCollisionsOnAwake = false;

        /// <summary>
        /// A boolean flag indicating whether the initialization process for managing collision behaviors
        /// has been successfully completed. When set to true, the component is ready to handle collision
        /// ignoring functionality between specified collider groups.
        /// </summary>
        private bool isInitialized = false;

        /// <summary>
        /// A boolean flag indicating whether collisions can be ignored between the specified sets of colliders.
        /// This property is used to enable or disable the collision-ignoring functionality in the system.
        /// </summary>
        private bool allowIgnoreCollisions = false;

        /// <summary>
        /// A flag indicating whether collisions are currently being ignored based on the configured collision ignoring logic.
        /// Set to true when collision ignoring is actively in progress or has been applied; otherwise, false.
        /// </summary>
        private bool collisionsCurrentlyIgnored = false;

        /// <summary>
        /// A boolean flag indicating whether the process of ignoring collisions between specified collider groups
        /// is currently in progress. Used to prevent re-entry or multiple simultaneous collision ignoring routines.
        /// </summary>
        private bool collisionIgnoringInProgress = false;

        /// <summary>
        /// Executes during the Unity lifecycle's Awake phase, ensuring necessary initializations and collision configurations
        /// are applied if the component's properties are set to do so.
        /// Specifically, it conditionally initializes the component and sets up automated collision ignoring
        /// based on the serialized settings.
        /// </summary>
        private void Awake()
        {
            if (initializeOnAwake) Initialize();
            if (ignoreCollisionsOnAwake) IgnoreContainedCollisions();
        }

        /// <summary>
        /// Initializes the component by validating the presence of a Collider on the current GameObject
        /// and setting up the ability to ignore collisions based on the specified configuration.
        /// </summary>
        /// <returns>
        /// True if initialization was successful and collisions can be ignored, false otherwise.
        /// </returns>
        public bool Initialize()
        {
            isInitialized = true;
            allowIgnoreCollisions = true;

            if (toBeIgnored == null || toBeIgnored.Length == 0)
            {
                Debug.LogWarning("This component has no colliders to be ignored");
                allowIgnoreCollisions = false;
                isInitialized = true;
            }

            if (toDoIgnoring == null || toDoIgnoring.Length == 0)
            {
                Debug.LogWarning("This component has no colliders to do the ignoring");
                allowIgnoreCollisions = false;
                isInitialized = true;
            }

            return isInitialized;
        }

        /// <summary>
        /// Handles the process of ignoring collision detection between specified sets of colliders
        /// managed by the IgnoreSpecificCollisions component. This coroutine leverages optimized
        /// pairwise processing logic to efficiently manage collision rules while ensuring sequential progress
        /// through asynchronous execution. The method only proceeds when collisions are allowed to be ignored
        /// and no other collision ignoring process is currently active.
        /// </summary>
        /// <returns>
        /// An IEnumerator representing the coroutine execution. Provides asynchronous processing
        /// to efficiently apply collision ignoring rules across collider groups, ensuring minimal
        /// performance impact during runtime.
        /// </returns>
        private IEnumerator IgnoreContainedCollisionsCoroutine()
        {
            /*
         * Instead of `foreach` or LINQ, use `CollectionsMarshal.AsSpan` to access the underlying memory of your lists directly.
         * This removes the overhead of the list enumerator and bounds checking in every iteration.
         */
            if (!allowIgnoreCollisions || collisionsCurrentlyIgnored) yield break;
            collisionIgnoringInProgress = true;
            yield return Combinator.ProcessAllUniquePairsBetween(toDoIgnoring, toBeIgnored, toBeIgnoredInBoth,
                Physics.IgnoreCollision);
            collisionIgnoringInProgress = false;
            collisionsCurrentlyIgnored = true;
        }

        /// <summary>
        /// Initiates the process of ignoring collisions between the collider attached to the current GameObject
        /// and a designated set of colliders. The method checks internal states to ensure collisions
        /// are only ignored when allowed and not already in progress.
        /// </summary>
        public void IgnoreContainedCollisions()
        {
            if (!collisionIgnoringInProgress) StartCoroutine(IgnoreContainedCollisionsCoroutine());
        }
    }
}
