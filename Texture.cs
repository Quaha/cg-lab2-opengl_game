using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Game {
    public class Texture {

        public int texture_ID;

        public Texture(string path) {
            texture_ID = GL.GenTexture();

            // Привязка текстуры
            GL.ActiveTexture(TextureUnit.Texture0);
            Bind();

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

            // Загрузка изображения с использованием StbImageSharp
            StbImage.stbi_set_flip_vertically_on_load(1); // Флаг отражения по вертикали
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

            // Загрузка изображения в текстуру
            GL.TexImage2D(TextureTarget.Texture2D,
                          0,
                          PixelInternalFormat.Rgba,
                          image.Width,
                          image.Height,
                          0,
                          PixelFormat.Rgba,
                          PixelType.UnsignedByte,
                          image.Data);

            unBind();
        }

        public void Bind() {
            GL.BindTexture(TextureTarget.Texture2D, texture_ID);
        }

        public void unBind() {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void delete() {
            GL.DeleteTexture(texture_ID);
        }
    }
}
