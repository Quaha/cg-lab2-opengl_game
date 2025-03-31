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

using StbImageSharp;

namespace Game {
    class Game : GameWindow {

        int width, height;

        Camera camera;
       
        int VAO;
        int VBO;
        int EBO;

        int texture_ID;
        int texture_VBO;

        Shader shader_program;

        private int frame_сount = 0;  // Счётчик кадров
        private float time_elapsed = 0f;  // Время прошедшее с последнего расчёта FPS
        private float fps = 0f;  // Текущее значение FPS

        List<Vector3> vertices = new List<Vector3>() {
            new Vector3(-0.5f,  0.5f, 0.5f), //top-left-front vertice
            new Vector3( 0.5f,  0.5f, 0.5f), //top-right-front vertice
            new Vector3( 0.5f, -0.5f, 0.5f), //bottom-right-front vertice
            new Vector3(-0.5f, -0.5f, 0.5f), //bottom-left-front vertice

            new Vector3(0.5f, 0.5f, 0.5f), //top-left-back vertice
            new Vector3(0.5f, 0.5f, -0.5f), //top-right-back vertice
            new Vector3(0.5f, -0.5f,  -0.5f), //bottom-right-back vertice
            new Vector3(0.5f, -0.5f,  0.5f), //bottom-left-back vertice

            new Vector3(0.5f,  0.5f, -0.5f), //top-left-back vertice
            new Vector3(-0.5f,  0.5f, -0.5f), //top-right-back vertice
            new Vector3(-0.5f,  -0.5f, -0.5f), //bottom-right-back vertice
            new Vector3(0.5f, -0.5f, -0.5f), //bottom-left-back vertice
        
            new Vector3(-0.5f, 0.5f, -0.5f), //top-right-back vertice
            new Vector3(-0.5f, 0.5f, 0.5f), //top-left-back vertice
            new Vector3(-0.5f, -0.5f,  0.5f), //bottom-left-back vertice
            new Vector3(-0.5f, -0.5f,  -0.5f), //bottom-right-back vertice

            new Vector3(-0.5f, 0.5f, -0.5f), //top-right-back vertice
            new Vector3(0.5f, 0.5f, -0.5f), //top-left-back vertice
            new Vector3(0.5f, 0.5f,  0.5f), //bottom-left-back vertice
            new Vector3(-0.5f, 0.5f,  0.5f), //bottom-right-back vertice

            new Vector3(-0.5f, -0.5f, -0.5f), //top-right-back vertice
            new Vector3(0.5f, -0.5f, -0.5f), //top-left-back vertice
            new Vector3(0.5f, -0.5f,  0.5f), //bottom-left-back vertice
            new Vector3(-0.5f, -0.5f,  0.5f), //bottom-right-back vertice
        };


        uint[] indices = {
            // front
            0, 1, 2, // top triangle
            2, 3, 0, // bottom triangle

            // right
            4, 5, 6,
            6, 7, 4,

            // back
            8, 9, 10,
            10, 11, 8,

            // left
            12, 13, 14,
            14, 15, 12,

            // up
            16, 17, 18,
            18, 19, 16,

            // down
            20, 21, 22,
            22, 23, 20,

        };

        List<Vector2> texture_coords = new List<Vector2>() {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
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

            this.CursorState = CursorState.Grabbed;

            // Создание и привязка VAO
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // Создание и настройка VBO для вершин
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            // Привязываем EBO
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // Создание и настройка VBO для текстурных координат
            texture_VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, texture_VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, texture_coords.Count * Vector3.SizeInBytes, texture_coords.ToArray(), BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(VAO, 1);

            // Отвязываем VAO и VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            // Загрузка текстуры
            texture_ID = GL.GenTexture(); // Создание пустой текстуры
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture_ID);

            // Параметры текстуры
            GL.TexParameter(TextureTarget.Texture2D,
                            TextureParameterName.TextureWrapS,
                            (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D,
                            TextureParameterName.TextureWrapT,
                            (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D,
                            TextureParameterName.TextureMinFilter,
                            (int)TextureMinFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D,
                            TextureParameterName.TextureMagFilter,
                            (int)TextureMagFilter.Nearest);

            // Загрузка изображения
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult boxTexture = ImageResult.FromStream(
                File.OpenRead("../../../Textures/block.jpg"),
                ColorComponents.RedGreenBlueAlpha
            );

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                boxTexture.Width,
                boxTexture.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                boxTexture.Data
            );

            // Отвязка текстуры
            GL.BindTexture(TextureTarget.Texture2D, 0);

            shader_program.loadShader();

            GL.Enable(EnableCap.DepthTest);

            camera = new Camera(width, height, Vector3.Zero);
        }

        // Очистка ресурсов OpenGL
        protected override void OnUnload() {
            GL.DeleteBuffer(VAO);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);

            GL.DeleteTexture(texture_ID);
            GL.DeleteBuffer(texture_VBO);

            shader_program.deleteShader();
        }

        // Рендер изображения
        protected override void OnRenderFrame(FrameEventArgs args) {

            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;
            base.OnUpdateFrame(args);

            camera.Update(input, mouse, args);

            frame_сount++;

            // Обновляем время, прошедшее с последнего кадра
            time_elapsed += (float)args.Time;

            // Когда прошло 1 секунда (или более), рассчитываем FPS
            if (time_elapsed >= 1.0f) {
                fps = frame_сount;  // FPS = количество кадров за секунду
                Console.WriteLine($"FPS: {fps}");  // Выводим FPS в консоль

                // Сбрасываем счётчик и время
                frame_сount = 0;
                time_elapsed = 0f;
            }

            GL.ClearColor(0.3f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader_program.useShader();

            GL.BindTexture(TextureTarget.Texture2D, texture_ID);

            // Трансформация
            Matrix4 model = Matrix4.CreateRotationY(0);
            Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -2f);
            model *= translation;

            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjection();

            int modelLocation = GL.GetUniformLocation(shader_program.shader_handle, "model");
            int viewLocation = GL.GetUniformLocation(shader_program.shader_handle, "view");
            int projectionLocation = GL.GetUniformLocation(shader_program.shader_handle, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
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
