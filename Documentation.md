# System Documentation for A Purrfect Escape

There are a number of system involved in the design of this game. They were designed to be abstract enough to be applied to any objects in the game, and make changing the game much easier.

This documentation covers a lot of the systems in the game and serve as a reference so that you can understand why certain methods in the scripts are being called at various times.

## Project Information

The version of Unity that the project uses is `2017.4.2f2`. Please make sure that you are using this version of Unity before you modify any of the objects. modifying scripts should be fine, regardless of version.


## Inventory System

## Light System

## AI

## Persistence: Saving and Loading
Unity Scenes are used for auxilary parts of the game, including menus, credits, etc. However the gameplay data itself is actually loaded from and saved to a number of files on the disk. This data is saved in `JSON` Format. Each level is encoded with a `"Name "` string and a `"Contents"` Dictionary. This dictionary associates the IDs of gameobjects to the pieces of data that each object contains. All objects have a `"transform"`, which is stored as a comma separated string: `x, y, rotation, scale-x, scale-y`, and a `"type"`, which corresponds to a PersistantType that helps load the prefab corresponding to the savable object. However, these are not the only keys that savable objects can have. Infact, anything that implements the `IPersistantObject` (Ignore the misspelling of persistent) interface will be able to save it's state.

### IPersistantObject
This Interface requires you to inherit a number of methods:

#### `void PreSave()`
Anything that should be done before the object is saved can be called in this method

#### `void Load(Dictionary<string, string> saveData)`
This is a method that is called on a freshly instaniated prefab.
The transform is already loaded onto the prefab so you don't need to worry about loading that. All you need to do is set values for keys that you may have already saved for that object.

#### `void PostLoad()`
This method is called after all the objects are loaded. You can use this to add items to an inventory (such as the player), after all the inventory items have been loaded.

#### `Dictionary<string, string> Save()`
This returns a Dictionary that will be parsed into json and saved. You do not have to worry about saving the transform of an object, that is already done.

#### `void Unload()`
This is a method that destroys the game object associated with this script. it usually invokes:
```c#
God.Kill(this.gameObject);
```
This method is called just before the next level is loaded, after the previous level was saved.

#### `PersistanceType GetPType()`
Returns the PersistanceType of this object, which is saved as the type of this object. to save a new type of thing, you will need to add a new type to the `PersistanceType` Enum, and then add the new type along with it's associated prefab to the `Savior` Object in the `Main` scene. this Savior object should know all of the prefabs in the game.

#### `IEnumerable<string> PersistThroughLoad()`
This function is a little confusing, because it is named persist, in a different way that persist is used elsewhere in the code. This means all of the keys that should keep the version of that key from the previous level, instead of loading it like the rest of everything else that is loaded after the game. For instance, we want to player's inventory to persist after loading. So we want to use the inventory that is currently on the player, and not the inventory that is on file. This data is transfered to the next level when the next level is loaded.

#### `MonoBehaviour GetMono()`
This is the MonoBehaviour object that is associated with the script. Most of the time, this is the script itself, as anything that needs to be saved should be a monobehavior. this is used in order to get and set the transform of the object.s


### Main organization of the game
There are two subfolders that game data goes into: `Assets\Levels` and `Assets\Save`. `Levels` stores the pristine versions of each level. it stores the prototype objects of the levels. That way, whenever a new game is started, the levels are copied to the `Save` folder, and the state of the game is saved there. everything, including the starting point of the player, the location of items, the state of doors and guards, is remembered between levels, because it is saved directly to these files. It is important that this delineation is preserved. 

Level Designers should be using the `Savior` in Save Mode to write new levels or overwrite old ones in the Levels folder, but when the game itself loads, it should never touch the Levels folder other than to read from it. All of the game's state is stored in the `Save` folder. This will also make game post processing easier.


## Data Collection