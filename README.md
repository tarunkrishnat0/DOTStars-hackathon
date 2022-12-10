# Problem Statement
Input:
- [x] Spawn `N` number of Robots(capsules) at start.
- [x] Spawn `M` number of EnergyStations(spheres) at start.
- [ ] Spawn EnergyStation based on the user clicks.

Conditions:
- [ ] On EnergyStation spawn, it starts moving in a random direction with random speed.
- [x] On Robot Spawn, its speed is randomly decided but it always towards the nearest energy point.
- [ ] Robots color change based on their closeness to the energy point.
- [ ] On hitting the boundary, Robots and EnergyStations get reflected.

Termination:
- [ ] On reaching the target EnergyStation, Robots will die due to high voltage :dizzy_face:

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

## Attributions

- Hackathon Link: https://www.metaversecreators.dev/hackathon/
- ECS Templates: https://github.com/WAYN-Games/DOTS-Training/tree/DOTS-111/Assets/Editor
- Textures: https://www.youtube.com/watch?v=IO6_6Y_YUdE

## References

- https://www.youtube.com/watch?v=IO6_6Y_YUdE
- https://www.youtube.com/watch?v=H7zAORa3Ux0
- https://www.youtube.com/playlist?list=PL6ubahbodJ3N2udo4n9yGQcpbWnqdgYnL
- https://github.com/Unity-Technologies/EntityComponentSystemSamples