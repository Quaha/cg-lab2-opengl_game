using OpenTK.Mathematics;

namespace Game {

    public class Light {

        Vector3 position;
        Vector3 colour;
        float power;

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