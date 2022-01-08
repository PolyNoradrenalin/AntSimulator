using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using AntEngine;
using AntEngine.Colliders;
using AntEngine.Entities;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;
using AntEngine.Resources;
using AntEngine.Entities.Pheromones;
using App.Renderers;
using App.Renderers.EntityRenderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = System.Numerics.Vector2;

namespace App.UIElements
{
    /// <summary>
    ///     Represents the frame in which a simulation will occur.
    /// </summary>
    public class SimFrame : UIElement
    {
        public static Texture2D EntityTexture;
        public static Texture2D AntTexture;
        public static Texture2D ColonyTexture;
        public static Texture2D CircleTexture;

        private readonly IList<IRenderer> _renderers;
        private readonly PaintBrushSelection _paintBrushSelection;
        private PaintBrushSelection.PaintBrushState _paintBrushState = PaintBrushSelection.PaintBrushState.Wall;

        Resource _food = new Resource("food", "fruit");
        Vector2 _colonyScale = Vector2.One * 20F;
        private int _colonySpawnCost = 10;
        private int _colonyStockpileQuantity = 50;
        private Vector2 _foodScale = Vector2.One * 10F;
        private int _foodValue = 50;

        public SimFrame(Rectangle rect, World world) : base(rect)
        {
            _renderers = new List<IRenderer>();
            SimWorld = world;
            world.EntityAdded += OnEntityAdded;
            world.EntityRemoved += OnEntityRemoved;
            MouseHeld += OnMouseHeld;
            MouseReleased += (arg1, arg2, arg3) => OnMouseReleased(arg1, arg2, arg3);

            WorldRenderer worldRenderer = new WorldRenderer(world.Collider, EntityTexture);
            _renderers.Add(worldRenderer);
            // TODO: Unsubscribe to allow GC.

            Rectangle paintBrushSelectionRectangle = new Rectangle(rect.Width - 200, rect.Height / 2, 32, 32 * 3);

            PaintBrushSelection paintBrushSelection =
                new PaintBrushSelection(paintBrushSelectionRectangle, PaintBrushSelection.PaintBrushState.Wall);
            _renderers.Add(paintBrushSelection);

            paintBrushSelection.PaintBrushStateChange += OnBrushStateChange;
        }

        private void OnBrushStateChange(PaintBrushSelection.PaintBrushState state)
        {
            _paintBrushState = state;
        }

        private void OnMouseHeld(MouseState mouseState, UIElement arg2, Rectangle canvasOffset)
        {
            (int worldPixelWidth, int worldPixelHeight) = WorldRenderer.WorldPixelSize(canvasOffset, SimWorld.Size);

            (int worldDivX, int worldDivY) = GetWorldDivisionCoords(mouseState, worldPixelWidth, worldPixelHeight);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (0 > worldDivX || worldDivX >= World.WorldColliderDivision || 0 > worldDivY ||
                    worldDivY >= World.WorldColliderDivision)
                    return;

                switch (_paintBrushState)
                {
                    case PaintBrushSelection.PaintBrushState.Wall:
                        SimWorld.Collider.Matrix[worldDivY][worldDivX] = true;
                        SimWorld.ApplyEntityBuffers();
                        break;
                }
            }
        }

        private void OnMouseReleased(MouseState mouseState, UIElement arg2, Rectangle canvasOffset)
        {
            (int worldPixelWidth, int worldPixelHeight) = WorldRenderer.WorldPixelSize(canvasOffset, SimWorld.Size);

            (float simX, float simY) = GetWorldCoords(mouseState, worldPixelWidth, worldPixelHeight);

            if (mouseState.LeftButton == ButtonState.Released)
            {
                if (0 > simX || simX >= SimWorld.Size.X || 0 > simY || simY >= SimWorld.Size.Y)
                    return;

                switch (_paintBrushState)
                {
                    case PaintBrushSelection.PaintBrushState.Colony:
                        Colony colony = new Colony(SimWorld,
                            (name, transform, world, _) => new Ant("Ant", transform, world))
                        {
                            Transform = {Position = new Vector2(simX, simY), Scale = _colonyScale}
                        };
                        colony.SpawnCost.AddResource(_food, _colonySpawnCost);
                        colony.Stockpile.AddResource(_food, _colonyStockpileQuantity);
                        SimWorld.ApplyEntityBuffers();
                        break;
                    case PaintBrushSelection.PaintBrushState.Food:
                        ResourceEntity unused = new ResourceEntity(SimWorld, _foodValue, _food)
                        {
                            Transform = {Position = new Vector2(simX, simY), Scale = _foodScale}
                        };
                        SimWorld.ApplyEntityBuffers();
                        break;
                }
            }
        }

        public World SimWorld { get; }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            base.Render(spriteBatch, gdm, canvasOffset);

            foreach (IRenderer r in _renderers)
            {
                r.Render(spriteBatch, gdm,
                    new Rectangle(Position.X + canvasOffset.Left, Position.Y + canvasOffset.Top, Size.Width,
                        Size.Height));
            }
        }

        public void AddRenderer(IRenderer r)
        {
            _renderers.Add(r);
        }

        public void RemoveRenderer(IRenderer r)
        {
            _renderers.Remove(r);
        }

        private void OnEntityAdded(Entity entity)
        {
            IRenderer renderer = entity switch
            {
                Ant ant => new AntRenderer(ant, AntTexture, Color.Black),
                Colony colony => new ColonyRenderer(colony, ColonyTexture),
                ResourceEntity resource => new EntityRenderer(resource, CircleTexture, Color.Green),
                FoodPheromone foodPheromone => new EntityRenderer(foodPheromone, CircleTexture, Color.Red),
                HomePheromone homePheromone => new EntityRenderer(homePheromone, CircleTexture, Color.Blue),
                _ => new EntityRenderer(entity, EntityTexture, Color.White)
            };

            AddRenderer(renderer);
        }

        private void OnEntityRemoved(Entity entity)
        {
            foreach (IRenderer renderer in _renderers.ToList())
            {
                if (!(renderer is EntityRenderer entityRenderer)) continue;

                if (entityRenderer.Entity.Equals(entity)) RemoveRenderer(renderer);
            }
        }

        /// <summary>
        /// Returns the relative coordinates of the mouse.
        /// Represents the value in pixels inside the SimFrame.
        /// </summary>
        private (float relativeX, float relativeY) GetRelativeCoords(MouseState mouseState)
        {
            (float mouseX, float mouseY) = mouseState.Position.ToVector2();
            return (mouseX - Position.X, Size.Height - (mouseY - Position.Y));
        }

        /// <summary>
        /// Returns the coordinates of the mouse in relation to the World.
        /// </summary>
        private (float X, float Y) GetWorldCoords(MouseState mouseState, int worldPixelWidth, int worldPixelHeight)
        {
            (float relativeX, float relativeY) = GetRelativeCoords(mouseState);

            return (SimWorld.Size.X * (relativeX / worldPixelWidth), SimWorld.Size.Y * (relativeY / worldPixelHeight));
        }

        /// <summary>
        /// Returns index of the mouse's position corresponding to WorldCollider array.
        /// </summary>
        private (int X, int Y) GetWorldDivisionCoords(MouseState mouseState, int worldPixelWidth, int worldPixelHeight)
        {
            (float relativeX, float relativeY) = GetRelativeCoords(mouseState);

            return ((int) MathF.Round(relativeX / worldPixelWidth * SimWorld.Collider.Subdivision),
                (int) MathF.Round(relativeY / worldPixelHeight * SimWorld.Collider.Subdivision));
        }
    }
}