# PuzzleLab

Unity project for prototyping and testing puzzles.

## Completed

- Grid System (took way too long to implement, but seems to be working now)
    - Change `Grid Size` in inspector, script will automatically determine which tiles must be added/removed.
    - 2D array is serialized in inspector, and makes it easy to find a tile via script, given its X/Y coordinates.
    - Tiles have a dropdown allowing one to select whether it is a Floor/Wall/Chasm tile.
    - Tile positions **cannot be manually adjusted**, their position is predetermined by their position in the grid 2D array.
    - **Tip**: Quickly create layouts by shift-clicking and dragging areas to select a group of tiles, or ctrl-clicking and dragging to deselect. You can then change the tile type of all selected tiles. 
- Player movement
    - Walking directions
    - Dectects walkable vs. non-walkable tiles
    - Can walk on TumbleBridges
- TumbleBridge behaviour
    - Tumbles in any direction as long as there is room available, will become 'consolidated' once it lands in a chasm, becoming walkable but no longer allowing any control
- End Area (Will change to "COMPLETE!" after player reaches it)

## To Do

- Create DemoBot bot behaviour (Imitate player behaviour, but takes up 2+ tiles)
- Create destructible wall (can be destroyed by DemoBot)
- Add behaviour for player to check other factors besides tile status
    - Is there a destructible wall or a bot in the way?
    - [DONE] Is a TumbleBridge covering a gap, making it accessible?
    - Since this is merely a prototyping playground, these have low priority. Base requirements only include making it playable enough to determine whether the puzzle is good or needs adjusting.