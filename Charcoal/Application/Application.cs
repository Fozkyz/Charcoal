using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charcoal.Application
{
    public class Application
    {
        public static Engine Engine { get; set; }

        public static ContentManager Content { get; set; }

        public static GraphicsDevice GraphicsDevice { get; set; }

        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }

        public static void Quit()
        {
            Engine.Exit();
        }
    }
}
