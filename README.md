Overview:
Unity test task, made with Unity 2018.2.8f1.

Approach:
The main goal is to create a universal and versatile extension that handles distances and occlusion status of objects relative to a reference point (setted in run-time, i.e. 
a player).
With versatility in mind i decided to create a two-layered system where layers are expandable and independent. Fist layer is "detectable objects" where I'll store and configure
object references and methods which are called when said object is detected. Second layer is "detection zones", it's there to handle different detection zones and methods
which are called when any of the detectable objects is in the zone. This layer is nessesary to be used in stuff like AI programming, when NPCs not only have line of sight 
but also hearing ability. The only setting which is excluded from those two layers is line of sight cone radius. I decided to rely on gizmos heavily because this is supposed to be
an easy to use extension.

Technical stuff:
The magic is in the UnityEngine.Physics.OverlapSphere method which returns an array of colliders present in the sphere with the position of a Detector object and customizable 
radius. To detect if the target is occluded i used UnityEngine.Physics.Raycast. To figure out if detector looks away from the target I created a directional vector between detector's 
and target's positions and checked if it happens to be in detector's FOV. To calculate distance between detector and target i used Vector3.Distance method. 

Usability:
With the help of Gizmos i visualized the whole thing making it somewhat pleasant to use. I also included settings to configure detection zone types.
