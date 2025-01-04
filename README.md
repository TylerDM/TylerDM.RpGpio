# Introduction
RpGpio provides a more opinionated wrapper over the GPIO APIs present in System.Device.Gpio which are purpose built for use with modern (40 pin) Raspberry Pi hardware.  

Features:
- Prevents invalid states such as opening a pin in read mode, then writing to it.
- Provide DI support out of the box.
- Add abstraction classes that allow rapid use of common hardware.
- Abstract away if you're using Input, InputPullUp, or InputPullDown.

# Devices
Wrapper classes are provided for common hardware.  Check the Devices folder to see all currently available device wrappers.

Examples of currently implemented devices:
- Button
  - Prevent double events caused by noise inherent when using Input and InputPullDown mode.
  - Measures the amount of time a button was held down.
  - Provides an API surface that abstracts if (or if any) pull up or down resistors are used.
- Keypad
  - Allows you to quickly make use of any grid based keypad.
  - Provides built in support for the Parallax 4x4.
  - Prevents invalid states caused by hardware noise.

Other devices as of time of writing:
- Buzzers
- LEDs
- Pulsers
- Relays