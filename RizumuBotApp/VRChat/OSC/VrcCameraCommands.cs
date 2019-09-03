using System;
using System.Collections.Generic;
using System.Text;

namespace RizumuBot.VRChat.OSC
{
    public enum VrcCameraCommands
    {
        None, // Does nothing
        Back, // Moves camera to the balcony above the stairs
        DJ, // Moves camera up behind the DJ
        Stage, // Moves camera to the stage
        Move // Animates camera around the room
    }
}
