using System;

namespace OpenSage.LowLevel
{
    partial class HostView
    {
        private IntPtr Window;
        
        public HostView()
        {
            GLFW.glfwInit();
            Window = GLFW.glfwCreateWindow(640, 480, "OpenSAGE", IntPtr.Zero, IntPtr.Zero);
        }
        
        private void PlatformSetCursor(HostCursor cursor)
        {
            
        }
    }
}
