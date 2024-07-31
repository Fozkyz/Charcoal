using Charcoal.Application;
using Charcoal.Graphics.PostProcessing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charcoal.Graphics
{
	public abstract class BaseRenderer : IDisposable
	{
		protected internal GraphicsDevice _graphicsDevice;
		protected internal SpriteBatch _spriteBatch;
		protected internal RenderTarget2D _renderTarget;
		
		protected bool _disposed = false;

		public bool Dirty { get; set; } = true;

		public BaseRenderer(GraphicsDevice graphicsDevice)
		{
			_graphicsDevice = graphicsDevice;
		}

		public virtual void Initialize(ContentManager contentManager)
		{
			_spriteBatch = new SpriteBatch(_graphicsDevice);
			// Create and load UI manager
			// Add ambient light
		}

		public abstract RenderTarget2D GetDepthBuffer();

		public abstract RenderTarget2D GetNormalBuffer();

		protected RenderTarget2D CreateRenderTarget(
			bool mipMap = false, 
			SurfaceFormat surfaceFormat = SurfaceFormat.Color, 
			DepthFormat depthFormat = DepthFormat.Depth24, 
			int preferredMultiSampleCount = -1, 
			RenderTargetUsage usage = RenderTargetUsage.DiscardContents)
		{
			var width = _graphicsDevice.PresentationParameters.BackBufferWidth;
			var height = _graphicsDevice.PresentationParameters.BackBufferHeight;

			if (preferredMultiSampleCount == -1)
			{
				preferredMultiSampleCount = _graphicsDevice.PresentationParameters.MultiSampleCount;
			}

			return new RenderTarget2D(_graphicsDevice, width, height, mipMap, surfaceFormat, depthFormat, preferredMultiSampleCount, usage);
		}

		protected virtual void RebuildRenderTargets()
		{
			if (!Dirty)
				return;

			var presentationParameters = _graphicsDevice.PresentationParameters;
			var surfaceFormat = presentationParameters.BackBufferFormat;
			_renderTarget = new RenderTarget2D(_graphicsDevice, presentationParameters.BackBufferWidth, presentationParameters.BackBufferHeight, false, surfaceFormat, presentationParameters.DepthStencilFormat, presentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);

			Dirty = false;
		}

		protected virtual void RenderToBackBuffer()
		{
			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
			_spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
			_spriteBatch.End();
		}

		protected void RenderPostProcess(List<PostProcessPass> passes, RenderTarget2D renderTarget)
		{
			if (passes.Count == 0) 
				return;

			_graphicsDevice.SetRenderTarget(renderTarget);

			// Apply post process passes
		}

		public abstract void Render(Scene scene);

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public abstract void Dispose(bool disposing);

		protected void DisposeObject(IDisposable disposable)
		{
			if (disposable != null)
			{
				disposable.Dispose();
				disposable = null;
			}
		}

		protected void DisposeObject(IDisposable[] disposables)
		{
			if (disposables != null)
			{
                for (int i = 0; i < disposables.Length; i++)
                {
					if (disposables[i] == null)
						continue;

					disposables[i].Dispose();
					disposables[i] = null;
                }
            }
		}
	}
}
