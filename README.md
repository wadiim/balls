# Balls

A GUI application written in C# for a concurrent programming class. It simulates the movement of balls within a rectengular space, although the simulation is not intended to be realistic. The application employs the concept of concurrent programming by implementing each ball as a separate thread.

<p align="center">
  <img src="https://github.com/wadiim/tetris/assets/33803413/6a483d99-0e76-48eb-8a1d-1353779b687b" />
</p>

## Features

* **Collisions:** The balls can collide with each other. When a collision occurs, the velocities of the colliding balls are simply swapped. This collision implementation does not aim for realistic physics.
* **Collision detection:** Two conditions must be met for a collision to be detected between balls. Firstly, they must touch or overlap, and secondly, they need to be getting closer to each other (i.e. the distance between their centers must decrease over time). This second condition prevents the balls from sticking together.
* **Dynamic refresh rate:** The refresh rate of each ball is dependent on its speed. Faster-moving balls are updated more frequently, ensuring smoother animations.
* **Layered design pattern:** The program employs a layered design pattern, which provides a structured and modular approach to organizing the codebase.

## License

[Apache License 2.0](https://github.com/wadiim/balls/blob/main/LICENSE)
