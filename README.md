# GDAPDEV-MP-RPG
 3D Rpg using Unity for Mobile

## WIP
 * Gesture Manager - Slide input & Accelerometer
 * Import AudioManager & DialogueManager

## Notes
 * Use Awake() for Managers 
 * Use OnEnable() for PoolableObjects/Objects expected to be disabled and reenabled
  * Use Start() for Objects dependent on Manager Instances -- Managers need to setup first on Awake() & OnEnable()