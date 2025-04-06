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
    class Shader {

        public int shader_handle;

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
        private void checkProgramLink() {
            GL.GetProgram(shader_handle, GetProgramParameterName.LinkStatus, out int linkStatus);
            if (linkStatus == 0) {
                string info = GL.GetProgramInfoLog(shader_handle);
                Console.WriteLine($"Error linking program: {info}");

                throw new Exception("Program linking failed");
            }
        }

        // Проверка корректности компиляции
        private void checkShaderCompile(int shader) {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int compileStatus);
            if (compileStatus == 0) {
                string infoLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine($"Error compiling shader: {infoLog}");

                throw new Exception("Shader compilation failed");
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

        public void SetTransformationMatrices(Matrix4 model, Camera camera) {
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            int modelLocation = GL.GetUniformLocation(shader_handle, "model");
            int viewLocation = GL.GetUniformLocation(shader_handle, "view");
            int projectionLocation = GL.GetUniformLocation(shader_handle, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);
        }

        // Активация шейдерной программы
        public void useShader() {
            GL.UseProgram(shader_handle);
        }

        // Удаление шейдерной программы
        public void deleteShader() {
            GL.DeleteProgram(shader_handle);
        }
    }
}
