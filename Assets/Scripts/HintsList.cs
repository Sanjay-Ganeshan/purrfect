using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class HintsList
{
	public const int HINTS_PER_LEVEL = 3;
	public const string NO_MORE_HINTS = "Maybe I should think a little harder...";
	public static string[,] ALL_HINTS = new string[10,3] { // c# won't let me make this const
		{	"I wonder what this whistle does?",
			"Come here, kitty!",
			"Can I bring the cat with me\nto the next room?"},
		{	"I can’t get through the bars…\nbut I wonder if my cat can?",
		 	"I wonder what this laser pointer does?",
			"How can I make my cat grab the key?"},
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
			"How can I get my cat to distract\nthat guard next to the second key?\nThere’s so many mirrors..."},
		{	"Looks like I have to get my cat to me\nusing the long way around...",
			"Let me try bouncing off a bunch of mirrors\nto get my laser pointer to go\nwhere I need it to...",
			"Wait, didn't my cat find a\nball of yarn earlier?"
		}
	};
	public const string HINT_TO_BRING_CAT = "Wait! I should bring my cat, too!";
	public static string[] ON_WHISTLE_PICKUP = new string[2] {
		"Hey, I wonder if the cat listens to this whistle!\nCan I get it to come with me to the next room?",
		"Open up your inventory with Q and select the\nwhistle. Hold down your left mouse button to\nblow on the whistle, calling your cat over!"
	};
	public static string[] ON_LASER_POINTER_PICKUP = {
		"Hehe, I hope this cat likes laser pointers!",
		"Take your laser pointer out from your inventory\nand aim at a wall to direct your cat\nto a point on the wall!"
	};
	public const string GUARD_SEES_PLAYER = "Yikes, the guard saw me! I'll try a different way.";
	public static string[] OPENING_NARRATIVE = new string[4] {
		"Where am I?\n(Click the dialogue box to advance)",
		"My head hurts...\nI remember I was walking home,\nand then I heard a cat meowing...",
		"...",
		"I should get out of here..."
	};
	public const string CAT_GOT_KEY = "Awesome! Now I can get the key from the cat.";
	public static string[] PLAYER_GOT_KEY = new string[2] {
		"I think I can use this key on the gate!",
		"Walk up to the gate and press E to use the key."
	};
	public const string INTERACT_GATE_WITHOUT_KEY = "I need to find a key to unlock this first...";
	public const string CANT_GO_BACK = "There's no time to go back!\nI need to get out of here, fast!\nI have homework due tomorrow...";
	public static bool YARN_SAID = false;
	public static string[] YARN = new string[2] {
		"Looks like my cat likes chasing yarn...\nMaybe I can get it to go where\nthe laser can't shine?",
		"(When your cat brings the yarn back to you,\ntry using the yarn when your cat\nis appropriately positioned!)"
	};
	public static bool GUARDS_SAID = false;
	public static string[] GUARDS = new string[2] {
		"If those guards see me,\nI'll have to go back to the entrance...",
		"Maybe I can distract them with my cat\nand sneak around them?"
	};
	public static bool FINAL_NARRATIVE_SAID = false;
	public static string[] FINAL_LEVEL_NARRATIVE = new string[11] {
		"Hohoho... Why hello there, my little mouse.",
		"I see you woke up! But you should stop\nscurrying around, you know?",
		"I set up a small trap to separate you\nfrom your new feline friend...\nwho you won't be seeing anymore.",
		"Be a good little girl and let me fix you up.\nThere's no claws for concern.",
		"Huh?! What are you planning on doing to me?",
		"I'm fixing humeownity one step at a time...",
		"By turning everyone into cats,\nwe'll never have any problems again!",
		"Hehehe!!",
		"(She's crazy! I need to get out of here\nwithout her noticing me...)",
		"And don't even think about escaping!\nI threw away the key,\nand I'll be watching the only exit!",
		"Hahahahahaha!!"
	};
	public static int[] FINAL_LEVEL_NARRATIVE_SAYERS = new int[11] {
		1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1
	};
	public const string WITCH_GIVES_KEY = "Ugh, I forgot about that booby trap.\nNever mind, you still can't get past me!";
	public const string GAME_END_YAY = "I made it! and I even made a new friend, too...";
	public const string TWO_DOORS_HINT = "I hope I can figure out which key\nopens which door...";
	public const string SHOULD_HOLD = "(Hold down the mouse button to\nkeep using the whistle.)";
	public static bool SHOULD_HOLD_SAID = false;
	public const string NO_YARN_PERSIST = "I'm almost there!\n...Shoot, this yarn is completely unraveled.\nGuess I'll have to leave it behind.";
	public const string MIRROR_HINT = "I wonder if I can use that mirror\nin the corner to my advantage?";
}
