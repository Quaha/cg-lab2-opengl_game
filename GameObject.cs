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

namespace Game {
    class GameObject {

        private ArrayObject VAO;
        private BufferObject VBO;
        BufferObject normals_VBO;
        private BufferObject texture_VBO;
        private BufferObject EBO;

        private int vertex_count;
        private Texture texture;

        private Vector3 position;

        Vector3[] vertices;
        Vector3[] normals;
        uint[] indices;
        List<Vector2> texture_coords;

        public GameObject(Texture texture, 
                          Vector3 position, 
                          float dx, 
                          float dy,
                          float dz,
                          bool stretching = true)
        { // Parallelepiped
            this.texture = texture;
            this.position = position;

            // Вершины параллелепипеда
            vertices = new Vector3[] {
                new Vector3(-dx / 2,  dy / 2,  dz / 2),
                new Vector3( dx / 2,  dy / 2,  dz / 2),
                new Vector3( dx / 2, -dy / 2,  dz / 2),
                new Vector3(-dx / 2, -dy / 2,  dz / 2),

                new Vector3( dx / 2,  dy / 2,  dz / 2),
                new Vector3( dx / 2,  dy / 2, -dz / 2),
                new Vector3( dx / 2, -dy / 2, -dz / 2),
                new Vector3( dx / 2, -dy / 2,  dz / 2),

                new Vector3( dx / 2,  dy / 2, -dz / 2),
                new Vector3(-dx / 2,  dy / 2, -dz / 2),
                new Vector3(-dx / 2, -dy / 2, -dz / 2),
                new Vector3( dx / 2, -dy / 2, -dz / 2),

                new Vector3(-dx / 2,  dy / 2, -dz / 2),
                new Vector3(-dx / 2,  dy / 2,  dz / 2),
                new Vector3(-dx / 2, -dy / 2,  dz / 2),
                new Vector3(-dx / 2, -dy / 2, -dz / 2),

                new Vector3(-dx / 2,  dy / 2, -dz / 2),
                new Vector3( dx / 2,  dy / 2, -dz / 2),
                new Vector3( dx / 2,  dy / 2,  dz / 2),
                new Vector3(-dx / 2,  dy / 2,  dz / 2),

                new Vector3(-dx / 2, -dy / 2, -dz / 2),
                new Vector3( dx / 2, -dy / 2, -dz / 2),
                new Vector3( dx / 2, -dy / 2,  dz / 2),
                new Vector3(-dx / 2, -dy / 2,  dz / 2)
            };

            normals = new Vector3[] {
                // front
                Vector3.UnitZ, Vector3.UnitZ, Vector3.UnitZ, Vector3.UnitZ,
                // right
                Vector3.UnitX, Vector3.UnitX, Vector3.UnitX, Vector3.UnitX,
                // back
                -Vector3.UnitZ, -Vector3.UnitZ, -Vector3.UnitZ, -Vector3.UnitZ,
                // left
                -Vector3.UnitX, -Vector3.UnitX, -Vector3.UnitX, -Vector3.UnitX,
                // top
                Vector3.UnitY, Vector3.UnitY, Vector3.UnitY, Vector3.UnitY,
                // bottom
                -Vector3.UnitY, -Vector3.UnitY, -Vector3.UnitY, -Vector3.UnitY
            };

            // Индексы
            indices = new uint[] {
                0,  1,  2,  2,  3,  0,
                4,  5,  6,  6,  7,  4,
                8,  9, 10, 10, 11,  8,
               12, 13, 14, 14, 15, 12,
               16, 17, 18, 18, 19, 16,
               20, 21, 22, 22, 23, 20,
            };

            if (stretching) {
                texture_coords = new List<Vector2>() {
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
            }
            else {
                texture_coords = new List<Vector2>() {
                    // front (dx, dy)
                    new Vector2(0f, dy),
                    new Vector2(dx, dy),
                    new Vector2(dx, 0f),
                    new Vector2(0f, 0f),

                    // right (dz, dy)
                    new Vector2(0f, dy),
                    new Vector2(dz, dy),
                    new Vector2(dz, 0f),
                    new Vector2(0f, 0f),

                    // back (dx, dy)
                    new Vector2(0f, dy),
                    new Vector2(dx, dy),
                    new Vector2(dx, 0f),
                    new Vector2(0f, 0f),

                    // left (dz, dy)
                    new Vector2(0f, dy),
                    new Vector2(dz, dy),
                    new Vector2(dz, 0f),
                    new Vector2(0f, 0f),

                    // top (dx, dz)
                    new Vector2(0f, dz),
                    new Vector2(dx, dz),
                    new Vector2(dx, 0f),
                    new Vector2(0f, 0f),

                    // bottom (dx, dz)
                    new Vector2(0f, dz),
                    new Vector2(dx, dz),
                    new Vector2(dx, 0f),
                    new Vector2(0f, 0f),
                };

            }

            vertex_count = indices.Length;

            // Буферы
            VBO = new BufferObject(BufferType.ArrayBuffer);
            normals_VBO = new BufferObject(BufferType.ArrayBuffer);
            texture_VBO = new BufferObject(BufferType.ArrayBuffer);
            EBO = new BufferObject(BufferType.ElementBuffer);
            VAO = new ArrayObject();

            VAO.Bind();

            VBO.setData(vertices, BufferHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            texture_VBO.setData(texture_coords.ToArray(), BufferHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            normals_VBO.setData(normals, BufferHint.StaticDraw);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);

            EBO.setData(indices, BufferHint.StaticDraw);

            VAO.unBind();
        }


        public GameObject(string modelPath, Texture texture, Vector3 position, float size) {
            this.position = position;
            this.texture = texture;

            var importer = new AssimpContext();
            Scene scene = importer.ImportFile(modelPath,
                PostProcessSteps.Triangulate |
                PostProcessSteps.GenerateSmoothNormals |
                PostProcessSteps.FlipUVs); // FlipUVs — на случай, если текстура инвертирована

            List<float> vertexData = new List<float>();
            List<uint> indexData = new List<uint>();
            List<Vector3> vertexList = new List<Vector3>();
            List<Vector3> normalList = new List<Vector3>();
            texture_coords = new List<Vector2>();
            uint indexOffset = 0;

            float scaleFactor = size;

            foreach (var mesh in scene.Meshes) {
                for (int i = 0; i < mesh.Vertices.Count; i++) {
                    var vertex = mesh.Vertices[i];
                    var normal = mesh.HasNormals ? mesh.Normals[i] : new Assimp.Vector3D(0, 1, 0); // fallback

                    // Масштабируем координаты
                    vertex.X *= scaleFactor;
                    vertex.Y *= scaleFactor;
                    vertex.Z *= scaleFactor;

                    // Сохраняем вершину и нормаль для поля
                    var v = new Vector3(vertex.X, vertex.Y, vertex.Z);
                    var n = new Vector3(normal.X, normal.Y, normal.Z);
                    vertexList.Add(v);
                    normalList.Add(n);

                    // Добавляем в общий массив для OpenGL
                    vertexData.Add(v.X);
                    vertexData.Add(v.Y);
                    vertexData.Add(v.Z);
                    vertexData.Add(n.X);
                    vertexData.Add(n.Y);
                    vertexData.Add(n.Z);

                    // Текстурные координаты (если есть)
                    if (mesh.HasTextureCoords(0)) {
                        var uv = mesh.TextureCoordinateChannels[0][i];
                        texture_coords.Add(new Vector2(uv.X, uv.Y));
                    }
                    else {
                        texture_coords.Add(Vector2.Zero); // placeholder
                    }
                }

                foreach (var face in mesh.Faces) {
                    foreach (var index in face.Indices) {
                        indexData.Add(indexOffset + (uint)index);
                    }
                }

                indexOffset += (uint)mesh.Vertices.Count;
            }

            // Сохраняем поля
            vertices = vertexList.ToArray();
            normals = normalList.ToArray();
            indices = indexData.ToArray();
            vertex_count = indices.Length;

            float[] vertexArray = vertexData.ToArray();

            // Создание буферов
            VBO = new BufferObject(BufferType.ArrayBuffer);
            normals_VBO = new BufferObject(BufferType.ArrayBuffer);
            texture_VBO = new BufferObject(BufferType.ArrayBuffer);
            EBO = new BufferObject(BufferType.ElementBuffer);
            VAO = new ArrayObject();

            VAO.Bind();

            // Загружаем позицию и нормали (в одном VBO, 6 float на вершину)
            VBO.setData(vertexArray, BufferHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Загружаем текстурные координаты
            if (texture_coords.Count > 0) {
                float[] texCoordsArray = texture_coords
                    .SelectMany(tc => new float[] { tc.X, tc.Y })
                    .ToArray();

                normals_VBO.setData(normals, BufferHint.StaticDraw);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(2);
            }

            // Загружаем индексы
            EBO.setData(indices, BufferHint.StaticDraw);

            VAO.unBind();
        }

        // Метод для сдвига объекта
        public void setPosition(Vector3 newPosition) {
            position = newPosition;
        }

        public Vector3 getPosition() { 
            return position;
        }

        public void render(Shader shader) {
            // Создаём матрицу трансляции
            Matrix4 model = Matrix4.CreateTranslation(position);

            // Передаем матрицу в шейдер
            int modelLocation = GL.GetUniformLocation(shader.shader_handle, "model");
            GL.UniformMatrix4(modelLocation, true, ref model);

            VAO.Bind();

            texture.Bind();
            GL.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, vertex_count, DrawElementsType.UnsignedInt, 0);
            texture.unBind();

            VAO.unBind();
        }

        public void delete() {
            VAO.delete();
            VBO.delete();
            EBO.delete();

            texture_VBO.delete();
            texture.delete();
        }
    }
}
