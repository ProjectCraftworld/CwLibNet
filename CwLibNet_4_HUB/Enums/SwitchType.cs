using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Enums;

public enum SwitchType
{
    // INVALID(-1)
    INVALID = -1,
    // BUTTON(0)
    BUTTON,
    // LEVER(1)
    LEVER,
    // TRINARY(2)
    TRINARY,
    // PROXIMITY(3)
    PROXIMITY,
    // KEY(4)
    KEY,
    // STICKER(5)
    STICKER,
    // GRAB(6)
    GRAB,
    // PRESSURE(7)
    PRESSURE,
    // PAINT(8)
    PAINT,
    // CONTROLLER_BINARY(9)
    CONTROLLER_BINARY,
    // CONTROLLER_ANALOG(10)
    CONTROLLER_ANALOG,
    // SACKBOT_KEY(11)
    SACKBOT_KEY,
    // SACKBOT_PLAYER(12)
    SACKBOT_PLAYER,
    // SACKBOT_BOT(13)
    SACKBOT_BOT,
    // AND(14)
    AND,
    // COUNTDOWN(15)
    COUNTDOWN,
    // TIMER(16)
    TIMER,
    // TOGGLE(17)
    TOGGLE,
    // IMPACT(18)
    IMPACT,
    // RANDOM(19)
    RANDOM,
    // DIRECTION(20)
    DIRECTION,
    // OR(21)
    OR,
    // XOR(22)
    XOR,
    // NOT(23)
    NOT,
    // NOP(24)
    NOP,
    // MOISTURE(25)
    MOISTURE,
    // INACTIVE(26)
    INACTIVE,
    // SIGN_SPLIT(27)
    SIGN_SPLIT,
    // ALWAYS_ON(28)
    ALWAYS_ON,
    // ANIMATIC(29)
    ANIMATIC,
    // SCORE(30)
    SCORE,
    // DEATH(31)
    DEATH,
    // CUTSCENE_CAM_FINISHED(32)
    CUTSCENE_CAM_FINISHED,
    // CUTSCENE_CAM_ACTIVE(33)
    CUTSCENE_CAM_ACTIVE,
    // MAGIC_MOUTH(34)
    MAGIC_MOUTH,
    // SELECTOR(35)
    SELECTOR,
    // MICROCHIP(36)
    MICROCHIP,
    // CIRCUIT_BOARD(37)
    CIRCUIT_BOARD,
    // CONTROL_PAD(38)
    CONTROL_PAD,
    // PROJECTILE(39)
    PROJECTILE,
    // CIRCUIT_NODE(40)
    CIRCUIT_NODE,
    // ANGLE(41)
    ANGLE,
    // VELOCITY_LINEAR(42)
    VELOCITY_LINEAR,
    // VELOCITY_ANGULAR(43)
    VELOCITY_ANGULAR,
    // MOVINATOR_PAD(44)
    MOVINATOR_PAD,
    // MOTION_RECORDER(45)
    MOTION_RECORDER,
    // FILTER(46)
    FILTER,
    // COLOUR_GATE(47)
    COLOUR_GATE,
    // WAVE_GENERATOR(48)
    WAVE_GENERATOR,
    // EMITTEE(49)
    EMITTEE,
    // POCKET_ITEM(50)
    POCKET_ITEM,
    // POSE(51)
    POSE,
    // ADVANCED_COUNTDOWN(52)
    ADVANCED_COUNTDOWN,
    // STATE_SENSOR(53)
    STATE_SENSOR,
    // KEY_REMOTE(54)
    KEY_REMOTE,
    // VITA_PAD(55)
    VITA_PAD,
    // TWEAK_TRIGGER(56)
    TWEAK_TRIGGER,
    // WORM_HOLE(57)
    WORM_HOLE,
    // QUEST(58)
    QUEST,
    // GRID_MOVER(59)
    GRID_MOVER,
    // QUEST_SENSOR(60)
    QUEST_SENSOR,
    // ADVENTURE_ITEM_GETTER(61)
    ADVENTURE_ITEM_GETTER,
    // SHARDINATOR(62)
    SHARDINATOR,
    // @Deprecated
    // POCKET_ITEM_DISPENSER(63)
    POCKET_ITEM_DISPENSER,
    // POCKET_ITEM_PEDESTAL(64)
    POCKET_ITEM_PEDESTAL,
    // DATA_SAMPLER(65)
    DATA_SAMPLER,
    // TREASURE_SLOT(66)
    TREASURE_SLOT,
    // STREAMING_HINT(67)
    STREAMING_HINT,
    // JOINT_POSITION(68)
    JOINT_POSITION,
    // SLIDE(69)
    SLIDE,
    // IN_OUT_MOVER(70)
    IN_OUT_MOVER,
    // SAVE_CHIP(71)
    SAVE_CHIP,
    // PLATFORM_SENSOR(72)
    PLATFORM_SENSOR,
    // RACE_END(73)
    RACE_END,
    // ANTI_STREAMING(74)
    ANTI_STREAMING,
    // GAME_LIVE_STREAMING_CHOICE(75)
    GAME_LIVE_STREAMING_CHOICE,
    // SEARCHLIGHT(76)
    SEARCHLIGHT,
    // POPIT_CURSOR_SENSOR(77)
    POPIT_CURSOR_SENSOR,
    // PROGRESS_BOARD(78)
    PROGRESS_BOARD,
    // BOUNCER(79)
    BOUNCER,
    // KILL_TWEAKER(80)
    KILL_TWEAKER,
    // POWERUP_TWEAKER(81)
    POWERUP_TWEAKER,
    // RACE_START(82)
    RACE_START,
    // DECORATION_MOUNT(83)
    DECORATION_MOUNT,
    // SPRING_SENSOR(84)
    SPRING_SENSOR
}