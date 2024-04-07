using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charcoal.Application
{
	public class Engine : Game
	{
		protected GraphicsDeviceManager _graphicsDeviceManager;

		protected bool _initialized;

		private bool _autoDetectResolution;
		private bool _requestFullscreen;

		private int _totalFrames;
		private float _elapsedTime;
		private int _fps = 0;

		public Engine(string title = "Charcoal Game", int width = 0, int height = 0, bool fullscreen = false)
			: base()
		{
			_graphicsDeviceManager = new GraphicsDeviceManager(this);
			_graphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;

			_initialized = false;
			_autoDetectResolution = false;
			_requestFullscreen = false;

			Window.Title = title;
			Content.RootDirectory = "Content";

			// initialize renderer

			Application.Content = Content;
			Application.Engine = this;
			Application.GraphicsDevice = GraphicsDevice;
			Application.GraphicsDeviceManager = _graphicsDeviceManager;

			if (width != 0 && height != 0)
			{
				_graphicsDeviceManager.PreferredBackBufferWidth = width;
				_graphicsDeviceManager.PreferredBackBufferHeight = height;
			}

			Screen.Setup(width, height, false, true);
		}

		private void OnResize(object sender, PreparingDeviceSettingsEventArgs e)
		{
			var presentationParameters = e.GraphicsDeviceInformation.PresentationParameters;
			var width = presentationParameters.BackBufferWidth;
			var height = presentationParameters.BackBufferHeight;
			Screen.Setup(width, height, null, null);

			// re-render
		}

		protected override void Initialize()
		{
			if (Application.GraphicsDevice == null)
				Application.GraphicsDevice = GraphicsDevice;

			_graphicsDeviceManager.PreferredBackBufferWidth = Screen.Width;
			_graphicsDeviceManager.PreferredBackBufferHeight = Screen.Height;
			_graphicsDeviceManager.ApplyChanges();

			if (_autoDetectResolution)
				Screen.SetBestResolution(_requestFullscreen);

			// initialize renderer

			// initialize input

			_graphicsDeviceManager.PreparingDeviceSettings += OnResize;
			_initialized = true;

			base.Initialize();
		}

		protected override void Update(GameTime gameTime)
		{
			_elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

			if (_elapsedTime > 1000.0f)
			{
				_fps = _totalFrames;
				_totalFrames = 0;
				_elapsedTime = 0;
			}

			base.Update(gameTime);
			// update scene
		}

		protected override void Draw(GameTime gameTime)
		{
			_totalFrames++;
			GraphicsDevice.Clear(Color.Black);

			// render scene

			base.Draw(gameTime);
		}

		protected override void EndDraw()
		{
			if (Screen.LockCursor)
				Mouse.SetPosition(Screen.Width / 2, Screen.Height / 2);

			base.EndDraw();
		}
	}
}
