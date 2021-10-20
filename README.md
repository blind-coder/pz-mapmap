# pz-mapmap
A commandline tool to convert Project Zomboid map data / savegames into PNG files

## Setup
- Download and extract MapMap.exe into its own directory on a volume with a LOT of space. You're going to need it. This tool needs to run on Windows as mono on Linux has issues with transparency when creating image files.
- Copy the entire `texturepacks` directory next to MapMap.exe
- Create a directory `Mapname_lotpack` and copy all files of the map into it
- Create a directory `Mapname_output`
Your setup should look like this now:

```
MapMap.exe
texturepacks/
├── ApCom_old.pack
├── ApCom.pack
├── ApComUI.pack
├── Characters.pack
├── Erosion.pack
├── IconsMoveables.pack
├── JumboTrees1x.pack
├── JumboTrees2x.pack
├── Mechanics.pack
├── Overlays1x.pack
├── Overlays2x.pack
├── RadioIcons.pack
├── Tiles1x.floor.pack
├── Tiles1x.pack
├── Tiles2x.floor.pack
├── Tiles2x.pack
├── UI2.pack
├── UI.pack
└── WeatherFx.pack
Mapname_lotpack/
├── 1_1.lotheader
├── 2_1.lotheader
├── chunkdata_1_1.bin
├── chunkdata_2_1.bin
├── map.info
├── objects.lua
├── spawnpoints.lua
├── thumb.png
├── world_1_1.lotpack
└── world_2_1.lotpack
Mapname_output/
```

Now run the command:
```
MapMap.exe -gfxsource TexturePacks\Erosion.pack -gfxsource TexturePacks\Tiles2x.pack -gfxsource TexturePacks\Tiles2x.floor.pack -gfxsource TexturePacks\ApCom.pack -gfxsource texturepacks\RadioIcons.pack -gfxsource TexturePacks\ApComUI.pack -mapsource Mapname_lotpack -output Mapname_output\ -dolayers true -divider 4 -maxthreads 2
```

A lot of png files will be created in the `Mapname_output` directory, occupying - depending on the size of your map - possibly hundreds of GB of space. For reference, the main Project Zomboid map occupies 461 GB after this.
When you are done with this, you need to head over to the deepzoom-utils repository (https://github.com/blind-coder/deepzoom-utils) and continue there.


