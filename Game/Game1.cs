using System.Diagnostics;
using Basement.Core.Services;
using Basement.Engine.Input;
using Basement.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Basement;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = default!;
    private AssetService _assets = default!;

    private readonly StateStack _states = new();
    private readonly InputState _input = new();
    private readonly RenderService _render = new();

    public GameContext Context { get; private set; } = default!;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _render.Load(GraphicsDevice);
        _assets = new AssetService(Content);

        _assets.RegisterTextureAtlas(
            atlasId: "Character",
            jsonPath: "Content/Sprites/Atlas/character.json",
            textureKey: "Sprites/Atlas/character"
        );

        Context = new GameContext {
            Input = _input,
            States = _states,
            GraphicsDevice = GraphicsDevice,
            Content = Content,
            Render = _render,
            Assets = _assets
        };

        _states.Push(new TitleState(Context));
    }

    protected override void Update(GameTime gameTime) {
        _input.Update();

        _states.HandleInput(gameTime);
        _states.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _states.Draw(gameTime, _spriteBatch);

        base.Draw(gameTime);
    }
}
