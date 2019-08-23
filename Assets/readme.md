# Swarmify
Swarmify is a simple swarm motion planning and collion avoidance simulation based on the potential field method.

## How it works
Without going into detail. Every agent (Followr) has its own sensor range. Whithin that it reacts on other agents by moving away from them. Also, every agent has a target (Leader) which it tries to reach.
In other words. The movement directions results of the sum of all repulive parts (ohter agents) and the attractive part (target).
Properly set up an tweaked this leads to pretty fluent and also collision free motion/path planning.

... The terms Leader, Attractor and Target as well as Agent, Drone, Follower are used synonymous.

## Basic set up guide

### One Leader and one Autonomous drone
1. Import Package
    * You can start playing around with the test scenes right away
    * Add scenes to build settings to enalbe scene switch buttons (Autonomous, AutonomousDirected and Centralized)
2. Add **AutonomousDrone** to you scene
3. Add **Leader** to you scene
4. Set **Attractor** of **AutonomousDrone** by dragging **Leader GameObject** in Inspector
5. Hit Play
* Works the same with **AutonomousDroneDirected**
    * only that **AutonomousDroneDirected** rotates its model torwards the **Leader**.

### One Leader and multiple Autonomous drones
Just add more **Drones** and assign the same **Leader**

### Centralized setup
Same as the Automnomous setup. In addition add a **CentralizedProcessor** to you scene.
Don't forget to assign the **Leader**/**Attractor** of the **Drone**.