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

using Assimp;
using Assimp.Configs;
using Assimp.Unmanaged;


namespace Game {
    class Game : GameWindow {

        protected int width, height;

        protected Camera camera;

        protected List<GameObject> game_objects;

        protected Shader shader_program;
        protected FrameCounter fps_counter;

        static bool vaseLoaded = false;
        static int vaseVAO, vaseVBO;
        static int vaseVertexCount = 0;

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

        protected void initObjects() {
            game_objects = new List<GameObject>();

            string wall = "../../../Textures/wall.png";
            string wood = "../../../Textures/wood.jpg";

            string frame = "../../../Textures/frame.jpg";

            string paint1 = "../../../Textures/painting1.jpg";
            string paint2 = "../../../Textures/painting2.png";
            string paint3 = "../../../Textures/painting3.jpg";
            string paint4 = "../../../Textures/painting4.png";
            string paint5 = "../../../Textures/painting5.jpg";
            string paint6 = "../../../Textures/painting6.jpg";

            string vase_obj = "../../../Objects/vase.obj";
            string vase = "../../../Textures/vase.png";

            // floor
            game_objects.Add(
                new GameObject(
                    new Texture(wood),
                    new Vector3(0.0f, 0.0f, 0.0f),
                    15.0f,
                    0.1f,
                    15.0f,
                    false
                )
            );

            // vases
            {
                game_objects.Add(
                    new GameObject(
                        vase_obj,
                        new Texture(vase),
                        new Vector3(6.5f, 0.0f, 6.5f),
                        0.01f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        vase_obj,
                        new Texture(vase),
                        new Vector3(-6.5f, 0.0f, 6.5f),
                        0.01f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        vase_obj,
                        new Texture(vase),
                        new Vector3(6.5f, 0.0f, -6.5f),
                        0.01f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        vase_obj,
                        new Texture(vase),
                        new Vector3(-6.5f, 0.0f, -6.5f),
                        0.01f
                    )
                );
            }

            // walls
            {
                game_objects.Add(
                new GameObject(
                    new Texture(wall),
                    new Vector3(0.0f, 2.5f, -7.5f),
                    15.0f,
                    5.0f,
                    0.1f
                )
            );

                game_objects.Add(
                    new GameObject(
                        new Texture(wall),
                        new Vector3(0.0f, 2.5f, 7.5f),
                        15.0f,
                        5.0f,
                        0.1f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(wall),
                        new Vector3(7.5f, 2.5f, 0),
                        0.1f,
                        5.0f,
                        15.0f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(wall),
                        new Vector3(-7.5f, 2.5f, 0),
                        0.1f,
                        5.0f,
                        15.0f
                    )
                );
            }

            // paintings
            {
                game_objects.Add(
                    new GameObject(
                        new Texture(paint1),
                        new Vector3(-7.4f, 2.5f, 0),
                        0.1f,
                        2.5f,
                        5.0f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(frame),
                        new Vector3(-7.401f, 2.5f, 0),
                        0.1f,
                        2.6f,
                        5.1f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(paint2),
                        new Vector3(7.4f, 2.5f, 0),
                        0.1f,
                        2.5f,
                        5.0f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(frame),
                        new Vector3(7.401f, 2.5f, 0),
                        0.1f,
                        2.6f,
                        5.1f
                    )
                );


                game_objects.Add(
                    new GameObject(
                        new Texture(paint3),
                        new Vector3(-4.0f, 2.5f, 7.4f),
                        3.0f,
                        3.0f,
                        0.1f,
                        false
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(frame),
                        new Vector3(-4.0f, 2.5f, 7.401f),
                        3.1f,
                        3.1f,
                        0.1f,
                        false
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(paint6),
                        new Vector3(3.0f, 2.5f, 7.4f),
                        3.0f,
                        4.0f,
                        0.1f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(frame),
                        new Vector3(3.0f, 2.5f, 7.401f),
                        3.1f,
                        4.1f,
                        0.1f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(paint4),
                        new Vector3(4.0f, 2.5f, -7.4f),
                        5.0f,
                        3.0f,
                        0.1f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(frame),
                        new Vector3(4.0f, 2.5f, -7.401f),
                        5.1f,
                        3.1f,
                        0.1f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(paint5),
                        new Vector3(-4.0f, 2.5f, -7.4f),
                        5.0f,
                        3.0f,
                        0.1f
                    )
                );

                game_objects.Add(
                    new GameObject(
                        new Texture(frame),
                        new Vector3(-4.0f, 2.5f, -7.401f),
                        5.1f,
                        3.1f,
                        0.1f
                    )
                );
            }
        }

        protected override void OnLoad() {
            base.OnLoad();

            CursorState = CursorState.Grabbed;

            initObjects();

            shader_program.loadShader();

            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUnload() {
            
            for (int i = 0; i < game_objects.Count;i++) {
                game_objects[i].delete();
            }

            shader_program.delete();
        }

        protected override void OnRenderFrame(FrameEventArgs args) {

            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;
            base.OnUpdateFrame(args);

            // Обновление камеры
            camera.update(input, mouse, args);

            // Обновление FPS
            fps_counter.updateCounter((float)args.Time);
            if (fps_counter.canGetFPS()) {
                Console.WriteLine($"FPS: {fps_counter.getFPS()}");
            }

            // Очищаем экран
            GL.ClearColor(0.3f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Используем шейдер
            shader_program.useShader();

            // Подготовка трансформации: только модель
            Matrix4 model = camera.GetModelMatrix(new Vector3(0f, 0f, -2f)) * Matrix4.CreateScale(0.1f); // Применяем трансляцию объекта

            // Передаем матрицы в шейдер
            shader_program.setTransformationMatrices(model, camera);

            for (int i = 0; i < game_objects.Count; i++) {
                game_objects[i].render(shader_program);
            }

            // Меняем буферы
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

            if (camera != null) {
                camera.UpdateProjection(width, height);
            }
        }
    }
}
