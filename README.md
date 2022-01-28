# VVVF Simulator
Simulates VVVF inverter sound on a PC.

# Description
This program is for the C# console app on VisualStudio.<br>

# Term of use
We welcome the work with this program.<br>
Please make sure do next thing<br>
- Post the URL of this GitHub.

Don't do next thing.<br>
- Release modified code in other place. (Except VVVF sound code)
- Give this code to others with not downloading from here.

# Dependencies
## Generating Video
・OpenCV - You can get from NuGet<br>
・System.Drawing.Common - You can get from NuGet<br>
・OpenH264 - You can get from the Internet<br>

### About openH264
You can download from this link.<br>
https://github.com/cisco/openh264/releases<br>
The version which this application uses is `1.8.0`<br>
File name is `openh264-1.8.0-win64.dll.bz2`<br>
After you download and extract the file, you will have `openh264-1.8.0-win64.dll`. Make sure you put it in the same directory as the program .exe file.<br>

## Generating Audio
・There are no dependencies.

## Realtime Audio Generation
・NAudio - You can get from NuGet.

# Functions
## VVVF Audio Generation
This application will export simulated vvvf inverter sound in the `.wav` extension.<br>
The sampling frequency is 192kHz.<br>

## Waveform Video Generation
This application will export video as a `.avi` extension.

## Control stat Video Generation
This application can export video of the control stat values.<br>
The file will be the`.avi` extension. <br>

## Realtime Audio Generation
You can generate the audio in real time and control if the sound increases or decreases in frequency as well as the rate that the frequency increases or decreases. <br>
Key Bindings<br>
```
W - Largest Change in frequency
S - Medium Change in frequency
X - Smallest Change in frequency
B - Brake Toggle between ON/OFF
N - Mascon Toggle between ON/OFF 
R - Reselect vvvf inverter sound
Enter - Exit the program
```

# Parent Project
This program was ported from RPi-Zero-VVVF
https://github.com/JOTAN-0655/RPi-Zero-VVVF
