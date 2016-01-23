## The OCZ NIA™ is a innovative bci device which let's users control games with their mind. ##

Even more, the NIA™ is actually a basic EMG-ECG-EEG-EOG with a $150 price mark, which makes it one of the cheapest around.

This project is dedicated to bringing new ways to use the OCZ NIA™ by supplying information about how it works & code to use it in other interesting ways.


---

So far we have a PartsList, some TechnicalSpecs, and a [Reader](http://nia-brew.googlecode.com/files/nia_ReaderV1.6.1.zip) made by _D3adg0d_, that connects to the usb stream to show some information about what's going on.

NeuroUpdate also did some tests and made a modified version of the [WiiMoteLib](http://www.neuroupdate.com/nia/) that dumps some data to a excel sheet.

**Update:**
Added a [test app](http://nia-brew.googlecode.com/files/NiaSharpReader_binsrc.zip) in C# with sourcecode that should _theoretically_ work on x64.


---

### Current Todo list ###
  * Figure out how to correctly interpret the USB data
  * Maybe write up a nice documentation page on how to actually use the NIA
  * Get some code working with [NeuroServer](http://openeeg.sourceforge.net/doc/sw/NeuroServer/) so we can use it correctly with [BrainBay](http://www.shifz.org/brainbay/) or similar software
  * And then publish some libs/docs in various languages, so others can start doing the same