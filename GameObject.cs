using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using Assimp;

namespace Game {
    class GameObject {

        ArrayObject VAO;
        BufferObject VBO;
        BufferObject normals_VBO;
        BufferObject texture_VBO;
        BufferObject EBO;

        int vertex_count;
        Texture texture;

        Vector3 position;

        Vector3[] vertices;
        Vector3[] normals;
        uint[] indices;
        List<Vector2> texture_coords;

        Vector3 rotation = Vector3.Zero;

        public GameObject(Texture texture, 
                          Vector3 position, 
                          float dx, 
                          float dy,
                          float dz,
                          bool stretching = true)
        { // Параллелепипед
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
                    new Vector2(0f, dy),
                    new Vector2(dx, dy),
                    new Vector2(dx, 0f),
                    new Vector2(0f, 0f),

                    new Vector2(0f, dy),
                    new Vector2(dz, dy),
                    new Vector2(dz, 0f),
                    new Vector2(0f, 0f),

                    new Vector2(0f, dy),
                    new Vector2(dx, dy),
                    new Vector2(dx, 0f),
                    new Vector2(0f, 0f),

                    new Vector2(0f, dy),
                    new Vector2(dz, dy),
                    new Vector2(dz, 0f),
                    new Vector2(0f, 0f),

                    new Vector2(0f, dz),
                    new Vector2(dx, dz),
                    new Vector2(dx, 0f),
                    new Vector2(0f, 0f),

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
            normals_VBO = new BufferObject(BufferType.ArrayBuffer);
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


        public GameObject(string model_path, Texture texture, Vector3 position, float size) {
            this.position = position;
            this.texture = texture;

            var importer = new AssimpContext();
            Scene scene = importer.ImportFile(
                model_path,
                PostProcessSteps.Triangulate |
                PostProcessSteps.GenerateSmoothNormals
            );

            List<float> vertex_data = new List<float>();
            List<uint> index_data = new List<uint>();
            List<Vector3> vertex_list = new List<Vector3>();
            List<Vector3> normal_list = new List<Vector3>();

            texture_coords = new List<Vector2>();

            uint index_offset = 0;

            float scaleFactor = size;

            foreach (var mesh in scene.Meshes) {
                for (int i = 0; i < mesh.Vertices.Count; i++) {
                    var vertex = mesh.Vertices[i];
                    var normal = mesh.HasNormals ? mesh.Normals[i] : new Assimp.Vector3D(0, 1, 0);

                    // Масштабирование координат
                    vertex.X *= scaleFactor;
                    vertex.Y *= scaleFactor;
                    vertex.Z *= scaleFactor;

                    var v = new Vector3(vertex.X, vertex.Y, vertex.Z);
                    var n = new Vector3(normal.X, normal.Y, normal.Z);
                    vertex_list.Add(v);
                    normal_list.Add(n);


                    vertex_data.Add(v.X);
                    vertex_data.Add(v.Y);
                    vertex_data.Add(v.Z);
                    vertex_data.Add(n.X);
                    vertex_data.Add(n.Y);
                    vertex_data.Add(n.Z);

                    // Текстурные координаты
                    if (mesh.HasTextureCoords(0)) {
                        var uv = mesh.TextureCoordinateChannels[0][i];
                        texture_coords.Add(new Vector2(uv.X, uv.Y));
                    }
                    else {
                        texture_coords.Add(Vector2.Zero);
                    }
                }

                foreach (var face in mesh.Faces) {
                    foreach (var index in face.Indices) {
                        index_data.Add(index_offset + (uint)index);
                    }
                }

                index_offset += (uint)mesh.Vertices.Count;
            }

            vertices = vertex_list.ToArray();
            normals = normal_list.ToArray();
            indices = index_data.ToArray();
            vertex_count = indices.Length;

            float[] vertexArray = vertex_data.ToArray();

            VBO = new BufferObject(BufferType.ArrayBuffer);
            normals_VBO = new BufferObject(BufferType.ArrayBuffer);
            texture_VBO = new BufferObject(BufferType.ArrayBuffer);
            EBO = new BufferObject(BufferType.ElementBuffer);
            VAO = new ArrayObject();

            VAO.Bind();

            VBO.setData(vertexArray, BufferHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            if (texture_coords.Count > 0) {
                float[] texCoordsArray = texture_coords
                    .SelectMany(tc => new float[] { tc.X, tc.Y })
                    .ToArray();

                normals_VBO.setData(normals, BufferHint.StaticDraw);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(2);
            }

            EBO.setData(indices, BufferHint.StaticDraw);

            VAO.unBind();
        }

        public void setPosition(Vector3 newPosition) {
            position = newPosition;
        }

        public void setRotation(Vector3 newRotation) {
            rotation = newRotation;
        }

        public Vector3 getPosition() { 
            return position;
        }

        public void render(Shader shader) {
            // Создание матрицы трансляции
            Matrix4 model =
                Matrix4.CreateRotationX(rotation.X) *
                Matrix4.CreateRotationY(rotation.Y) *
                Matrix4.CreateRotationZ(rotation.Z) *
                Matrix4.CreateTranslation(position);

            int model_location = GL.GetUniformLocation(shader.getHandle(), "model");
            GL.UniformMatrix4(model_location, true, ref model);

            VAO.Bind();

            texture.Bind();
            GL.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, vertex_count, DrawElementsType.UnsignedInt, 0);
            texture.unBind();

            VAO.unBind();
        }

        public void delete() {
            VAO.delete();
            VBO.delete();
            texture_VBO.delete();
            normals_VBO.delete();
            EBO.delete();

            texture.delete();
        }
    }
}
