using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game {
    class Game : GameWindow {

        int width, height;

        int VAO;
        int VBO;

        Shader shader_program;

        float[] vertices = {
               0f,  0.5f, 0f, //top vertex
            -0.5f, -0.5f, 0f, //bottom left vertex
             0.5f, -0.5f, 0f //bottom right vertex
        };


        public Game(int width, int height) : base (GameWindowSettings.Default,
                                                   NativeWindowSettings.Default) {
            this.CenterWindow(new Vector2i(width, height));
            this.height = height;
            this.width = width;

            shader_program = new Shader();
        }

        // Инициализация ресурсов OpenGL
        protected override void OnLoad() {
            base.OnLoad();

            // Создание и привязка VAO
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // Создание и привязка VBO
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Указание атрибутов вершин
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            shader_program.loadShader();
        }

        // Очистка ресурсов OpenGL
        protected override void OnUnload() {
            GL.DeleteBuffer(VAO);
            GL.DeleteBuffer(VBO);

            shader_program.deleteShader();
        }

        // Рендер изображения
        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.ClearColor(0.3f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader_program.useShader();

            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        // Отображение изображения
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
