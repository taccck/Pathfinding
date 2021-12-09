# Pathfinding

Serialization
  In Assets > Scripts > Pathfinding > Serialization > SaveNodes.cs I use an xml serializer to write a 2d array to the disk. It's used to avoid performing n^2 raycasts on start to find out which nodes can be traversed from, to another node. Instead the raycasts will only be performed once and then be loaded from a text file.
  
Singleton
  In Assets > Scripts > Pathfinding > LevelPathfinding.cs I use a singleton since only one level will be loaded at the time. It can't be a public static class because it needs to populate its grid of nodes on start. Also, it would be inconvenient to reference it through dependency injection, because it would need a reference in a path requester and then be passed onto the pathfinding script. 

Observer
  In Assets > Scripts > Character > PathRequester.cs I invoke an event system the path indexes setter. Whenever an object should move to the next point in its path it will have a method called for it. The path testing script in the same folder has a method subscribing to the event system which updates the direction it's walking in. In my current project this is an anti pattern because I can get the walk direction in update directly from the path request since there on the same game object. This pattern works better when there are multiple subscribers and the listener and the subscribers aren't on the same game object.  
