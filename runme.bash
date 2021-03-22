#!/bin/bash

# The directory given as a parameter to -tiledef must contain the following files from PZ:
# 0_tiledefinitions.tiles  1_newtiledefinitions.tiles  2_tiledefinitions_erosion.tiles 3_tiledefinitions_apcom.tiles

# -gfxsource points to Erosion.pack and Tiles.pack, specified multiple times

# -mapsource points to the map to display, can be specified multiple times

# -minx -maxx -miny -maxy are used to only show a subset of cells, can only be specified once

# -output specifies the output directory. A / or \ must be at the end of the parameter

# -divider specifies into how many pieces a cell is divided on each side
# -divider 2 splits into 2x2 (4) images
# -divider 3 splits into 3x3 (9) images
# ...
# -divider 300 splits into 300x300 (90.000!) images.

# -dolayers true|false specifies if layers are split across multiple PNG files.
# true: One image will be created for each layer. You will be able to look inside houses.
# false: One image will be created containing all layers. You will not be able to look inside houses.

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
	-dolayers false \
	-divider 2
