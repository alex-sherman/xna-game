using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Learning
{
    class Player
    {
        private Vector3 position = new Vector3(0, 0, 10);
        private Matrix rotation;
        private int xRotation;
        private int yRotation;
        public void Update(GameTime time, int dx, int dy, KeyboardState keyboard){
            this.xRotation -= dx;
            this.yRotation -= dy;
            this.rotation = Matrix.CreateRotationX(this.yRotation * .01f) * Matrix.CreateRotationY(this.xRotation * .01f);
            Vector3 toAdd = Vector3.Zero;
            if (keyboard.IsKeyDown(Keys.W))
            {
                toAdd.Z -= .05f;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                toAdd.Z += .05f;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                toAdd.X += .05f;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                toAdd.X -= .05f;
            }
            if (keyboard.IsKeyDown(Keys.Space))
            {
                this.position += Vector3.Up*.05f;
            }
            else if (keyboard.IsKeyDown(Keys.LeftControl))
            {
                this.position -= Vector3.Up*.05f;
            }
            this.position += Vector3.Transform(toAdd, this.rotation);

        }
        public Matrix getCameraMatrix()
        {
            
            return Matrix.CreateLookAt(this.position,
                   Vector3.Transform(new Vector3(0,0,-1),this.rotation)+this.position, Vector3.Transform(Vector3.Up,this.rotation));


        }
        public Vector3 getCameraPos()
        {
            return new Vector3(this.position.X,this.position.Y,this.position.Z);
        }

    }
}
