using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class GameConstants
{
    public const string BTN_USE_CURRENTLY_EQUIPPED_ITEM = "UseItem";
    public const string BTN_DISPLAY_CHARACTER_INVENTORY = "ShowInventory";
    public const string AXIS_MOVE_VERTICAL = "Vertical";
    public const string AXIS_MOVE_HORIZONTAL = "Horizontal";
    public const string BTN_INTERACT = "Interact";
    public const string BTN_SELECT = "UseItem";
    public const string BTN_RESET = "ResetLevel";
    public const string LBL_CAT = "Cat";
	public const string LBL_PLAYER = "Player";
    public const string ZOC_LAYER = "Interactable";
    public const int LASER_PRIORITY = 10;
    public const int WHISTLE_PRIORITY = 5;
    public const int BALL_PRIORITY = 15;
    public const float BALL_SPEED = 2f;
    public const float DEFAULT_REFRACTIVE_INDEX = 1.0f;
    public const string PLAYER_ID = "player";
    public const string CAMERA_ID = "camera";
    public const string LEVEL_PATH = "Levels";
    public const string SAVE_PATH = "Save";
	public const int MAX_CAT_DIST_FOR_LEVEL_END = 1;
	public const string OPENING_NARRATIVE_LEVEL = "level0_Redo";
	public const string BRING_CAT_HINT_LEVEL = "level1_Redo";
	public const string FIRST_GUARD_LEVEL = "level5_Redo";
	public const string FINAL_LEVEL = "level9_Redo";
	public const int CAT_SIGHT_RANGE = 300;
	public const int GUARD_SIGHT_RANGE = 300;
}
