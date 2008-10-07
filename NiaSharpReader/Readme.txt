-----------
GenericHid:
-----------
A standalone verision of the generic hid library availaible from
http://www.lvr.com/hidpage.htm


---------
ZedGraph:
---------
A charting library from
http://zedgraph.org


---------------
NiaSharpReader:
---------------

The form code itself has been reduced to it's strict minimum as far as interactions
with the generic hidlibrary goes, and the way the data is interpreted and charted comes
from the hacked niawiimote app & excel files at http://www.neuroupdate.com/nia/


All in all the output seems pretty much ok, even if the app itself still has some bugs.


The code "should" work on win98/ME/XP/Vista, including 32bit & 64bit versions
..as long as you have the .NET 2.0 framework installed


If you want to mess with the source-code and don't have Visual Studio 2008, you can grab
the free "Visual C# 2008 Express Edition" here: http://www.microsoft.com/express/download/


If you don't have a NIA or just want to play with some sample data, place "recording.dat"
in the same folder as the compiled NiaSharpReader.exe

It is a serialized "System.Collections.Generic.List<byte[]>" containing the raw packets
in the order they where captured.


-------------------
Special Credits to:
-------------------
AiboPet
Neuroupdate
D3adg0d
and of course the makers of the above libraries.

All, without whom none of this would have been possible.