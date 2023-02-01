using System;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Structure for the linear formula `px + qy = r`
    /// </summary>
    public struct PqrForm : IEquatable<PqrForm>
    {
        /// <summary>The p constant in the linear formula `px + qy = r`</summary>
        public float P;

        /// <summary>the q constant in the linear formula `px + qy = r`</summary>
        public float Q;

        /// <summary>the r constant in the linear formula `px + qy = r`</summary>
        public float R;

        /// <summary>
        /// Method to get the point (x, 0) where x is an arbitrary number and 0 the y-value.<br/>
        /// This method can also return `null`. When null is returned, the line doesn't intersect with the x-axis.<br/>
        /// This method can also return `+Infinity` meaning that the line has no slope, but also has a `R` value of 0 
        /// </summary>
        /// <returns>
        /// The point where the line intersects with the x-axis
        /// </returns>
        public Vector2? Root
        {
            get
            {
                if (!float.IsInfinity(GetX(0))) return new Vector2(GetX(0), 0);

                if (R == 0)
                {
                    return new Vector2(float.PositiveInfinity, 0);
                }

                return null;
            }
        }

        /// <summary>
        /// Method to get the point (0, y) where y is an arbitrary number and 0 is the x-value.<br/>
        /// This method can also return `null`. When null is returned, the line doesn't intersect with the y-axis.<br/>
        /// This method can also return `+Infinity` meaning that the line is equal to `x = 0`.
        /// </summary>
        /// <returns>
        /// The point where the line intersects with the y-axis;
        /// </returns>
        public Vector2? YIcept
        {
            get
            {
                if (float.IsInfinity(GetY(0)))
                {
                    if (R == 0)
                    {
                        return new Vector2(0, float.PositiveInfinity);
                    }

                    return null;
                }

                return new Vector2(0, GetY(0));
            }
        }

        /// <summary>
        /// Gets the slope of the formula
        /// </summary>
        public float Slope
        {
            get
            {
                if (Q == 0)
                {
                    return float.PositiveInfinity;
                }

                return -P / Q;
            }
            set
            {
                if (float.IsInfinity(value))
                {
                    P = 1;
                    Q = 0;
                    return;
                }

                P = -value;
                Q = 1;
            }
        }

        /// <summary>
        /// Gets the angle of the slope in radians
        /// </summary>
        public float Angle => Mathf.Atan2(Slope, 1);

        /// <summary>
        /// Constructor to create a line between point a to point b
        /// </summary>
        /// <param name="point1">Point a</param>
        /// <param name="point2">Point b</param>
        public PqrForm(Vector2 point1, Vector2 point2) : this()
        {
            WithPoints(point1, point2);
        }

        /// <summary>
        /// Constructor to create a `px + pq = r` formula from a `y = ax + b` formula
        /// </summary>
        /// <param name="a">The a variable in the `y = ax + b` formula</param>
        /// <param name="b">The b variable in the `y = ax + b` formula</param>
        public PqrForm(float a, float b) : this()
        {
            WithAb(a, b);
        }

        /// <summary>
        /// Constructor to create a formula from a slope and an arbitrary point through which the formula should pass
        /// </summary>
        /// <param name="slope">The slope of the formula</param>
        /// <param name="point">The point that should be on the formula</param>
        public PqrForm(float slope, Vector2 point) : this()
        {
            WithSlopePoint(slope, point);
        }

        /// <summary>
        /// Constructor to copy a formula
        /// </summary>
        /// <param name="copy">The formula that gets copied</param>
        public PqrForm(PqrForm copy)
        {
            P = copy.P;
            Q = copy.Q;
            R = copy.R;
        }

        /// <summary>
        /// Method to get the X value of a formula at the given Y-position
        /// </summary>
        /// <param name="y">Y-position you want to know the X-value of</param>
        /// <returns>
        /// The X-position.<br/>
        /// When the line has a slope of 0, this method will return +Infinity
        /// </returns>
        public float GetX(float y)
        {
            if (P != 0)
            {
                return (R - Q * y) / P;
            }

            return float.PositiveInfinity;
        }

        /// <summary>
        /// Method to get the Y value of a formula at the given X-position
        /// </summary>
        /// <param name="x">X-position you want to know the Y-value of</param>
        /// <returns>
        /// The Y-position.<br/>
        /// When the slope of the formula is `Infinity`, this method will return +Infinity
        /// </returns>
        public float GetY(float x)
        {
            if (Q != 0)
            {
                return (R - P * x) / Q;
            }

            return float.PositiveInfinity;
        }

        /// <summary>
        /// Method to get a slope from a given angle
        /// </summary>
        /// <param name="angle">The angle in radians</param>
        /// <returns>The slope that gets calculated with the given angle</returns>
        public static double SlopeFromAngle(double angle)
        {
            if (Math.Abs(Math.Abs(angle) - Math.PI / 2) < 0.0001f)
            {
                return float.PositiveInfinity;
            }

            return Math.Tan(angle);
        }


        /// <summary>
        /// Method to get an angle from a given slope
        /// </summary>
        /// <param name="slope">The given slope</param>
        /// <returns>The angle in radians calculated with the given slope</returns>
        public static double AngleFromSlope(double slope) => Math.Atan2(slope, 1);

        /// <summary>
        /// Method to get the formula that will intersect this formula at a right-angle
        /// </summary>
        /// <returns>A formula that intersects with a right-angle</returns>
        public PqrForm Perpendicular() =>
            new()
            {
                P = Q,
                Q = -P,
                R = R,
            };

        public PqrForm Perpendicular(Vector2 through)
        {
            PqrForm form = new PqrForm();
            form.PerpendicularTo(this, through);
            
            return form;
        }
        
        /// <summary>
        /// Checks if two formula's intersect with each other and what kind of  
        /// </summary>
        /// <param name="a">Formula a</param>
        /// <param name="b">Formula b</param>
        /// <param name="tolerance">Tolerance check since it's decimal arithmetic</param>
        /// <returns>The type of intersection:
        /// <code>
        /// Does, Once - when it only intersects once
        /// Does, Continuous - when the lines overlap
        /// Not - when the lines don't intersect
        /// </code>
        /// </returns>
        public static IntersectionType DoesIntersect(PqrForm a, PqrForm b, double tolerance)
        {
            float slopeDiff = Math.Abs(a.Slope - b.Slope);
            if (float.IsInfinity(a.Slope) && float.IsInfinity(b.Slope))
                slopeDiff = 0;

            if (!(slopeDiff <= tolerance)) return IntersectionType.Does | IntersectionType.Once;

            if (Math.Abs(a.R / a.Q - b.R / b.Q) < tolerance)
                return IntersectionType.Does | IntersectionType.Continuous;

            return IntersectionType.Not;
        }

        /// <summary>
        /// Method to get the intersection-point of two formula's
        /// </summary>
        /// <param name="form1">Formula 1</param>
        /// <param name="form2">Formula 2</param>
        /// <param name="tolerance">The tolerance in difference for checking the two slope's</param>
        /// <returns>
        /// The point at which the two formula's intersect
        /// </returns>
        public static (IntersectionType iType, Vector2? iPoint) Intersect(PqrForm form1, PqrForm form2,
            double tolerance)
        {
            IntersectionType iType = DoesIntersect(form1, form2, tolerance);

            if (iType.HasFlag(IntersectionType.Not))
                return (iType, null);
            if (iType.HasFlag(IntersectionType.Continuous))
                return (iType, new Vector2(float.PositiveInfinity, float.PositiveInfinity));


            // the pqr variables of the first formula
            float p = form1.P;
            float q = form1.Q;
            float r = form1.R;

            // pqr variables of the second formula
            float a = form2.P;
            float b = form2.Q;
            float c = form2.R;
            float x = 0, y = 0;

            if (a != 0 && q != 0 && b == 0)
            {
                x = c / a;
                y = r / q - (p * c) / (q * a);
                return (IntersectionType.Does | IntersectionType.Once, new Vector2(x, y));
            }

            if (b != 0 && p != 0 && a == 0)
            {
                y = c / b;
                x = r / p - (q * c) / (p * b);
                return (IntersectionType.Does | IntersectionType.Once, new Vector2(x, y));
            }

            if ((q != 0 && a != 0 && p == 0) || (p != 0 && b != 0 && q == 0))
            {
                // in this case, it is best to switch the formula's
                return Intersect(form2, form1, tolerance);
            }

            // get the x value
            float xFactor = q / b;
            x = (r - xFactor * c) / (p - xFactor * a);

            // get te y value
            float yFactor = p / a;
            y = (r - yFactor * c) / (q - yFactor * b);

            return (IntersectionType.Does | IntersectionType.Once, new Vector2(x, y));
        }

        /// <summary>
        /// Sets the p, q and r components in such a way the the formula goes through both point a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void WithPoints(Vector2 a, Vector2 b)
        {
            float deltaY = b.y - a.y;
            float deltaX = b.x - a.x;
            float rc = deltaY / deltaX;

            if (float.IsNaN(rc))
            {
                rc = 0;
            }

            if (!float.IsInfinity(rc))
            {
                float r = a.y - rc * a.x;
                P = -rc;
                Q = 1;
                R = r;
                return;
            }

            P = 1;
            Q = 0;
            R = a.x;
        }

        /// <summary>
        /// Sets the p, q and r such that the formula has a slope of `a` and goes through point (0, `b`)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void WithAb(float a, float b)
        {
            Slope = a;
            R = b;
        }

        /// <summary>
        /// Sets the p, q and r such that the formula has a slope of `slope` and goes through point `point`
        /// </summary>
        /// <param name="slope"></param>
        /// <param name="point"></param>
        public void WithSlopePoint(float slope, Vector2 point)
        {
            Slope = slope;
            R = float.IsInfinity(slope) ? point.x : point.y - point.x * slope;
        }

        /// <summary>
        /// Sets this formula perpendicular to `other`
        /// </summary>
        /// <param name="other"></param>
        public void PerpendicularTo(PqrForm other)
        {
            P = other.Q;
            Q = -other.P;
            R = other.R;
        }

        /// <summary>
        /// Sets this formula perpendicular to `other` and that is goes through `throughPoint`
        /// </summary>
        /// <param name="other"></param>
        /// <param name="throughPoint"></param>
        public void PerpendicularTo(PqrForm other, Vector2 throughPoint)
        {
            PerpendicularTo(other);
            WithSlopePoint(Slope, throughPoint);
        }

        public override string ToString()
        {
            return $"{P}x + {Q}y = {R}";
        }

        #region IEquatable Pattern

        public bool Equals(PqrForm other)
        {
            return P.Equals(other.P) && Q.Equals(other.Q) && R.Equals(other.R);
        }

        public override bool Equals(object obj)
        {
            return obj is PqrForm other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(P, Q, R);
        }

        #endregion
    }
}