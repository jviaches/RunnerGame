# RunnerGame
Runner where player must espace from skull island. Main character steal a Jeep and runaway with it. Player can control car onlyin  2 directions (left and right). Road can go right left or streight. Player runs away through the desert (10 levels) and each level he is being followed by car trying to capture him (enemy car touch is enoigh). Besides avoiding enemi's touch, in order to succesfully pass level, player also must not exceed level time (countdown) which is might be different from level to level.

Player able to pick up (located on road) 2 types of (randomly generated) bonuses: increase countdown time for X seconds and temporary speed increase for X seconds.
When car is being hitted by some object, it looses its speed. When car is out of the road, it is "roll over" (immidiately game over).

Game mechanics:
User controls car by clicking/taping on the screen. Any click or tap will change running object direction: left or right.


![GitHub Logo](/media/mafia_image.PNG)

###########################   Road Map for Ver 1.0    ###########################

**Description** Player able to complete 10 customized levels of driving car in desert with limited amount of time (countdown) and also being followed by 1 or 2 cars trying to "touch" him.
- Android application (apk). atleast for our 2 devices it is scalable as it should be.
- Player able to pick randomly generated bonuses
  - increase countdown time for 5 seconds
  - increase countdown time for 10 seconds
  - increase temporary speed for 5 seconds
  - increase temporary speed for 10 seconds
- Player hit:
  - On road obsticle hit reduces speed to complete stop
  - Of road hit "roll out car animation" + level failed.
  - Player's car touched by enemy vechicle leads to fail to pass level.
- Main screen (background image + 3 buttons)
  - Start game
  - Settings
    - Background music ON/OFF
  - Exit
- Level completed screen (when user able to succseffuly complete level) with time result and next level button.
- Level failed screen (when user failed to complete level) with time result and retry button.
- Graphics
  - Main screen (background + buttons)
  - Level completed screen (background + buttons)
  - Level failed screen (background + buttons)
  - Moving object (model/prefab) 
  - Walls 
  - Level environment (surrounding)
  - Bonuses
    - increase countdown time for 5 seconds 
      - ICON
      - 3D object (prefab)
    - increase countdown time for 10 seconds
      - ICON
      - 3D object (prefab)
    - increase temporary speed for 5 seconds
      - ICON
      - 3D object (prefab)
    - increase temporary speed for 10 seconds
      - ICON
      - 3D object (prefab)
- Sound
  - Background sounds(when playing level)
  - Button pressed sound (any button same sound)
  - Level completed succesfully
  - Level failed
  - Coin collection sound
  
  This is the bare minimum till May 30.
