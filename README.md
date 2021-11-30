# LilaGamesSolution
Unity Developer Test Solution for Lila Games

**Solution 1 Overview**

To view the completed scene, open the scene in *"Assets/Scenes/Problem 1 Solution.unity"*. 

To change the configurations in the problem requirements, in the scene, select the game object *"Grid Canvas/Tile Manager/"*, then change the *Tile Manager* component script configurations. Hover your mouse over each option to see a tooltip to get more information on what each option does. Note: You have to press play in the editor for the tiles / grids to be generated. The grid / tiles will be procedurally generated and fitted for any aspect ratio / resolution when the scene is played.

![Component Config](https://i.imgur.com/vx9WNkf.png)

Scripts for this solution may be found in the directory *"Assets\Scripts\Problem 1 Scripts"*. 

**Solution 2 Overview**

Scripts for this solution may be found in directory *"Assets\Scripts\Problem 2 Scripts"*. 

Here is kind of an overview of how the code is architectured:

The weapons class was designed with scalability in mind, so creating new primary and secondary weapons only require you to create classes which inherit the respective primary and secondary classes. The base weapon class implements basic / standard weapon behavior, however inheritted weapon scripts may implement / override their own unique behavior. Weapon attributes are stored in their own flyweight pattern reference called the weapon config.The weapon config class simply exposes data to be utilized by the weapon behavior (max clip size / fire rate / damage per bullet / etc).

The player class has two primary weapon slots and one secondary weapon slot, as well as an actively equipped ("armed") weapon that's used which the UI observes. The player class utilizes the base weapon class interface of the "active" weapon and calls its fire weapon functions depending on the player's input (mouse button down / held / released) in the update function frame by frame. It's up to the individual weapon scripts to decide how the behavior is implemented once the fire functions are called.

The weapon UI HUD simply observes events in the player and the active weapon equipped. It's best practice for UI to follow the observer pattern since the objects in question should have nothing to do with handling when / how UI should be changed. UI and behavior should always be decoupled.

![Solution 2 Overview](https://i.imgur.com/33BcdDW.png)
