using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Learning.Actor
{
    class Actor : ModelRend, Targetable
    {
        public static List<Model> models = new List<Model>();
        public static void LoadContent(ContentManager Content)
        {
            models.Add(Content.Load<Model>("Models\\ship"));
        }
        public Actor(int model)
        {
            this.model = models[model];
        }
        public bool isFriend(Targetable other) { return true; }
        public bool isFoe(Targetable other) { return false; }
    }
}
