# CaiLearning01

My first Unity game development project.

I'm learning C#, Unity, Git, and software development fundamentals by building small projects from scratch.

## Current Features

- 2D player object
- WASD and arrow-key movement
- Adjustable movement speed
- Frame-rate independent movement
- Diagonal movement normalization
- Enemies, obstacles and healing items
- Score, Health, Wallet, Crate Durability, and Tool Uses UI 
- Repair options, Collect Coin, Restart buttons
- Timed message to reflect repair states
- Centred square game view with fullscreen and resizable-window support.

## Controls

- Use WASD or Arrow-keys to move player
- Press and hold Left Shift to run
- Press E to use small repair or click Repair 1
- Press Q to use strong repair or click Repair 2
- Click Restart anytime or press R after dying or winning to restart game
- Click Collect Coin to add 1 coin to wallet
- Click Damage to reduce crate durability by 1, separate from player's health
 
## How to Run 

1. Install Unity Hub and Unity Editor 6000.3.22f1 (Unity 6.3 LTS).
2. Clone or download this repository. If downloaded as a ZIP,
   extract it first.
3. In Unity Hub, select Projects → Add → Add project from disk.
   Select the project folder containing Assets, Packages,
   and ProjectSettings.
4. Open the project and wait for Unity to finish importing
   assets, resolving packages, and compiling scripts.
5. In the Project window, open Assets/Scenes/SampleScene.unity.
6. Press Play. Click inside the Game view to give it keyboard
   focus, then use the controls listed above.

## How to Survive

Collect **5 dark purple circles** to win and stop all the enemies. Watch the bright red fire pit nearby: it deals **1 damage** each time you touch it.

- **Dark red enemy:** Moves slowly but deals **3 damage** per hit. One hit can end your run.
- **Brown enemy:** Chases you when you get close and deals **1 damage per second** while you remain in contact. Get far enough away and it stops chasing.
- **Pink enemy:** Patrols four points near the purple circles and deals **1 damage** per hit. It chases you within range and returns to patrolling when you escape.

Avoid the dark pit—it kills you instantly. The nearby pool of acid and blood deals **2 damage**, which can also be fatal if your health is low.

Need healing? The green triangle restores **1 health**, and the yellow stick restores you to full health.

Remember: you’re faster than all three enemies naturally, and you can double your speed by holding Left Shift. Try to lure and strand some enemies. You can win this. Good luck!

## Built With

- Unity
- C#
- Git & GitHub

## Learning Goals

This repository documents my progress as I learn programming and game development.