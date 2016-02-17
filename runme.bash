#!/bin/bash

# The directory tiledef must contain the following files from PZ:
# 0_tiledefinitions.tiles  1_newtiledefinitions.tiles  2_tiledefinitions_erosion.tiles

# -gfxsource points to Erosion.pack and Tiles.pack, specified multiple times

# -mapsource points to the map to display, can be specified multiple times

# -minx -maxx -miny -maxy are used to only show a subset of cells, can only be specified once

# -output specifies the output directory

# -divider specifies into how many pieces a cell is divided on each side
# -divider 2 splits into 2x2 images
# -divider 3 splits into 3x3 images
# etc

# -dolayers 0|1 specifies if all layers are put into a single image. 0: yes 1: no

# Example call using Muldraugh and a Savegame, showing only cell 36x31 into the directory output/ with 2x2 images per cell putting all layers into one image:
mono MapMap.exe \
	-tiledef tiledef/ \
	-gfxsource TexturePacks/Erosion.pack \
	-gfxsource TexturePacks/Tiles.pack \
	-mapsource "~/.local/share/Steam/steamapps/common/ProjectZomboid/projectzomboid/media/maps/Muldraugh, KY/" \
	-mapsource "~/Zomboid/Saves/The First Week/27-01-2016_09-14-37/" \
	-minx 36 -maxx 36 \
	-miny 31 -maxy 31 \
	-output output/ \
	-dolayers 0 \
	-divider 2
