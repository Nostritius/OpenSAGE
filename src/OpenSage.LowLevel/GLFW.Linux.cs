using System;
using System.Runtime.InteropServices;

namespace OpenSage.LowLevel
{
    public class GLFW
    {
        [DllImport("glfw", EntryPoint = "glfwInit")] 
        private static extern bool _glfwInit();
        
        [DllImport("glfw", EntryPoint = "glfwCreateWindow")] 
        private static extern IntPtr _glfwCreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share);
        
        [DllImport("glfw", EntryPoint = "glfwMakeContextCurrent")] 
        private static extern void _glfwMakeContextCurrent(IntPtr window);
        
        [DllImport("glfw", EntryPoint = "glfwSwapBuffers")] 
        private static extern void _glfwSwapBuffers(IntPtr window);
        
        [DllImport("glfw", EntryPoint = "glfwGetProcAddess")] 
        private static extern IntPtr _glfwGetProcAddress(string procname);
        
        [DllImport("glfw", EntryPoint = "glfwPollEvents")] 
        private static extern void _glfwPollEvents(IntPtr window);
        
        [DllImport("glfw", EntryPoint = "glfwWindowShouldClose")] 
        private static extern int _glfwWindowShouldClose(IntPtr window);

        public static bool glfwInit()
        {
            return _glfwInit();
        }

        public static IntPtr glfwCreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share)
        {
            return _glfwCreateWindow(width, height, title, monitor, share);
        }

        public static void glfwPollEvents(IntPtr window)
        {
            _glfwPollEvents(window);
        }

        public static int glfwWindowShouldClose(IntPtr window)
        {
            return _glfwWindowShouldClose(window);
        }

        public static void glfwMakeContextCurrent(IntPtr window)
        {
            _glfwMakeContextCurrent(window);
        }

        public static void glfwSwapBuffers(IntPtr window)
        {
            _glfwSwapBuffers(window);
        }
    }
}
