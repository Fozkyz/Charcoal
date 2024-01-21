using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charcoal.Application
{
    public class Screen
    {
        public static event Action<int, int> ScreenSizeChanged = null;

        public static Rectangle ScreenRect { get; internal set; }

        public static float AspectRatio => Width / (float)Height;

        public static bool Fullscreen
        {
            get { return Application.GraphicsDeviceManager.IsFullScreen; }
            set
            {
                if (Application.GraphicsDeviceManager.IsFullScreen != value)
                    ToggleFullscreen();
            }
        }

        public static int Width => ScreenRect.Width;
        public static int Height => ScreenRect.Height;

        public static bool LockCursor { get; set; }

        public static bool ShowCursor
        {
            get => Application.Engine.IsMouseVisible;
            set => Application.Engine.IsMouseVisible = value;
        }

        public static void Setup(int width, int height, bool? lockCursor, bool? showCursor)
        {
            ScreenRect = new Rectangle(0, 0, width, height);

            if (lockCursor.HasValue)
            {
                LockCursor = lockCursor.Value;
            }

            if (showCursor.HasValue)
            {
                ShowCursor = showCursor.Value;
            }

            ScreenSizeChanged?.Invoke(width, height);
        }

        public static void SetBestResolution(bool fullscreen)
        {
            var graphics = Application.GraphicsDevice;
            var modes = graphics.Adapter.SupportedDisplayModes;
            var width = 800;
            var height = 480;

            foreach (var mode in modes)
            {
                width = mode.Width > width ? mode.Width : width;
                height = mode.Height > height ? mode.Height : height;
            }

            SetResolution(width, height, fullscreen);
        }

        public static void SetResolution(int width, int height, bool fullscreen)
        {
            var graphicsManager = Application.GraphicsDeviceManager;
            graphicsManager.PreferredBackBufferWidth = width;
            graphicsManager.PreferredBackBufferHeight = height;
            graphicsManager.ApplyChanges();

            Setup(width, height, null, null);

            graphicsManager.IsFullScreen = fullscreen;
        }

        public static void ToggleFullscreen()
        {
            Application.GraphicsDeviceManager.ToggleFullScreen();
        }
    }
}
