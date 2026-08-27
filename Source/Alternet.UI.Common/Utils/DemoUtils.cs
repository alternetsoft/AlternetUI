using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Alternet.UI
{
    /// <summary>
    /// Contains utility methods used in demo applications.
    /// </summary>
    public static class DemoUtils
    {
        public static string LoremIpsum =
"Beneath a sky stitched with teacup clouds, the girl tiptoed across checkerboard moss. " +
"Each step made a peculiar sound—like libraries whispering to mushrooms. " +
"Trees bent inward to eavesdrop, their leaves rustling riddles only crickets could decipher." +
Environment.NewLine + Environment.NewLine +
"The map she carried was drawn entirely in nonsense, but somehow it felt correct. " +
"It pulsed faintly in her hands, humming with ink made from stolen dreams and marmalade." +
Environment.NewLine + Environment.NewLine +
"“Left is usually right,” said the rabbit-shaped shadow, bowing courteously. " +
"“Unless, of course, you're upside-down.”" +
Environment.NewLine + Environment.NewLine +
"And so, with a smile too wide for logic, she stepped forward—into a world where clocks " +
"melted politely and hats outgrew heads.";

        public const string LoremIpsumSmall =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. " +
            "Suspendisse tincidunt orci vitae arcu congue commodo. " +
            "Proin fermentum rhoncus dictum.\n";

        public const string LoremIpsumSmallSingleLine =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
            "Suspendisse tincidunt orci vitae arcu congue commodo. " +
            "Proin fermentum rhoncus dictum.";

        public const string LoremIpsumSmallThreeLines =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n" +
            "Suspendisse tincidunt orci vitae arcu congue commodo.\n" +
            "Proin fermentum rhoncus dictum.";
    }
}
