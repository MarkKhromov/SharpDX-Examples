﻿using System;
using SharpDX.Windows;
using SharpDXExamples.Examples.Core;

namespace SharpDXExamples.Examples.AlphaMapping {
    public class AlphaMapping : Example {
        public const string Title = "Alpha Mapping";

        RenderForm renderForm;
        Graphics graphics;

        protected override bool Initialize() {
            renderForm = new RenderForm(Title) {
                Width = DefaultWindowWidth,
                Height = DefaultWindowHeight
            };
            graphics = new Graphics();
            if(!graphics.Initialize(DefaultWindowWidth, DefaultWindowHeight, renderForm.Handle))
                return false;

            return true;
        }

        protected override void Show() {
            RenderLoop.Run(renderForm, () => {
                if(!graphics.Frame())
                    throw new InvalidOperationException();
            });
        }

        protected override void Shutdown() {
            graphics.Shutdown();
            renderForm.Dispose();
        }
    }
}