namespace SharpDXExamples.Examples.FrustumCulling {
    public class Position {
        public float RotationY { get; private set; }

        float frameTime;
        float leftTurnSpeed;
        float rightTurnSpeed;

        public void SetFrameTime(float frameTime) {
            this.frameTime = frameTime;
        }

        public void TurnLeft(bool keyDown) {
            if(keyDown) {
                leftTurnSpeed += frameTime * 0.01f;
                if(leftTurnSpeed > frameTime * 0.15f) {
                    leftTurnSpeed = frameTime * 0.15f;
                }
            } else {
                leftTurnSpeed -= frameTime * 0.005f;
                if(leftTurnSpeed < 0.0f) {
                    leftTurnSpeed = 0.0f;
                }
            }

            RotationY -= leftTurnSpeed;
            if(RotationY < 0.0f) {
                RotationY += 360.0f;
            }
        }

        public void TurnRight(bool keyDown) {
            if(keyDown) {
                rightTurnSpeed += frameTime * 0.01f;
                if(rightTurnSpeed > frameTime * 0.15f) {
                    rightTurnSpeed = frameTime * 0.15f;
                }
            } else {
                rightTurnSpeed -= frameTime * 0.005f;
                if(rightTurnSpeed < 0.0f) {
                    rightTurnSpeed = 0.0f;
                }
            }

            RotationY += rightTurnSpeed;
            if(RotationY > 360.0f) {
                RotationY -= 360.0f;
            }
        }
    }
}