using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

using AntEngine;
using AntEngine.Colliders;
using AntEngine.Entities;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;
using AntEngine.Resources;
using AntEngine.Entities.Pheromones;
using AntEngine.Entities.Strategies.Movement;
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
        private PaintBrushSelection.PaintBrushState _paintBrushState = PaintBrushSelection.PaintBrushState.Wall;

        #region Colony constants
        private readonly Vector2 _colonyScale;
        private readonly int _colonySpawnCost;
        private readonly int _colonyStockpileStart;
        
        private readonly int _colonySpawnDelay;
        private readonly int _colonySpawnBurst;
        private readonly float _colonySpawnRadius;
        #endregion

        #region Food constants
        private readonly Resource _food;
        private readonly Vector2 _foodScale;
        private readonly int _foodValue;
        #endregion

        #region Ant constants
        private readonly float _antMoveRandom;
        private readonly float _antMoveOldDir;
        private readonly float _antPerceptionDistance;
        private readonly int _antPerceptionPrecision;
        private readonly int _antPheromoneFoodEmit;
        private readonly int _antPheromoneFoodMax;
        private readonly int _antPheromoneHomeEmit;
        private readonly int _antPheromoneHomeMax;
        private readonly float _antPheromoneMergeDistance;
        private readonly int _antPheromoneDelay;
        private readonly float _antPickupDistance;
        private readonly int _antPickupCapacity;
        private readonly float _antMaxSpeed;
        #endregion

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

            Rectangle paintBrushSelectionRectangle = new Rectangle(rect.Width - rect.Width / 10, rect.Height / 2, 32, 32 * 3);

            PaintBrushSelection paintBrushSelection =
                new PaintBrushSelection(paintBrushSelectionRectangle, PaintBrushSelection.PaintBrushState.Wall);
            _renderers.Add(paintBrushSelection);

            paintBrushSelection.PaintBrushStateChange += OnBrushStateChange;
            
            _colonyScale = Vector2.One * float.Parse(AntSimulator.Properties.Get("colony_scale", "20"), CultureInfo.InvariantCulture);
            _colonySpawnCost = int.Parse(AntSimulator.Properties.Get("colony_spawncost", "10"));
            _colonyStockpileStart = int.Parse(AntSimulator.Properties.Get("colony_stockpilestart", "50"));
            _colonySpawnDelay = int.Parse(AntSimulator.Properties.Get("colony_spawn_delay", "16"));
            _colonySpawnBurst = int.Parse(AntSimulator.Properties.Get("colony_spawn_burst", "1"));
            _colonySpawnRadius = float.Parse(AntSimulator.Properties.Get("colony_spawn_radius", "0.1"), CultureInfo.InvariantCulture);

            _food = new Resource("food", "fruit");
            _foodScale = Vector2.One * float.Parse(AntSimulator.Properties.Get("food_scale", "10"), CultureInfo.InvariantCulture);
            _foodValue = int.Parse(AntSimulator.Properties.Get("food_value", "50"));

            _antMoveRandom = float.Parse(AntSimulator.Properties.Get("ant_move_random", "0.5"), CultureInfo.InvariantCulture);
            _antMoveOldDir = float.Parse(AntSimulator.Properties.Get("ant_move_olddir", "0.9"), CultureInfo.InvariantCulture);
            _antPerceptionDistance = float.Parse(AntSimulator.Properties.Get("ant_perception_dist", "50.0"), CultureInfo.InvariantCulture);
            _antPerceptionPrecision = int.Parse(AntSimulator.Properties.Get("ant_perception_precision", "24"));
            _antPheromoneFoodEmit = int.Parse(AntSimulator.Properties.Get("ant_pheromone_food_emit", "2400"));
            _antPheromoneFoodMax = int.Parse(AntSimulator.Properties.Get("ant_pheromone_food_max", "2400"));
            _antPheromoneHomeEmit = int.Parse(AntSimulator.Properties.Get("ant_pheromone_home_emit", "12000"));
            _antPheromoneHomeMax = int.Parse(AntSimulator.Properties.Get("ant_pheromone_home_max", "20000"));
            _antPheromoneMergeDistance = float.Parse(AntSimulator.Properties.Get("ant_pheromone_mergedist", "5.0"), CultureInfo.InvariantCulture);
            _antPheromoneDelay = int.Parse(AntSimulator.Properties.Get("ant_pheromone_delay", "30"));
            _antPickupDistance = float.Parse(AntSimulator.Properties.Get("ant_pickup_distance", "5.0"), CultureInfo.InvariantCulture);
            _antPickupCapacity = int.Parse(AntSimulator.Properties.Get("ant_pickup_capacity", "15"));
            _antMaxSpeed = float.Parse(AntSimulator.Properties.Get("ant_maxspeed", "1.0"), CultureInfo.InvariantCulture);
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
                if (0 > worldDivX || worldDivX >= SimWorld.WorldColliderDivision || 0 > worldDivY ||
                    worldDivY >= SimWorld.WorldColliderDivision)
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
                            (name, transform, world, _) => new Ant("Ant", transform, world)
                            {
                                MaxSpeed = _antMaxSpeed,
                                MovementStrategy = new WandererStrategy(_antMoveRandom, transform.GetDirectorVector(), _antMoveOldDir),
                                PerceptionDistance = _antPerceptionDistance,
                                PerceptionMapPrecision = _antPerceptionPrecision,
                                FoodPheromoneTimeSpan = _antPheromoneFoodEmit,
                                FoodMaxPheromoneTime = _antPheromoneFoodMax,
                                HomePheromoneTimeSpan = _antPheromoneHomeEmit,
                                HomeMaxPheromoneTime = _antPheromoneHomeMax,
                                PheromoneMergeDistance = _antPheromoneMergeDistance,
                                PheromoneEmissionDelay = _antPheromoneDelay,
                                PickUpDistance = _antPickupDistance,
                                PickUpCapacity = _antPickupCapacity
                            })
                        {
                            Transform = {Position = new Vector2(simX, simY), Scale = _colonyScale},
                            SpawnDelay = _colonySpawnDelay,
                            SpawnBurst = _colonySpawnBurst,
                            SpawnRadius = _colonySpawnRadius
                        };
                        colony.SpawnCost.AddResource(_food, _colonySpawnCost);
                        colony.Stockpile.AddResource(_food, _colonyStockpileStart);
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

        /// <summary>
        /// Updates the position of all PaintBrushPosition objects in this SimFrame.
        /// </summary>
        public void UpdatePaintBrushPosition(int newPosition)
        {
            foreach (IRenderer _renderer in _renderers)
            {
                if (_renderer is PaintBrushSelection pbs)
                {
                    pbs.Position = (newPosition, pbs.Position.Y);
                    pbs.RefreshPositions();
                }
            }
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