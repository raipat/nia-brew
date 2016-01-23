# Technical Specs #
### So far this is what we have been able to gather as far as specs are concerned ###


**taken from _AiboPet's_ post on the official nia forum:**

What I know so far. The NIA continuously returns data.... I noticed that it produces 55 byte of data 512 time in a second.
That should be 56 bytes per record (but the last byte will always be zero) -- at least using the Windows HID drivers.

From Windows based HID sample app (see DDK HClient and TestVHID)
  * The "HID\_CAPS" attributes:
  * Input report byte length: 56
  * Output report byte length: 9
  * Feature report byte length: 2

The format of the 56 bytes appears to be as follows (with my experimental numbers):

  * Array of 16 sample values, 3 bytes each (usage 0xA). The number of valid samples varies (typically 3 to 9). Unused samples are 0x7A1200 (8000000 decimal)
  * 2 byte value (usage 0xC) - appears to be 0xBD38 most/all of the time
  * 2 byte value (usage 0x1) - appears to fluctuate between 0xFFFC to 0xFFFE
  * 2 byte value (usage 0xB) - timestamp, in samples, useful to see if record capture is
getting behind
  * 2 byte value (usage 0x9) - number of valid samples in this record (0x0003 to 0x0009)

Depending on the method of raw capture, you may get an extra 0 at the start.

The 3 byte (24 bit) sample data appears to be biased by 8000000 (decimal). I don't know how they encode it exactly, including Left vs. Right. I need to find a less RF noisy place to test. BTW: The NIA box has a 24 bit per channel stereo A-to-D converter.

**and another post from _D3adg0d_ on the same forum**

Secondly I noticed that the nia is a smiple HID device, or Human Interface Device. Just like a mouse or keyboard, or the WiiMote if you connect it through bluetooth, or an even better example would be a USB joystick. What that boils down to is that anyone that knows how to program can pull reports from the nia and as long as they know how to decode the report packet they can use it in their program however they want.

This is how HID works:
Every HID device has a report structure for input, output, and feature.
You can send it a output report, receive an input report, or set a feature.
All you have to do to get the values of that report is request it, its part of the HID standard.

So the nia is this:
  * VID: 1234
  * PID: 0000
  * REV: 0001
  * Report size (including report id)
  * Input: 56 bytes
  * Output: 9 bytes
  * Feature: 2 bytes
  * 24-bit biometric sample data array (min-8388608, max:8388607)

So as you can see you can send it a report request of 9 bytes or feature request of 2. Likewise your report will be 56 bytes. I fired up my handy dandy USB HID IO program and viewed the IO of the nia.

As soon as you plug in the nia, it starts streaming data to the PC without even having been sent an output report. Just like a USB joystick. Here are some examples of output:

```
RD 00  88 05 84 87 05 84 AF 03 84 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 38 BD FD FF 69 0C 03 
RD 00  9E 02 84 3E 03 84 EE 01 84 1E 03 84 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 38 BD FD FF 79 0C 04 
RD 00  15 FF 83 35 01 84 1D FD 83 3D 01 84 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 38 BD FD FF 89 0C 04 
RD 00  AC FE 83 04 FF 83 EC FD 83 6D FE 83 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 38 BD FD FF A0 0C 04 
RD 00  C5 FB 83 B6 FA 83 EE FA 83 D6 FB 83 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 00 12 7A 38 BD FD FF B4 0C 04
```


If you count it out you will see 56 bytes in each line. RD 00 is the report ID "00". Then there are 12 bytes of data that are constantly changing (likely the data we want) then there are another 29 bytes of data that rarely change then 14 bytes that seem to be timing.

So what does all this mean you might ask. First you don't need a driver for 64 bit systems or Linux, HID is a standard it doesn't need a driver! I even tested it out I plugged it into my Vista 64bit laptop and fired up USBHIDIO and was able to stream data from the nia just like a 32 bit OS it will be the same for Linux. What we need is the field data for the input stream. If OCZ or whoever would release that then anyone could start to develop stuff for this just like the WiiMote

I haven't really tried to decode the first 24 bytes into fields yet but I might if I get a chance. So check out this page for more HID info - http://www.lvr.com/hidpage.htm this guy already has the framework for VB.net C# VB6 and C++6. Check out SimpleHIDwrite program that will allow you to stream the data and save it as a text file. As well as lots of info on HID stuff.


---


Also here are some files from the NeuroUpdate website that have done some tests and generated various excel charts with captured data:
  * http://www.neuroupdate.com/nia/