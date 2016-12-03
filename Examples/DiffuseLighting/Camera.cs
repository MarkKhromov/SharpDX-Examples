using SharpDX;

namespace SharpDXExamples.Examples.DiffuseLighting {
    public class Camera {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Matrix View { get; private set; }

        public void Render() {
            var pitch = Rotation.X * 0.0174532925f;
            var yaw = Rotation.Y * 0.0174532925f;
            var roll = Rotation.Z * 0.0174532925f;

            var rotationMatrix = Matrix.RotationYawPitchRoll(yaw, pitch, roll);

            var lookAt = Vector3.TransformCoordinate(new Vector3(0.0f, 0.0f, 1.0f), rotationMatrix);
            var up = Vector3.TransformCoordinate(Vector3.Up, rotationMatrix);

            lookAt = Position + lookAt;

            View = Matrix.LookAtLH(Position, lookAt, up);
        }
    }
}