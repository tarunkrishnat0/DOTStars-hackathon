# Problem Statement
Input:
- [x] Spawn `N` number of Robots(capsules) at start per category.
- [x] Spawn `M` number of EnergyStations(spheres) at start.
- [x] Option in UI for user to choose the spawn category - robots/energyStation.
- [x] Spawn Robot/EnergyStation in the position where user clicks.
- [x] Spawn Robot/EnergyStation in random positions if user chosses random position spawn.

Conditions:
- [x] On EnergyStation spawn, it starts moving in a random direction with random speed.
- [x] On Robot Spawn, its speed is randomly decided but it always towards the nearest energy point.
- [x] Robots color change based on their closeness to the energy point.
- [x] On hitting the boundary, Robots get reflected.
- [x] On hitting the boundary, EnergyStations get reflected.
- [x] EnergyStation capacity drops when a robot comes near it

Termination:
- [x] On reaching the target EnergyStation, Robots will die due to high voltage :dizzy_face:
- [x] When energy station capacity drops to zero, it dies

# Project Settings
- Unity 2022.2.0f1
- Universal Render Pipeline (Performant Settings)
- Platform: Standalone
- Player Settings
    - Backend - Mono
- Quality & Graphics Settings
    - URP Performant
- Editor Settings
    - Enable Play Mode Options
- Packages
	- com.unity.entities.graphics (1.0.0-pre.15)
    - com.unity.physics (1.0.0-pre.15)

## Attributions

- Hackathon Link: https://www.metaversecreators.dev/hackathon/
- ECS Templates: https://github.com/WAYN-Games/DOTS-Training/tree/DOTS-111/Assets/Editor
- Textures: https://www.youtube.com/watch?v=IO6_6Y_YUdE

## References

- [Unity ECS 1.0 Full Project Tutorial | Turbo Makes Games | YouTube](https://www.youtube.com/watch?v=IO6_6Y_YUdE)
- [DOTS 1.0 in 60 MINUTES! | Code Monkey | YouTube](https://www.youtube.com/watch?v=H7zAORa3Ux0)
- [How to make a game with Unity DOTS ? | WAYN Games | YouTube](https://www.youtube.com/playlist?list=PL6ubahbodJ3N2udo4n9yGQcpbWnqdgYnL)
- [DOTS Guide and Samples | Unity | GitHub](https://github.com/Unity-Technologies/EntityComponentSystemSamples)