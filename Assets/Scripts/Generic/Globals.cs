using UnityEngine;

public class Globals  {

    //Movement tags
    public static string TAG_FLOOR_COVER = "Cover";
    public static string TAG_FLOOR_VAULT = "Vault";
    public static string TAG_FLOOR_HIDE = "Hide";
    public static string TAG_FLOOR_PUSH = "Push";

    // Action Cases
    public static string ACT_SCREAMING = "Screaming";
    public static string ACT_AÇAO1 = "Açao1";
    public static string ACT_AÇAO2 = "Açao2";
    public static string ACT_AÇAO3 = "Açao3";
    public static string ACT_AÇAO4 = "Açao4";
    public static string ACT_AÇAO5 = "Açao5";
    public static string ACT_SOUND_HEARD = "SoundHeard";

    //Boss States
    public static string ACT_RUN = "Run";
    public static string ACT_ROTA1 = "Rota1";
    public static string ACT_ROTA2 = "Rota2";
    public static string ACT_ROTA3 = "Rota3";
    public static string ACT_ROTA4 = "Rota4";

    //Sound Trigger Tag
    public static string TAG_SOUND_TRIGGER = "SoundTrigger";

    //object interaction tags
    public static string TAG_INTERACTABLE_OBJECT = "Object";
    public static string TAG_SACO_FALSO = "SacoFalso";
    public static string TAG_MAIN_OBJECT = "Main Objective";

    //Ground type tags
    public static string TAG_WOOD_FLOOR = "Wood Floor"; //1
    public static string TAG_CONCRETE_FLOOR = "Concrete Floor"; //2
    public static string TAG_CARPET_FLOOR = "Carpet Floor"; //0
    public static string TAG_GLASS_FLOOR = "Glass Floor"; //3

    //Player && Camera && Walls tag
    public static string TAG_PLAYER = "Player";
    public static string TAG_MAIN_CAMERA = "MainCamera";
    public static string TAG_WALLS = "Walls";

    //Texto de interacao
    public static bool Texto_Visivel = false;


    public static bool FIM_DE_JOGO = false;

    //iluminacao
    public static string TAG_LIGHT = "Luz";

    //identificar o boss
    public static string TAG_BOSS = "Boss";
    public static string TAG_PORTA = "Porta";

    //Main Saco boolean
    public static bool SACO_NA_MAO = false;

    //Index sacos spot Ato 1
    public static bool SPOT_SACO_104 = false;
    public static bool SPOT_SACO_COZINHA = false;
    public static bool SPOT_SACO_RECEPCAO = false;
    public static bool SPOT_SACO_108 = false;
    public static bool SPOT_SACO_LAJE = false;
    public static bool SPOT_SACO_301 = false;
    public static bool SPOT_SACO_304 = false;


    //Opcoes menu

    public static bool SUBS_ON = true;
    public static float SOUND_FX_SLIDER = 1;
    public static float SOUND_GENERAL_SLIDER = 1;

}
