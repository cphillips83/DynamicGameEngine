
namespace Dynamic.GameEngine.Math
{
    public static class Extensions
    {
        private const float EPSILON = 1e-03f;

        public static Real Dot(this OpenTK.Vector2 left, OpenTK.Vector2 right)
        {
            return OpenTK.Vector2.Dot(left, right);
        }


        public static Real Dot(this OpenTK.Vector3 left, OpenTK.Vector3 right)
        {
            return OpenTK.Vector3.Dot(left, right);
        }

        public static OpenTK.Vector3 Cross(this OpenTK.Vector3 left, OpenTK.Vector3 right)
        {
            return OpenTK.Vector3.Cross(left, right);
        }

        public static OpenTK.Vector3 Ceiling(this OpenTK.Vector3 left, OpenTK.Vector3 right)
        {
            if (right.X > left.X)
            {
                left.X = right.X;
            }
            if (right.Y > left.Y)
            {
                left.Y = right.Y;
            }
            if (right.Z > left.Z)
            {
                left.Z = right.Z;
            }

            return left;
        }

        public static OpenTK.Vector2 ToNormalized(this OpenTK.Vector2 left)
        {
            left.Normalize();
            return left;
        }

        public static OpenTK.Vector3 ToNormalized(this OpenTK.Vector3 left)
        {
            left.Normalize();
            return left;
        }

        public static OpenTK.Vector2 Perpendicular(this OpenTK.Vector2 left)
        {
            return new OpenTK.Vector2(left.Y, -left.X);
        }

        public static OpenTK.Vector3 Perpendicular(this OpenTK.Vector3 left)
        {
            OpenTK.Vector3 result = left.Cross(OpenTK.Vector3.UnitX);

            // check length
            if (result.LengthSquared < Real.Epsilon)
            {
                // This vector is the Y axis multiplied by a scalar, so we have to use another axis
                result = left.Cross(OpenTK.Vector3.UnitY);
            }

            return result;
        }

        public static OpenTK.Vector2 Rotate(this OpenTK.Vector2 left, OpenTK.Vector2 pivot, Real amount)
        {
            var rot = pivot.GetRotation(left);
            rot += amount;

            var dir = new OpenTK.Vector2(Utility.Cos(rot), Utility.Sin(rot));
            return dir * (left - pivot).Length + pivot;
        }

        public static Radian GetRotation(this OpenTK.Vector2 left, OpenTK.Vector2 right)
        {
            if (left == right)
                return Utility.ATan2(0, 0);

            var diff = (right - left).ToNormalized();
            return Utility.ATan2(diff.Y, diff.X);
        }

        public static Radian GetRotation(this OpenTK.Vector2 left)
        {
            var diff = left.ToNormalized();
            return Utility.ATan2(diff.Y, diff.X);
        }

        public static OpenTK.Matrix4 ToMatrix4(this OpenTK.Matrix3 left)
        {
            OpenTK.Matrix4 result = OpenTK.Matrix4.Identity;

            result.M11 = left.M11;
            result.M12 = left.M12;
            result.M13 = left.M13;
            result.M21 = left.M21;
            result.M22 = left.M22;
            result.M23 = left.M23;
            result.M31 = left.M31;
            result.M32 = left.M32;
            result.M33 = left.M33;

            return result;
        }

        public static OpenTK.Vector2 Transform(this OpenTK.Vector2 position, OpenTK.Matrix3 matrix)
        {
            return position.Transform(matrix.ToMatrix4());
        }

        public static OpenTK.Vector2 Transform(this OpenTK.Vector2 position,  OpenTK.Matrix4 matrix)
        {
            return new OpenTK.Vector2((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41,
                                 (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
        }

        public static OpenTK.Vector3 Transform(this OpenTK.Vector3 left, OpenTK.Matrix3 right)
        {
            OpenTK.Vector3 product = new OpenTK.Vector3();

            product.X = right.M11 * left.X + right.M12 * left.Y + right.M13 * left.Z;
            product.Y = right.M21 * left.X + right.M22 * left.Y + right.M23 * left.Z;
            product.Z = right.M31 * left.X + right.M32 * left.Y + right.M33 * left.Z;

            return product;
        }

        public static Real AbsDot(this OpenTK.Vector3 left, OpenTK.Vector3 right)
        {
            return System.Math.Abs(left.X * right.X) + System.Math.Abs(left.Y * right.Y) + System.Math.Abs(left.Z * right.Z);
        }

        public static OpenTK.Quaternion Squad(Real t, OpenTK.Quaternion p, OpenTK.Quaternion a, OpenTK.Quaternion b, OpenTK.Quaternion q)
        {
            return Squad(t, p, a, b, q, false);
        }

        /// <summary>
        ///		Performs spherical quadratic interpolation.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static OpenTK.Quaternion Squad(Real t, OpenTK.Quaternion p, OpenTK.Quaternion a, OpenTK.Quaternion b, OpenTK.Quaternion q, bool useShortestPath)
        {
            Real slerpT = 2.0f * t * (1.0f - t);

            // use spherical linear interpolation
            OpenTK.Quaternion slerpP = Slerp(t, p, q, useShortestPath);
            OpenTK.Quaternion slerpQ = Slerp(t, a, b);

            // run another Slerp on the results of the first 2, and return the results
            return Slerp(slerpT, slerpP, slerpQ);
        }

        public static OpenTK.Quaternion Slerp(Real time, OpenTK.Quaternion quatA, OpenTK.Quaternion quatB)
        {
            return Slerp(time, quatA, quatB, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="quatA"></param>
        /// <param name="quatB"></param>
        /// <param name="useShortestPath"></param>
        /// <returns></returns>
        public static OpenTK.Quaternion Slerp(Real time, OpenTK.Quaternion quatA, OpenTK.Quaternion quatB, bool useShortestPath)
        {
            Real cos = quatA.Dot(quatB);

            Real angle = (Real)Utility.ACos(cos);

            if (Utility.Abs(angle) < EPSILON)
            {
                return quatA;
            }

            Real sin = Utility.Sin(angle);
            Real inverseSin = 1.0f / sin;
            Real coeff0 = Utility.Sin((1.0f - time) * angle) * inverseSin;
            Real coeff1 = Utility.Sin(time * angle) * inverseSin;

            OpenTK.Quaternion result;

            if (cos < 0.0f && useShortestPath)
            {
                coeff0 = -coeff0;
                // taking the complement requires renormalisation
                OpenTK.Quaternion t = coeff0 * quatA + coeff1 * quatB;
                t.Normalize();
                result = t;
            }
            else
            {
                result = (coeff0 * quatA + coeff1 * quatB);
            }

            return result;
        }

        /// <overloads><summary>
        /// normalised linear interpolation - faster but less accurate (non-constant rotation velocity)
        /// </summary>
        /// <param name="fT"></param>
        /// <param name="rkP"></param>
        /// <param name="rkQ"></param>
        /// <returns></returns>
        /// </overloads>
        public static OpenTK.Quaternion Nlerp(Real fT, OpenTK.Quaternion rkP, OpenTK.Quaternion rkQ)
        {
            return Nlerp(fT, rkP, rkQ, false);
        }

        /// <param name="shortestPath"></param>
        public static OpenTK.Quaternion Nlerp(Real fT, OpenTK.Quaternion rkP, OpenTK.Quaternion rkQ, bool shortestPath)
        {
            OpenTK.Quaternion result;
            Real fCos = rkP.Dot(rkQ);
            if (fCos < 0.0f && shortestPath)
            {
                result = rkP + fT * ((OpenTK.Quaternion.Invert(rkQ)) - rkP);
            }
            else
            {
                result = rkP + fT * (rkQ - rkP);
            }
            result.Normalize();
            return result;
        }

        /// <summary>
        /// Performs a Dot Product operation on 2 Quaternions.
        /// </summary>
        /// <param name="quat"></param>
        /// <returns></returns>
        public static Real Dot(this OpenTK.Quaternion left, OpenTK.Quaternion right)
        {
            return left.W * right.W + left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

    }
}
