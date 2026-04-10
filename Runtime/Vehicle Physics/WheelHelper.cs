using UnityEngine;


namespace PsigenVision.Utilities.PhysX.Vehicle
{
	/// <summary>
	/// Utility class to compute the steering angles for wheels based on the Ackerman steering geometry.
	/// </summary>
	public class WheelHelper
	{
		/// <summary>
		/// Calculates the Ackerman steering angles for the left and right wheels
		/// based on the specified turn radius, wheel base, and track width.
		/// </summary>
		/// <param name="turnRadius">The radius of the vehicle's turning center.</param>
		/// <param name="wheelBase">The distance between the front and rear axles of the vehicle.</param>
		/// <param name="trackWidth">The distance between the left and right wheels on the same axle.</param>
		/// <param name="steerAngle">The resulting steering angles for the left and right wheels in degrees.</param>
		public static void GetAckermanAnglesFromRadius(float turnRadius, float wheelBase, float trackWidth,
			out (float left, float right) steerAngle)
		{
			steerAngle = (
				Mathf.Atan(wheelBase / (turnRadius + trackWidth / 2)) * Mathf.Rad2Deg,
				Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2)) * Mathf.Rad2Deg
			);
		}

		/// <summary>
		/// Calculates the Ackerman steering angles for left and right wheels based on the provided turn radius, wheel base, and track width.
		/// </summary>
		/// <param name="turnRadius">The turning radius of the vehicle's center of rotation.</param>
		/// <param name="wheelBase">The distance between the front and rear axles of the vehicle.</param>
		/// <param name="trackWidth">The distance between the left and right wheels on the same axle of the vehicle.</param>
		/// <param name="leftSteerAngle">The calculated steer angle for the left wheel, expressed in degrees.</param>
		/// <param name="rightSteerAngle">The calculated steer angle for the right wheel, expressed in degrees.</param>
		public static void GetAckermanAnglesFromRadius(float turnRadius, float wheelBase, float trackWidth,
			out float leftSteerAngle, out float rightSteerAngle)
		{
			leftSteerAngle = Mathf.Atan(wheelBase / (turnRadius + trackWidth / 2)) * Mathf.Rad2Deg;
			rightSteerAngle = Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2)) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Calculates the Ackerman steering angles in radians for the left and right wheels
		/// based on the provided turn radius, wheelbase, and track width.
		/// </summary>
		/// <param name="turnRadius">The radius of the turn, measured from the center of the turn to the midpoint of the rear axle.</param>
		/// <param name="wheelBase">The distance between the front and rear axles of the vehicle.</param>
		/// <param name="trackWidth">The distance between the left and right wheels on the same axle of the vehicle.</param>
		/// <param name="steerAngle">The calculated steering angles for the left and right wheels, in radians.</param>
		public static void GetAckermanAnglesRadiansFromRadius(float turnRadius, float wheelBase, float trackWidth,
			out (float left, float right) steerAngle)
		{
			steerAngle = (
				Mathf.Atan(wheelBase / (turnRadius + trackWidth / 2)),
				Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2))
			);
		}

		/// <summary>
		/// Calculates the Ackerman steering angles in radians for the left and right wheels
		/// based on the provided turn radius, wheel base, and track width.
		/// </summary>
		/// <param name="turnRadius">The turning radius of the vehicle's center of rotation.</param>
		/// <param name="wheelBase">The distance between the front and rear axles of the vehicle.</param>
		/// <param name="trackWidth">The distance between the left and right wheels on the same axle of the vehicle.</param>
		/// <param name="leftSteerAngle">The calculated steer angle for the left wheel, in radians.</param>
		/// <param name="rightSteerAngle">The calculated steer angle for the right wheel, in radians.</param>
		public static void GetAckermanAnglesRadiansFromRadius(float turnRadius, float wheelBase, float trackWidth,
			out float leftSteerAngle, out float rightSteerAngle)
		{
			leftSteerAngle = Mathf.Atan(wheelBase / (turnRadius + trackWidth / 2));
			rightSteerAngle = Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2));
		}

		/// <summary>
		/// Calculates the Ackerman steering angles for left and right wheels based on the provided turn angle, wheel base, and track width.
		/// Offers an option to handle scenarios where the turning angle is approximately zero.
		/// </summary>
		/// <param name="turnAngle">The steering angle of the vehicle, expressed in degrees.</param>
		/// <param name="wheelBase">The distance between the front and rear axles of the vehicle.</param>
		/// <param name="trackWidth">The distance between the left and right wheels on the same axle of the vehicle.</param>
		/// <param name="steerAngle">The calculated steer angles for the left and right wheels, expressed in degrees.</param>
		/// <param name="checkForZeroAngle">Indicates if a check should be performed for zero turning angle. If true and the angle is approximately zero, the steer angles will be set to zero.</param>
		public static void GetAckermanAnglesFromTurnAngle(float turnAngle, float wheelBase, float trackWidth,
			out (float left, float right) steerAngle, bool checkForZeroAngle = false)
		{
			if (checkForZeroAngle && Mathf.Approximately(turnAngle, 0f))
			{
				steerAngle = (0, 0);
				return;
			}

			// Formula derivation uses the cotangent function, which is 1/tan.
			// The ideal radius is derived from the master angle: R = WheelBase / tan(masterAngle).
			var turnAngleRadians = turnAngle * Mathf.Deg2Rad;
			var turnRadius = wheelBase / Mathf.Tan(turnAngleRadians);
			steerAngle = (
				Mathf.Atan(wheelBase/(turnRadius + trackWidth/2))*Mathf.Rad2Deg,
				Mathf.Atan(wheelBase/(turnRadius - trackWidth/2))*Mathf.Rad2Deg
			);
			/*if (turnAngle > 0f){
				steerAngle = (
					Mathf.Atan(wheelBase/(turnRadius + trackWidth/2))*Mathf.Rad2Deg,
					Mathf.Atan(wheelBase/(turnRadius - trackWidth/2))*Mathf.Rad2Deg
				);
			}
			else
			{
				steerAngle = (
					Mathf.Atan(wheelBase/(turnRadius - trackWidth/2))*Mathf.Rad2Deg,
					Mathf.Atan(wheelBase/(turnRadius + trackWidth/2))*Mathf.Rad2Deg
				);
			}*/

		}

		/// <summary>
		/// Calculates the Ackerman steering angles for left and right wheels in degrees based on the provided turn angle, wheel base, and track width.
		/// </summary>
		/// <param name="turnAngle">The turning angle of the vehicle, in degrees.</param>
		/// <param name="wheelBase">The distance between the front and rear axles of the vehicle.</param>
		/// <param name="trackWidth">The distance between the left and right wheels on the same axle of the vehicle.</param>
		/// <param name="leftSteerAngle">The calculated steer angle for the left wheel, in degrees.</param>
		/// <param name="rightSteerAngle">The calculated steer angle for the right wheel, in degrees.</param>
		/// <param name="checkForZeroAngle">If true, checks whether the turn angle is approximately zero and sets the steer angles to zero in that case.</param>
		public static void GetAckermanAnglesFromTurnAngle(float turnAngle, float wheelBase, float trackWidth,
			out float leftSteerAngle, out float rightSteerAngle, bool checkForZeroAngle = false)
		{
			if (checkForZeroAngle && Mathf.Approximately(turnAngle, 0f))
			{
				leftSteerAngle = rightSteerAngle = 0;
				return;
			}

			// Formula derivation uses the cotangent function, which is 1/tan.
			// The ideal radius is derived from the master angle: R = WheelBase / tan(masterAngle).
			var turnAngleRadians = turnAngle * Mathf.Deg2Rad;
			var turnRadius = wheelBase / Mathf.Tan(turnAngleRadians);
			leftSteerAngle = Mathf.Atan(wheelBase / (turnRadius + trackWidth / 2)) * Mathf.Rad2Deg;
			rightSteerAngle = Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2)) * Mathf.Rad2Deg;
			/*if (turnAngle > 0f)
			{
				leftSteerAngle = Mathf.Atan(wheelBase / (turnRadius + trackWidth / 2)) * Mathf.Rad2Deg;
				rightSteerAngle = Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2)) * Mathf.Rad2Deg;
			}
			else
			{
				leftSteerAngle = Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2)) * Mathf.Rad2Deg;
				rightSteerAngle = Mathf.Atan(wheelBase / (turnRadius + trackWidth / 2)) * Mathf.Rad2Deg;
			}*/

		}

		/// <summary>
		/// Calculates the Ackerman steering angles in radians for the left and right wheels
		/// based on the provided turn radius, wheelbase, and track width.
		/// </summary>
		/// <param name="turnRadius">The turning radius of the vehicle's center of rotation.</param>
		/// <param name="wheelBase">The distance between the front and rear axles of the vehicle.</param>
		/// <param name="trackWidth">The distance between the left and right wheels on the same axle of the vehicle.</param>
		/// <param name="steerAngle">The calculated steering angles for the left and right wheels, expressed in radians.</param>
		public static void GetAckermanAnglesRadiansFromTurnAngle(float turnAngle, float wheelBase, float trackWidth,
			out (float left, float right) steerAngle, bool checkForZeroAngle = false)
		{
			if (checkForZeroAngle && Mathf.Approximately(turnAngle, 0f))
			{
				steerAngle = (0, 0);
				return;
			}

			// Formula derivation uses the cotangent function, which is 1/tan.
			// The ideal radius is derived from the master angle: R = WheelBase / tan(masterAngle).
			var turnAngleRadians = turnAngle * Mathf.Deg2Rad;
			var turnRadius = wheelBase / Mathf.Tan(turnAngleRadians);
			steerAngle = (
				Mathf.Atan(wheelBase/(turnRadius + trackWidth/2)),
				Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2))
			);
			// if (turnAngle > 0f){
			// 	steerAngle = (
			// 		Mathf.Atan(wheelBase/(turnRadius + trackWidth/2)),
			// 		Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2))
			// 	);
			// }
			// else
			// {
			// 	steerAngle = (
			// 		Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2)),
			// 		Mathf.Atan(wheelBase/(turnRadius + trackWidth/2))
			// 	);
			// }

		}

		/// <summary>
		/// Calculates the Ackerman steering angles for left and right wheels in radians
		/// based on the provided turn radius, wheel base, and track width.
		/// </summary>
		/// <param name="turnRadius">The turning radius of the vehicle's center of rotation.</param>
		/// <param name="wheelBase">The distance between the front and rear axles of the vehicle.</param>
		/// <param name="trackWidth">The distance between the left and right wheels on the same axle of the vehicle.</param>
		/// <param name="steerAngle">The calculated steer angles for the left and right wheels, expressed in radians.</param>
		public static void GetAckermanAnglesRadiansFromTurnAngle(float turnAngle, float wheelBase, float trackWidth,
			out float leftSteerAngle, out float rightSteerAngle, bool checkForZeroAngle = false)
		{
			if (checkForZeroAngle && Mathf.Approximately(turnAngle, 0f))
			{
				leftSteerAngle = rightSteerAngle = 0;
				return;
			}

			// Formula derivation uses the cotangent function, which is 1/tan.
			// The ideal radius is derived from the master angle: R = WheelBase / tan(masterAngle).
			var turnAngleRadians = turnAngle * Mathf.Deg2Rad;
			var turnRadius = wheelBase / Mathf.Tan(turnAngleRadians);
			leftSteerAngle = Mathf.Atan(wheelBase / (turnRadius + trackWidth / 2));
			rightSteerAngle = Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2));
			// if (turnAngle > 0f){
			// 	leftSteerAngle = Mathf.Atan(wheelBase / (turnRadius + trackWidth / 2));
			// 	rightSteerAngle = Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2));
			// }
			// else 
			// {
			// 	leftSteerAngle = Mathf.Atan(wheelBase / (turnRadius - trackWidth / 2));
			// 	rightSteerAngle = Mathf.Atan(wheelBase / (turnRadius + trackWidth / 2));
			// }

		}
		
		// Formula derivation uses the cotangent function, which is 1/tan.
		// The ideal radius is derived from the master angle: R = WheelBase / tan(masterAngle).
		
	}
}