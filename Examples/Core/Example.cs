using System;

namespace SharpDXExamples.Examples.Core {
    public abstract class Example {
        protected const int DefaultWindowWidth = 1280;
        protected const int DefaultWindowHeight = 720;

        public void Run() {
            if(!Initialize())
                throw new InvalidOperationException();
            Show();
            Shutdown();
        }

        protected abstract bool Initialize();
        protected abstract void Show();
        protected abstract void Shutdown();
    }
}