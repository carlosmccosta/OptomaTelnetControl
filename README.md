# OptomaTelnetControl

C# library for controlling Optoma projectors using Telnet.

Features supported:

- Power on / off
- Reset / increase / decrease:
  - Focus
  - Zoom
  - Horizontal / vertical lens shift
- Activate a profile containing the configurations for focus, zoom and lens shift defined in the `Profiles.xml`
- Save / activate a lens memory profile stored in the projector embedded memory
- Lock / unlock the projector lens configurations for avoiding its accidental change using the projector remote
- Change the video input source

API available in [OptomaControl.cs](OptomaTelnetControl/OptomaControl.cs)


## XML configurations

Edit the file [Configuration.xml](OptomaTelnetControl.CLI/xml/Configuration.xml) according to your hardware setup, such as the IP address of the projector.


### Profiles

For allowing easy switch between projection configurations, the OptomaTelnetControl C# library supports profiles, that are defined in the [Profiles.xml](OptomaTelnetControl.CLI/xml/Profiles.xml) file.

This file has a list of `State` configurations, that can be copied from the [State.xml](OptomaTelnetControl.CLI/xml/State.xml) file to a profile present in [Profiles.xml](OptomaTelnetControl.CLI/xml/Profiles.xml) file after configuring each desired projection configurations.

The profile that is activated after startup is defined in the `ActiveProfileIndex` (the first profile is at index 0).

For changing the profile during runtime, call the function below.

```csharp
bool ActivateStoredProfile(int profileNumber);
```


### State

The [State.xml](OptomaTelnetControl.CLI/xml/State.xml) file is necessary for properly using profiles, because the projector does not have absolute encoders and does not provide feedback about the state of the focus, zoom and lens shift.

As such, for moving between profiles states, it is necessary to send individual telnet commands to increase or decrease the current state of each of the adjustable configurations.

As a result, the `State.xml` acts as a relative encoder, which must be reset when using the projector for the first time or when the configurations are lost (which may happen if someone adjusts the configurations using the projector remote, instead of using the C# API of this library).

- After resetting the configurations, you must use the C# API of this library to change the focus, zoom and lens shift instead of using the projector remote, in order to ensure that the values in the `State.xml` file are consistent with the physical state of the projector.
- You can use the lock lens functionality of the projector to avoid accidentally changing the lens configurations using the projector remote.

After every API call that changes the projector configuration of the focus, zoom and lens shift, the `State.xml` file is updated / saved to the hard drive. You do not need to manually save the configurations in your application to update this file.


### State reset

For resetting the focus of the projector, use the projector remote and press on the `focus down` key until it reaches its minimum limit and then edit the [State.xml](OptomaTelnetControl.CLI/xml/State.xml) file and set the `Focus` to `0`.

Alternatively, you can open the `OptomaTelnetControl.CLI` application and press the `I` key.

Then, you can use the `F` key to increase the focus and the `G` key to decrease the focus.

After you reach your desired focus, you can copy the value present in the `Focus` tag of the [State.xml](OptomaTelnetControl.CLI/xml/State.xml) file and paste it into a profile present in [Profiles.xml](OptomaTelnetControl.CLI/xml/Profiles.xml).

For resetting the zoom and lens shift, the process is the same as the focus, but you only need to do their reset if you intend to change the zoom and lens shift of the projector.

If you do not intend to change the zoom and lens shift of the projector, just ensure that the values that you place in the [State.xml](OptomaTelnetControl.CLI/xml/State.xml) and [Profiles.xml](OptomaTelnetControl.CLI/xml/Profiles.xml) are the same and the `LensMemoryProfile` is set to `-1`, for not using the embedded lens memory functionality of the projector.

For resetting the zoom and lens shift, use the projector remote to reset their state by pressing the respective key below until the minimum is reached and then set the respective state to `0` in the [State.xml](OptomaTelnetControl.CLI/xml/State.xml) file.

- For resetting the focus, press the `focus down` key
- For resetting the vertical lens shift, press the `down lens shift` key
- For resetting the horizontal lens shift, press the `left lens shift` key

Alternatively, you can open the `OptomaTelnetControl.CLI` application and use the respective keys that are listed when the application starts (`O` key for resetting zoom and `Y` and `U` to reset lens shift).

For resetting the state using the C# API, use the functions below:

```csharp
bool ResetFocusToClosestProjectionDistance();
bool ResetZoomToSmallestProjectionSize();
bool ResetLensShiftToBottomLeft(); // or individually resetting the vertical and horizontal lens shift using the 2 functions below
bool ResetVerticalLensShiftToBottom();
bool ResetHorizontalLensShiftToLeft();
```

Fo using the projector embedded functionality for resetting the lens shift to the center, use the C# API function:
```csharp
bool RecalibrateLensShiftToCenter();
```

### State repeatability

Keep in mind that only the focus and zoom functionality of the Optoma projectors are accurate and repeatable.

Experimental testing with an Optoma ZU660e projector showed that the lens shift functionality using either slow incremental movement through telnet commands or using the fast embedded lens memory is not repeatable, having drift in relation to the calibrated / saved position.

The same happens with the embedded lens calibration / centering of the lens shift.

If your usage of the Optoma projector requires hight repeatability / calibration accuracy (such as projection mapping for augmented reality applications), it is recommended that you change only the projector focus (and only if really necessary).
