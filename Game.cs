using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using StbImageSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Game {
    class Game : GameWindow {

        int width, height;

        Camera camera;

        GameObject cube;

        Shader shader_program;
        FrameCounter fps_counter;

        public Game(int width, int height) : base(
            new GameWindowSettings(),
            new NativeWindowSettings {Title = "Art Gallery"})
        {
            CenterWindow(new Vector2i(width, height));

            this.width = width;
            this.height = height;

            shader_program = new Shader();
            fps_counter = new FrameCounter();
            camera = new Camera(width, height, Vector3.Zero);
        }

        protected override void OnLoad() {
            base.OnLoad();

            CursorState = CursorState.Grabbed;

            cube = new GameObject(new Texture("../../../Textures/block.jpg"));

            shader_program.loadShader();

            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUnload() {

            cube.delete();

            shader_program.deleteShader();
        }

        protected override void OnRenderFrame(FrameEventArgs args) {

            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;
            base.OnUpdateFrame(args);

            camera.update(input, mouse, args);

            fps_counter.updateCounter((float)args.Time);
            if (fps_counter.canGetFPS()) {
                Console.WriteLine($"FPS: {fps_counter.getFPS()}");
            }

            GL.ClearColor(0.3f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader_program.useShader();

            // Подготовка трансформации: только модель
            Matrix4 model = camera.GetModelMatrix(new Vector3(0f, 0f, -2f)); // Применяем трансляцию объекта

            // Передаем матрицы в шейдер
            shader_program.SetTransformationMatrices(model, camera);

            cube.render();

            Context.SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            if (KeyboardState.IsKeyDown(Keys.Escape)) {
                Close();
            }
            base.OnUpdateFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e) {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            this.width = e.Width;
            this.height = e.Height;
        }
    }
}
