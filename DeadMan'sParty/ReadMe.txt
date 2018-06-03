Welcome to the very first iteration of Dead Man's Party (working title)!

The following paragraphs will outline the basic premise of the game, detail the rules, and suggest possible changes to the core concept.

-Premise-

You play as a detective hunting a master serial killer. Substantial evidence and your gut tell you that they've fled to an old hotel inhabited by suspicious characters.
You know the killer's here so you bar off the entrance to the hotel. No one gets in and, more importantly, no one gets out. That is, at least for a week (the local marshall
doesn't like you over-exerting your authority). Until then, you have can decide where to room each suspect for the night. The killer will strike each night so use this power
to determine who they are. Can you identify the murderer before they slip right through your fingers?

-Rules-

The object of the game is to re-room suspects each night to determine which one is the murderer. At the end of each day, the murderer will kill one of their roommates. Track
who was present at each murder scene deduce who might be behind them. Once you feel confident in your decision, re-room that suspect to the execution chamber where they
will be put to death at the end of that day.

- Controls:
	- The entire game is controlled using the mouse.
	- To select a character (identified by a white circle), left-click on them. You can hover over them to see their name.
	- Room characters using the menu on the left side of the screen.
	- When you feel confident with your rooming choices, click the End Day button on the top-right corner of the screen. You can see how many days are left in the game
	in the space above that button. Note: Some conditions may prevent you from ending the day. Check the Unity debug log if this occurs.

- Win Condition:
	The player wins if they place the murderer in the execution room and end the day.

- Lose Conditions:
	The player loses if they end the day with an innocent character in the execution room or if the murderer remains at large after the final day.

- Rooming:
	- The player CANNOT end the day if suspects are located within the lobby. They must be placed in a room for the game to progress.
	- The day CANNOT end if a character is roomed with no roommates (they get lonely or something ... I don't know). Either give them a roommate or re-room them in a room
	with other occupants inside. Note: The day CAN end if a room is empty.
	- Rooms have a max occupancy (strict fire code, I guess). You cannot house more characters in a room than this max occupancy allows. In this version, rooms house
	up to four characters.

- The Execution Room:
	- If you think a suspect is the murderer, you can move them to the execution room.
	- Only one suspect can reside in the execution room at a time.
	- Characters placed in this room are executed at the end of the day. This gives you a chance to change your mind.
	- A win or lose condition will trigger if the day ends with a character in this room. Be sure you are confident in your room placement when you end this day.

- Hints:
	- The killer will not strike if they are housed with a single roommate. This would give them away.
	- This version handles a good deal of player feedback through the Unity debug log. I'd strongly recommend playing this version in the Unity editor until we work to
	implement actual feedback. Open the GameplayTestScene file to get to the action. For now, tolerate these placeholders.
	- I've found it helpful to use a pen and paper to track suspects.
	- If you wish to observe murderer behavior and debug as you expand on it, a chunk of code in the Character script changes the murderer's sprite for easy
	identification. This has been commented out. Just uncomment the chunk at line 84 of the Character script for this functionality.

-Changes-
	- As of now, the game is far too easy. We should work to make it harder for the player to determine who the killer is after the first few nights. One suggestion would
	be to up the number of murderers and have them switch off who gets to kill each night. Players must then execute all killers to win the game.
	- As mentioned above, it is vital to play this version in the Unity editor as the game messages players using the debug log. We will need to implement proper feedback
	in a later version.
	- Know that the premise isn't permanent. We can set it anywhere we want and even alter core elements of the game. We could even consider repurposing mechanics from our 
	Jaws game to this. Everything here is just a prototype of a rough idea. And boy is it rough.
	- Speaking of rough, this game has bugs. I don't know what or where they are just yet. I'm writing this must minutes after finishing the game's first version! But I
	know they're out there. Let me know if you can find any of them.
	- From now on, I'll be much more active in the Discord server. I had to spend some time developing this prototype on my own to properly explain the idea to everyone.
	Apologies for that...

Best,
Sam