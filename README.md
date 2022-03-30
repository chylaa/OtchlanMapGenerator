# OtchlanMapGenerator
### ***[Project in progress]***

Map generator for the Polish text-game Otchłań (https://otchlan.pl/).

Project is being created entirely with Windows Forms C# - it's premise is to make user's ability to navigate in the game world easy and more efficient,
thanks to automatically generated map of unlimited size and providing the function of determining the shortest route to the selected location.

## :Brief description:

Each square represents a single location. Its appearance reflects possible exit directions. Player moves in game by entering any substring of words "north" / "south" / "east" / "west" / "up" / "down"
Application detects that substring and automatically creates a map of visited locations in real time.
Reading names, exit directions and descriptions is done via OCR, which takes more time, so location data is updated in the background with some small delay.

At the moment, application displays information about the location after a single left-click on the segment, allows changing player position by right-clicking on segment and display description of selected location after pressing the "Show details" button, which also allows to correct names and descriptions. By selecting segment with double-click application displays route from player's position to selected location (appropriate sequence of commands is automatically copied to clipboard so it can be then pasted into game). Using the "Search" tab in Menu bar, user can find a location that contains a given fragment of text in the name or description.

Two languages are available and ability to block keyboard input (recommended when leaving the game and application windows).

At any moment map can be saved/opened with hot key or in Menu bar. Users can also adjust colors of their map in "Viev" tab. Color theme is then assigned to a specific save file.

>Potential further possibilities of the application can be read in the [TODO.txt](src/TODO.txt) file


## :Some usage images:
(20.03)
![App Viev](https://github.com/chylaa/OtchlanMapGenerator/blob/master/AplicationViev.png)

![Route mapping](https://github.com/chylaa/OtchlanMapGenerator/blob/master/AplicationVievRoute.png)

![Location Details](https://github.com/chylaa/OtchlanMapGenerator/blob/master/AplicationVievDetails.png)

