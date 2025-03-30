using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    class Camera {

        private float SPEED = 8f;

        private int SCREENWIDTH;
        private int SCREENHEIGHT;

        private float SENSITIVITY = 100f;

        public Vector3 position;

        Vector3 up = Vector3.UnitY;
        Vector3 front = -Vector3.UnitZ;
        Vector3 right = Vector3.UnitX;


        public Camera() {
        
        }

        public Camera(int width, int height, Vector3 position) {
            SCREENWIDTH = width;
            SCREENHEIGHT = height;
            this.position = position;
        }


        public Matrix4 GetViewMatrix() {
            return Matrix4.LookAt(position, position + front, up);
        }
        public Matrix4 GetProjection() {
            return Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(60f),
                SCREENWIDTH / SCREENHEIGHT,
                0.1f,
                100f
            );
        }

        public void InputController(KeyboardState input,
                                    MouseState mouse,
                                    FrameEventArgs e) {
            if (input.IsKeyDown(Keys.W)) {
                position += front * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.A)) {
                position -= right * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.S)) {
                position -= front * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.D)) {
                position += right * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.LeftShift)) {
                position -= up * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.Space)) {
                position += up * SPEED * (float)e.Time;
            }
        }
        public void Update(KeyboardState input,
                           MouseState mouse,
                           FrameEventArgs e) {
            InputController(input, mouse, e);
        }
    }
}
