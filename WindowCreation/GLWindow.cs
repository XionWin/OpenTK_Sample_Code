﻿using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using System.Drawing;

namespace WindowCreation;
public class GLWindow : Window
{
    public GLWindow() : base ("Window Creation")
    {}


    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.ClearColor(Color.MidnightBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }
}




