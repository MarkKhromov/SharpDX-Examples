using System;
using SharpDX.DirectInput;

namespace SharpDXExamples.Examples.DirectInput {
    public class Input {
        SharpDX.DirectInput.DirectInput directInput;
        Keyboard keyboard;
        Mouse mouse;
        MouseState mouseState;
        KeyboardState keyboardState;
        int screenWidth;
        int screenHeight;
        int mouseX;
        int mouseY;

        public bool Initialize(IntPtr hwnd, int screenWidth, int screenHeight) {
            try {
                this.screenWidth = screenWidth;
                this.screenHeight = screenHeight;
                mouseX = 0;
                mouseY = 0;
                directInput = new SharpDX.DirectInput.DirectInput();

                keyboard = new Keyboard(directInput);
                keyboard.SetCooperativeLevel(hwnd, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
                keyboard.Acquire();

                mouse = new Mouse(directInput);
                mouse.SetCooperativeLevel(hwnd, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
                mouse.Acquire();
            } catch { return false; }
            return true;
        }

        public void Shutdown() {
            mouse.Dispose();
            keyboard.Dispose();
            directInput.Dispose();
        }

        public bool Frame() {
            if(!ReadKeyboard())
                return false;

            if(!ReadMouse())
                return false;

            ProcessInput();

            return true;
        }

        public bool IsPressed(Key key) {
            return keyboardState.IsPressed(key);
        }

        public void GetMouseLocation(out int mouseX, out int mouseY) {
            mouseX = this.mouseX;
            mouseY = this.mouseY;
        }

        bool ReadKeyboard() {
            try {
                keyboardState = keyboard.GetCurrentState();
            } catch { return false; }
            return true;
        }

        bool ReadMouse() {
            try {
                mouseState = mouse.GetCurrentState();
            } catch { return false; }
            return true;
        }

        void ProcessInput() {
            mouseX += mouseState.X;
            mouseY += mouseState.Y;

            if(mouseX < 0) {
                mouseX = 0;
            }
            if(mouseY < 0) {
                mouseY = 0;
            }

            if(mouseX > screenWidth) {
                mouseX = screenWidth;
            }
            if(mouseY > screenHeight) {
                mouseY = screenHeight;
            }
        }
    }
}