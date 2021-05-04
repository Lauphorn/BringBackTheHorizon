Hi!

Thank you for purchase!

If you use Unity version newer than 5.6, you need to re-build the lighting in the demo scene by pressing the Build button in the Lighting window.


If you want to achieve the same visuals as on screenshots and video, check the Color Space settings in Player Settings, it should be set to Linear. Also check the Render Path settings in Graphics Settings, it should be set to Deferred.
Use free Unity 'Post Processing Stack' camera effects (https://www.assetstore.unity3d.com/en/#!/content/83912) and the adjusted profile for that, which can be founded in the QA_Office/Demo scene/ folder.


>>>For the best performance in the demo scenes you have to follow the next instruction<<<
1. Switch rendering path to Deferred (for VR you can still use Forward rendering path with the MSAA enabled)
2. Calculate Occlusion Culling
3. Decrease Realtime Shadows Distance to 10-15 or something or even bake the lighting in Subtractive mode for the best performance (you can do that with the 5 texels resolution for the quick test).


>>>DOORS<<<
To use the doors interaction (open/close doors in Play Mode with the "E" key) you have to be sure, that you set the same "PlayerHeadTag" in the door script inspector as you have on your player's camera. Also you have to add a sphere collider (and set it to trigger mode) to the player's camera.


>>>ELEVATORS<<<
How to setup the elevator system in your scene step by step guide:

1. Drag & drop the Elevator prefab into your scene from the QA_Office/Prefabs/ folder;

2. Copy the Elevator few times and set the "Current Floor" property to appropriate floor;

3. Type your player's tag into the "Player tag" field (it's set to "Player" by default);

4. If you need more than one elevator shafts in the scene, you have to create an empty gameobject for the each group of elevators (shaft), and put elevators inside these empty objects (parent them). You can add the ElevatorManager.cs script to these parent empty objects to be able to set a random floor at start or set the floor manually.

To learn more about other properties in the Elevator inspector, simply move your mouse cursor over the property name and you will see a hint.



If you have problems with this package, please, let me know mr.andreyvolkov@gmail.com

Don't forget to build your own original levels with this modular kit :) 