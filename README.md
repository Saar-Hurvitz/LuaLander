# LuaLander

A 2D lunar-lander game made in Unity. Fly the lander through rocky terrain, collect coins and fuel, and touch down gently on a landing pad. A softer, straighter landing on a higher-multiplier pad scores more points.

## Requirements

- **Unity 6000.5.2f1** (Unity 6). Install this exact version through Unity Hub. Other versions may trigger an upgrade prompt.
- Works on **Windows and macOS**. The project has no native plugins or platform-specific code.

## Getting started

```bash
git clone https://github.com/Saar-Hurvitz/LuaLander.git
```

1. In Unity Hub, choose **Add → Add project from disk** and select the cloned `LuaLander` folder.
2. The first open takes a few minutes while Unity rebuilds the `Library/` folder, which isn't stored in git.
3. Open `Assets/Scenes/MainMenuScene.unity` and press **Play**.

## How to play

| Action | Keyboard | Gamepad |
| --- | --- | --- |
| Main thruster (push up) | `W` / `↑` | Left stick up |
| Rotate left | `A` / `←` | Left stick left |
| Rotate right | `D` / `→` | Left stick right |
| Pause menu | `Esc` | Start |

- The lander hangs still until you press any control, and then gravity turns on.
- Each thruster burns fuel while you hold it. With an empty tank the controls stop responding.
- **Fuel canisters** refill the tank and **coins** add +500 points.

### Landing rules

You crash if any of these happen:

- **Wrong area:** you hit anything other than a landing pad.
- **Too fast:** impact speed is above 4.
- **Too steep:** the lander is tilted more than about 25° from upright (`dot(up) < 0.90`).

A successful landing scores:

```
(angleScore + speedScore) × pad multiplier
angleScore = 100 − |dot − 1| × 1000     (up to 100 for perfectly upright)
speedScore = (4 − impactSpeed) × 100    (up to 400 for a feather-light touch)
```

Each pad shows its multiplier (for example `x5`). After a successful landing, **Continue** takes you to the next level. After a crash, **Retry** restarts the level. When there are no more levels, the Game Over screen shows your final total score.

The pause menu lets you resume, return to the main menu, and step the sound and music volume through 0–9.

## Project structure

```
Assets/
├── Scenes/          MainMenuScene → GameScene → GameOverScene (all in Build Settings)
├── Scripts/
│   ├── Lander.cs                  Physics, fuel, landing checks and scoring; raises events
│   ├── GameManager.cs             Level loading, score/time, pause, level progression
│   ├── GameLevel.cs               Per-level data: start position, camera target, zoom
│   ├── GameInput.cs               Wrapper around the Input System actions
│   ├── LandingPad.cs / LandingPadVisual.cs   Score multiplier and its "xN" label
│   ├── LanderVisuals.cs / LanderAudio.cs     Thruster particles, explosion, thruster sound
│   ├── SoundManager.cs / MusicManager.cs     SFX and music volume; music continues across scenes
│   ├── CinemachineCameraZoom2D.cs            Smooth camera zoom between level overview and lander
│   ├── SceneLoader.cs             Scene switching by enum
│   ├── Pickups/                   CoinPickup, FuelPickup
│   └── UI/                        MainMenu, Stats (HUD), Landed, Pause, GameOver
├── Prefabs/         Level_1, Level_2, LandingPad, pickups, particle systems, MusicManager
├── InputActions.inputactions      Player controls (generates InputActions.cs)
├── Sounds/  Textures/  Fonts/     Audio, sprites, VT323 pixel font
└── CodeMonkeyFree/                Code Monkey's free editor-window package
```

### How it fits together

The game is event-driven. `Lander` is the single source of gameplay events: `OnLanded`, `OnStateChanged`, `OnCoinPickup`, `OnFuelPickup` and the thruster events. `GameManager`, `SoundManager`, `LanderVisuals`, `LanderAudio` and the UI scripts each subscribe to the events they need.

The main managers (`Lander`, `GameManager`, `GameInput`, `SoundManager`, `MusicManager`, `CinemachineCameraZoom2D`) are scene singletons reached through a static `Instance`. Level number, total score, volumes and music position are kept in `static` fields so they persist when scenes reload.

### Adding a level

1. Duplicate `Prefabs/Level_2.prefab` and edit the terrain, pads and pickups.
2. On its `GameLevel` component, set the next **Level Number**, the lander start point, the camera start target and the zoom-out size.
3. Add the prefab to the **Game Level List** on the `GameManager` object in `GameScene`.

## Tech

- Universal Render Pipeline (2D Renderer)
- Input System (new), with keyboard and gamepad
- Cinemachine 3
- TextMesh Pro, 2D Sprite Shape (terrain), 2D Animation

## Known issues

- `Assets/Scripts/UI/LandedUI.cs` declares a class named `NewMonoBehaviourScript`. Unity expects a MonoBehaviour's class name to match its file name, so rename the class to `LandedUI`.
- `GameInput.cs` and `GameManager.cs` contain unused `using NUnit...` lines. NUnit is a test-only assembly, so these can break player builds. `GameManager.cs` also has an unused `using Unity.VectorGraphics;`. Remove these lines before building.
- `GameManager.AddScore` and `Lander` still print debug messages with `Debug.Log`.

## Credits

- Font: [VT323](https://fonts.google.com/specimen/VT323), SIL Open Font License (`Assets/Fonts/OFL.txt`).
- Includes the free Code Monkey editor package (`Assets/CodeMonkeyFree`).
