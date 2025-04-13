using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Game {
    class Shader {

        int shader_handle;

        // Загрузка, компиляция и линковка шейдеров программы
        public void loadShader() {
            shader_handle = GL.CreateProgram();

            int vertex_shader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertex_shader, loadShaderSource("shader.vert"));
            GL.CompileShader(vertex_shader);
            checkShaderCompile(vertex_shader);

            int fragment_shader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragment_shader, loadShaderSource("shader.frag"));
            GL.CompileShader(fragment_shader);
            checkShaderCompile(fragment_shader);

            // Линковка шейдерной программы
            GL.AttachShader(shader_handle, vertex_shader);
            GL.AttachShader(shader_handle, fragment_shader);

            GL.LinkProgram(shader_handle);

            checkProgramLink();

            GL.DeleteShader(vertex_shader);
            GL.DeleteShader(fragment_shader);
        }

        // Проверка корректности линковки
        public void checkProgramLink() {
            GL.GetProgram(shader_handle, GetProgramParameterName.LinkStatus, out int link_status);
            if (link_status == 0) {
                string info = GL.GetProgramInfoLog(shader_handle);
                Console.WriteLine($"Error linking program: {info}");

                throw new Exception("Program linking failed!");
            }
        }

        // Проверка корректности компиляции
        public void checkShaderCompile(int shader) {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int compile_status);
            if (compile_status == 0) {
                string info = GL.GetShaderInfoLog(shader);
                Console.WriteLine($"Error compiling shader: {info}");

                throw new Exception("Shader compilation failed!");
            }
        }

        // Получение исходного кода шейдера
        public static string loadShaderSource(string filepath) {
            string shader_source = "";
            try {
                using (StreamReader reader = new StreamReader("../../../Shaders/" + filepath)) {
                    shader_source = reader.ReadToEnd();
                }
            }
            catch (Exception e) {
                Console.WriteLine("Failed to load shader source file: " + e.Message);

                throw new Exception("Shader loading failed");
            }
            return shader_source;
        }

        // Передача информации в шейдеры
        public void setShaderData(Matrix4 model, Camera camera, List<Light> lights) {
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            int modelLocation = GL.GetUniformLocation(shader_handle, "model");
            int viewLocation = GL.GetUniformLocation(shader_handle, "view");
            int projectionLocation = GL.GetUniformLocation(shader_handle, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            // Источники света (ограничение до 8, можно повысить в коде шейдеров)
            int count = Math.Min(lights.Count, 8);
            GL.Uniform1(GL.GetUniformLocation(shader_handle, "light_count"), count);

            for (int i = 0; i < count; i++) {
                string posName = $"light_positions[{i}]";
                string colName = $"light_colors[{i}]";
                string powName = $"light_power[{i}]";

                GL.Uniform3(GL.GetUniformLocation(shader_handle, posName), lights[i].getPosition());
                GL.Uniform3(GL.GetUniformLocation(shader_handle, colName), lights[i].getColour());
                GL.Uniform1(GL.GetUniformLocation(shader_handle, powName), lights[i].getPower());
            }
        }

        public void useShader() {
            GL.UseProgram(shader_handle);
        }

        public void delete() {
            GL.DeleteProgram(shader_handle);
        }

        public int getHandle() {
            return shader_handle;
        }
    }
}
