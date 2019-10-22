# Changelog

## 2019.3.0

Requires Unity 2019.3.0 or later.

#### Added

* **Call Tree Explorer :** Using Window/Gameplay Ingredients/Call Tree Explorer , opens a window that lists the tree of Events, Logic and Actions, State Machines and Event Calling Actions
* **Folders:** In the Game Object creation Menu, Select folder to add a folder in the hierarchy. Automatically adds Static Game Objects with colored icon (Displayed using Advanced Hierarchy View)
* Added option in GameplayIngredientsSettings to disable visibility of Callable[] bound to Update Loops.
* Added OnUpdate Event : Perform calls every update
* Added OnColider Event :  Perform calls upon collisions
* Added OnJoinBreak Event : Perform calls upon Rigid body joint break
* Added FlipFlop Logic : Two-state latch logic
* Added State Logic : Perform logic based on State Machine current state.
* Added Audio Mix Snapshot Action : Set Mixer Snapshots
* Added RigidBody Action : Perform actions on a rigidbody
* Added SetAnimatorParameterAction : Perform parameter setting on Animators
* Added Sacrifice Oldest option to Factory
* Added Folder Cosmetic Behavior (for view in Advanced HIerarchy View)
* Added Timer Component
* Added TimerAction to control Timer
* Added TimerDisplayRig
* Added Global Variables (Globals + Local Scope)
* Added Global Variable Set Action
* Added Global Variable Logic
* Added Global Variables Reset Action
* Added Context Menu in ToggleGameObjectAction to update entries based on current enabled state in scene. 

#### Changed

- Moved Menu Items in Window menu into a Gameplay Ingredients Subfolder
- GameManager Resets Global Variables Local Scope on Level Load

#### Fixed

* Fixed LinkGameView not working in play mode when excluding VirtualCameraManager.
* Fixed Performance issue in GameplayIngredientsSettings when having a big list of Excluded managers.
* Fixed ApplicationExitAction : Exits play mode when in Editor.

## 2019.1.2

#### Changed

* **[Breaking Change]** Discover Assets now reference many Scenes/SceneSetups 
  * Action to take: have to re-reference scenes in Discover Asset

#### Added

* Added Screenshot Manager (Defaults to F11 to take screenshots)
* Added OnMouseDownEvent
* Added OnMouseHoverEvent
* Added OnVisibilityEvent
* Added SaveDataSwitchOnIntLogic

#### Fixed

* Fixed warning in CycleResolutionsAction



## 2019.1.1

#### Changed

#### Added

* Log Action
* Added Playable Director to objects in discover (to open atimeline at a give playable director)
* Added support of Game Save Value index for Factories (in order to select a blueprint object from a saved value)

#### Fixed

* Fixed Import Errors at first project load, including the way we load discover and GameplayIngredients project settings

* Secure checks in Gathering Manager classes from assembly (skips protected assemblies now)

  

## 2019.1.0

#### Changed

* Removed counts in OnTriggerEvent
* Callables can now be friendly-named (with default formatting)
* Updated Starter Packages

#### Added

- Added NTimesLogic (split from OnTriggerEvent)
- Added Replace Mode for Level Streaming Manager
- Added UIToggle Action and Property Drawer
- Added Audio Play Clip Action
- Added Platform Logic

- New Welcome Screen, with Wizard
- New optional GameplayIngredients Project Configuration asset 
  - Toggles for verbose callable logging
  - Manager Exclusion List
- New Scene from Template Window + Config SceneTemplateLists Assets
  - Helps creating new scenes from user-made templates
- New Discover Window System:
  - Adds a new DiscoverAsset to reference Levels / Scene Setups
  - Adds new Discover components in scenes
  - Discover window helps navigate scenes while in editor and discover content.
- Added improved Game Manager
  - Manages loading of main menu & levels directly instead of using LevelStreamingManager
  - Manages Level Startup in sync after all scenes have started.

#### Fixed

* Fixed code to run on Unity 2019.1
* Fixed factory managed objects upon destroy
* Fixes in LinkGameView when application is not playing
* Fix in LevelStreamingManager incorrect computation of Scene Counts
* Fixes in VirtualCameraManager
* Fixes in Find/Replace window
* Fixes in Hierarchy View Hints for Unity 2019.3 new skin



## 2018.3.0

Initial Version
