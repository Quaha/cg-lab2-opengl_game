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
    class GameObject {

        private ArrayObject VAO;
        private BufferObject VBO;
        private BufferObject texture_VBO;
        private BufferObject EBO;

        private int vertex_count;
        private Texture texture;

        private Vector3 position;

        public GameObject(Texture texture, Vector3 position, float size) { // Cube
            this.texture = texture;
            this.position = position;

            // Вершины куба
            Vector3[] vertices = new Vector3[] {
                new Vector3(-size / 2,  size / 2,  size / 2),
                new Vector3( size / 2,  size / 2,  size / 2),
                new Vector3( size / 2, -size / 2,  size / 2),
                new Vector3(-size / 2, -size / 2,  size / 2),

                new Vector3( size / 2,  size / 2,  size / 2),
                new Vector3( size / 2,  size / 2, -size / 2),
                new Vector3( size / 2, -size / 2, -size / 2),
                new Vector3( size / 2, -size / 2,  size / 2),

                new Vector3( size / 2,  size / 2, -size / 2),
                new Vector3(-size / 2,  size / 2, -size / 2),
                new Vector3(-size / 2, -size / 2, -size / 2),
                new Vector3( size / 2, -size / 2, -size / 2),

                new Vector3(-size / 2,  size / 2, -size / 2),
                new Vector3(-size / 2,  size / 2,  size / 2),
                new Vector3(-size / 2, -size / 2,  size / 2),
                new Vector3(-size / 2, -size / 2, -size / 2),

                new Vector3(-size / 2,  size / 2, -size / 2),
                new Vector3( size / 2,  size / 2, -size / 2),
                new Vector3( size / 2,  size / 2,  size / 2),
                new Vector3(-size / 2,  size / 2,  size / 2),

                new Vector3(-size / 2, -size / 2, -size / 2),
                new Vector3( size / 2, -size / 2, -size / 2),
                new Vector3( size / 2, -size / 2,  size / 2),
                new Vector3(-size / 2, -size / 2,  size / 2)
            };

            // Индексы
            uint[] indices = {
                0,  1,  2,  2,  3,  0,
                4,  5,  6,  6,  7,  4,
                8,  9, 10, 10, 11,  8,
               12, 13, 14, 14, 15, 12,
               16, 17, 18, 18, 19, 16,
               20, 21, 22, 22, 23, 20,
            };

            // Текстурные координаты
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

            vertex_count = indices.Length;

            // Буферы
            VBO = new BufferObject(BufferType.ArrayBuffer);
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

            EBO.setData(indices, BufferHint.StaticDraw);

            VAO.unBind();
        }

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
            Vector3[] vertices = new Vector3[] {
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

            // Индексы
            uint[] indices = {
                0,  1,  2,  2,  3,  0,
                4,  5,  6,  6,  7,  4,
                8,  9, 10, 10, 11,  8,
               12, 13, 14, 14, 15, 12,
               16, 17, 18, 18, 19, 16,
               20, 21, 22, 22, 23, 20,
            };

            // Текстурные координаты
            List<Vector2> texture_coords;

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
            GL.DrawElements(PrimitiveType.Triangles, vertex_count, DrawElementsType.UnsignedInt, 0);
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
