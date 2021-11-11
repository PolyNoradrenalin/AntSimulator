﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers
{
    /// <summary>
    /// Represents the frame in which a simulation will occur.
    /// </summary>
    public class SimFrame : UIElement
    {
        private List<IRenderer> renderers;

        public SimFrame()
        {
            renderers = new List<IRenderer>();
        }
        
        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm)
        {
            foreach (IRenderer r in renderers)
            {
                r.Render(spriteBatch, gdm);
            }
        }

        public void AddRenderer(IRenderer r)
        {
            renderers.Add(r);
        }
        
        public void RemoveRenderer(IRenderer r)
        {
            renderers.Remove(r);
        }
    }
}