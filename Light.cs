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

    public class Light {

        private Vector3 position;
        private Vector3 colour;
        private float power;

        public Light(Vector3 position, Vector3 colour, float power) {
            this.position = position;
            this.colour = colour;
            this.power = power;
        }

        public Vector3 getPosition() { return position; }
        public Vector3 getColour() { return colour; }
        public void setPosition(Vector3 position) { this.position = position; }
        public void setColour(Vector3 colour) {  this.colour = colour; }
        public void setPower(float power) {  this.power = power; }
        public float getPower() { return power; }
    }
}