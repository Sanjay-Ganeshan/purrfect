using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class HintsList
{
	public const int HINTS_PER_LEVEL = 3;
	public const string NO_MORE_HINTS = "Maybe I should think a little harder...";
	public static string[,] ALL_HINTS = new string[8,3] { // for some reason c# won't let me make this const
		{	"I can’t get through the bars…\nbut I wonder if my cat can?",
		 	"How can I make my cat grab the key?",
			"I wonder if this whistle does anything…"},
		{	"I guess my cat only chases\nthe endpoint of the laser?",
			"Huh, if there’s a wall in the way,\nmy cat can’t see where the laser ends…",
			"What if I move my cat out of the cell first\nso that it’ll be able to see my laser\nwhen I shine it on the mirror?"},
		{	"My cat can’t pass through the windows,\nbut maybe it can still pass through\nthose bars to the side?",
			"How can I get my cat to continue through\nthe spiral toward the key?",
			"My laser will go straight through the windows…\nbut maybe I can still get the cat to move in\nthe right direction and stop at a window?"},
		{	"Hmm, it doesn’t look like I can shine my\nlaser anywhere near the key\nbecause of all those mirrors...",
			"Huh, it looks like my cat will chase the\ntoy ball when I’m not shining my laser.",
			"Hmm, where should I place the ball in relation\nto the cat, to get the cat to chase the\ntoy ball in the direction of the key?"},
		{	"I should probably avoid those scary guards.",
			"I wonder if I can distract the guards\nwith my cat in order to sneak past them.",
			"How can I get my cat to walk into the\nline of sight of that pesky last guard?"},
		{	"I wonder what that button does…",
			"Maybe I should get my cat to\nwalk over the button?",
			"Hey, a new mirror appeared…"},
		{	"Hmm, I don’t think I can get my\ncat to walk past the guard\nbecause of all those mirrors...",
			"How can I get my cat to press the button?",
			"Can I use the toy ball to get my cat\nto distract the guard?"},
		{	"Hmm… which key should I try to get first?",
			"Ugh, do I really have to get past\nall these guards twice, to get\nthe key and come back?",
			"How can I get my cat to distract\nthat guard next to the second key?\nThere’s so many mirrors..."}
	};
	public const string HINT_TO_BRING_CAT = "Wait! I should bring my cat, too!";
}
