using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using IRenderer = AntUI.Renderers.IRenderer;

namespace AntUI.Views
{
    public class SimulationCanvas : UserControl
    {
        private ISet<IRenderer> _renderers;
        
        public SimulationCanvas()
        {
            InitializeComponent();
            _renderers = new HashSet<IRenderer>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            foreach (IRenderer renderer in _renderers)
            {
                renderer.Draw(context);
            }
        }
    }
}