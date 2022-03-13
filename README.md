# OtchlanMapGenerator
###***[Project in progress]***
Map generator for the Polish text-game Otchłań (https://otchlan.pl/).

Project is created entirely with Windows Forms C# - it's premise is to make user's ability to navigate in the game world easy and more efficient,
thanks to automatically generated map of unlimited size and providing the function of determining the shortest route to the selected location.

##:Brief description:

Player moves in game by entering any substrings of words "north" / "south" / "east" / "west"
Application detects these substrings and automatically creates a map of visited locations in real time.
Reading names, exit directions and descriptions is done via OCR, which takes more time, so location data is updated in the background with some delay.

At the moment, application displays information about the location after a single click on the segment, allows correcting the names and descriptions after pressing the "Show details" button, which also displays description of selected location and (after double-clicking) displaying route from player's position to selected location (with appropriate sequence of commands that can be pasted into the game).
Two languages are available and ability to block keyboard input (recommended when leaving the game and application windows).

>Potential further possibilities of the application can be read in the [List of Todo's and Ideas](TODO.txt) file


##:Some usage images:

![App Viev](https://github.com/chylaa/OtchlanMapGenerator/blob/master/AplicationViev.png)
![Route mapping](https://github.com/chylaa/OtchlanMapGenerator/blob/master/AplicationVievRoute.png)


