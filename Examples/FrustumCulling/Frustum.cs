using SharpDX;

namespace SharpDXExamples.Examples.FrustumCulling {
    public class Frustum {
        Plane[] planes;

        public void ConstructFrustum(float screenDepth, Matrix projection, Matrix view) {
            var zMinimum = -projection.M43 / projection.M33;
            var r = screenDepth / (screenDepth - zMinimum);
            projection.M33 = r;
            projection.M43 = -r * zMinimum;

            var matrix = Matrix.Multiply(view, projection);

            planes = new[] {
                Plane.Normalize(new Plane(
                    matrix.M14 + matrix.M13,
                    matrix.M24 + matrix.M23,
                    matrix.M34 + matrix.M33,
                    matrix.M44 + matrix.M43
                )),
                Plane.Normalize(new Plane(
                    matrix.M14 - matrix.M13,
                    matrix.M24 - matrix.M23,
                    matrix.M34 - matrix.M33,
                    matrix.M44 - matrix.M43
                )),
                Plane.Normalize(new Plane(
                    matrix.M14 + matrix.M11,
                    matrix.M24 + matrix.M21,
                    matrix.M34 + matrix.M31,
                    matrix.M44 + matrix.M41
                )),
                Plane.Normalize(new Plane(
                    matrix.M14 - matrix.M11,
                    matrix.M24 - matrix.M21,
                    matrix.M34 - matrix.M31,
                    matrix.M44 - matrix.M41
                )),
                Plane.Normalize(new Plane(
                    matrix.M14 - matrix.M12,
                    matrix.M24 - matrix.M22,
                    matrix.M34 - matrix.M32,
                    matrix.M44 - matrix.M42
                )),
                Plane.Normalize(new Plane(
                    matrix.M14 + matrix.M12,
                    matrix.M24 + matrix.M22,
                    matrix.M34 + matrix.M32,
                    matrix.M44 + matrix.M42
                ))
            };
        }

        public bool CheckPoint(float x, float y, float z) {
            for(int i = 0; i < 6; i++) {
                if(Plane.DotCoordinate(planes[i], new Vector3(x, y, z)) <= 0.0f) {
                    return false;
                }
            }

            return true;
        }

        public bool CheckCube(float xCenter, float yCenter, float zCenter, float radius) {
            for(int i = 0; i < 6; i++) {
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter - radius, yCenter - radius, zCenter - radius)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter + radius, yCenter - radius, zCenter - radius)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter - radius, yCenter + radius, zCenter - radius)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter + radius, yCenter + radius, zCenter - radius)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter - radius, yCenter - radius, zCenter + radius)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter + radius, yCenter - radius, zCenter + radius)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter - radius, yCenter + radius, zCenter + radius)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter + radius, yCenter + radius, zCenter + radius)) >= 0.0f) {
                    continue;
                }

                return false;
            }

            return true;
        }

        public bool CheckSphere(float xCenter, float yCenter, float zCenter, float radius) {
            for(int i = 0; i < 6; i++) {
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter, yCenter, zCenter)) < -radius) {
                    return false;
                }
            }

            return true;
        }

        public bool CheckRectangle(float xCenter, float yCenter, float zCenter, float xSize, float ySize, float zSize) {
            for(int i = 0; i < 6; i++) {
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter - xSize, yCenter - ySize, zCenter - zSize)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter + xSize, yCenter - ySize, zCenter - zSize)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter - xSize, yCenter + ySize, zCenter - zSize)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter - xSize, yCenter - ySize, zCenter + zSize)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter + xSize, yCenter + ySize, zCenter - zSize)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter + xSize, yCenter - ySize, zCenter + zSize)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter - xSize, yCenter + ySize, zCenter + zSize)) >= 0.0f) {
                    continue;
                }
                if(Plane.DotCoordinate(planes[i], new Vector3(xCenter + xSize, yCenter + ySize, zCenter + zSize)) >= 0.0f) {
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}