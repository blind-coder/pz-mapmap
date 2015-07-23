
all: MapMap.exe MapMapDebug.exe

MapMap.exe: ./MapMap/Program.cs ./MapMap/Main.cs ./MapMap/Properties/AssemblyInfo.cs ./MapMapLib/MMPack.cs ./MapMapLib/MMLua.cs ./MapMapLib/MMCellData.cs ./MapMapLib/MMTextureData.cs ./MapMapLib/MMBinReader.cs ./MapMapLib/MMTextures.cs ./MapMapLib/MMCellReader.cs ./MapMapLib/MMPlotter.cs ./MapMapLib/Properties/AssemblyInfo.cs ./MonoTransparanceBug/transparancy.cs
	mcs -optimize+ -o MapMap.exe /reference:System.Drawing.dll /reference:System.Windows.dll ./MapMap/Program.cs ./MapMap/Main.cs ./MapMapLib/MMPack.cs ./MapMapLib/MMLua.cs ./MapMap/Properties/AssemblyInfo.cs ./MapMapLib/MMCellData.cs ./MapMapLib/MMTextureData.cs ./MapMapLib/MMTextures.cs ./MapMapLib/MMCellReader.cs ./MapMapLib/MMPlotter.cs ./MapMapLib/MMBinReader.cs

MapMapDebug.exe: MapMap.exe
	mcs -optimize- -debug -o MapMapDebug.exe /reference:System.Drawing.dll /reference:System.Windows.dll ./MapMap/Program.cs ./MapMap/Main.cs ./MapMapLib/MMPack.cs ./MapMapLib/MMLua.cs ./MapMap/Properties/AssemblyInfo.cs ./MapMapLib/MMCellData.cs ./MapMapLib/MMTextureData.cs ./MapMapLib/MMTextures.cs ./MapMapLib/MMCellReader.cs ./MapMapLib/MMPlotter.cs ./MapMapLib/MMBinReader.cs

clean:
	rm -f MapMap.exe MapMap.exe.mdb MapMapDebug.exe MapMapDebug.exe.mdb
