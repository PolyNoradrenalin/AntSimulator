using Avalonia.Media;

namespace AntUI.Renderers
{
    /// <summary>
    /// Defines a renderer that can draw on a SimulationCanvas.
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Method called by the canvas to draw what the renderer is rendering.
        /// </summary>
        void Draw(DrawingContext context);
    }
}