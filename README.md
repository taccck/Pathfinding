Sam Nilsson   
An implementation of A* pathfinding

Patterns: 
Serialization   
  In Assets > Scripts > Pathfinding > Serialization > SaveNodes.cs I use an xml serializer to write a 2d array to the disk. It's used to avoid performing n^2 raycasts on start to find out which nodes can be traversed from, to another node. Instead the raycasts will only be performed once and then be loaded from a text file.
  
Singleton   
  In Assets > Scripts > Pathfinding > LevelPathfinding.cs as LevelPathfinding.current I use a singleton since only one level will be loaded at the time. It can't be a public static class because it needs to populate its grid of nodes on start. Also, it would be inconvenient to reference it through dependency injection, because it would need a reference in a path requester and then be passed onto the pathfinding script. 

Observer    
  In Assets > Scripts > Character > PathRequester.cs as PathIndex I invoke an event system from the path indexes setter. Whenever an object should move to the next point in its path it will have a method called for it. The path testing script in the same folder has a method subscribing to the event system which updates the direction it's walking in. In my current project this is an antipattern because I can get the walk direction in update directly from the path request since they’re on the same game object. This pattern works better when there are multiple subscribers and the listener and the subscribers aren't on the same game object.  

Dirty Flag    
  In Assets > Scripts > Pathfinding > LevelPathfinding.cs > ResetNodes() I use dity flag by looping through all nodes and checking if they’re marked as modified and only if their values get reset. This is also an antipattern because I could reset the value of all nodes and it wouldn't make a difference or I could pass in the close and open sets from the pathfinding script and only reset their nodes since they contain all the modified nodes. 

Dependency Injection    
  In Assets > Scripts > Pathfinding > LevelPathfinding.cs > GetClosestNode() I pass a node array to the method through its parameter to find which of them is closest to the position. I could also make a private node array in the class but I feel that makes the logic less clear. Also, if I did that for all values I pass on in parameters the amount of variables at the top of the file would skyrocket and give me a headache. 
