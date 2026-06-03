using UnityEngine;

public static class PartidaGuardada
{
    private const string KEY_EXISTE = "PartidaGuardada";
    private const string KEY_ESCENA = "EscenaGuardada";
    private const string KEY_POSX = "Guardado_PosX";
    private const string KEY_POSY = "Guardado_PosY";
    private const string KEY_POSZ = "Guardado_PosZ";
    private const string KEY_VIDA = "Guardado_Vida";
    private const string KEY_RESTAURANDO = "RestaurandoPartida";

    public static bool Existe()
    {
        return PlayerPrefs.GetInt(KEY_EXISTE, 0) == 1;
    }

    public static string ObtenerEscena()
    {
        return PlayerPrefs.GetString(KEY_ESCENA, "");
    }

    public static void Guardar(string escena, Vector3 posicion, int vida)
    {
        PlayerPrefs.SetInt(KEY_EXISTE, 1);
        PlayerPrefs.SetString(KEY_ESCENA, escena);
        PlayerPrefs.SetFloat(KEY_POSX, posicion.x);
        PlayerPrefs.SetFloat(KEY_POSY, posicion.y);
        PlayerPrefs.SetFloat(KEY_POSZ, posicion.z);
        PlayerPrefs.SetInt(KEY_VIDA, vida);
        PlayerPrefs.Save();
    }

    public static void MarcarRestauracion()
    {
        PlayerPrefs.SetInt(KEY_RESTAURANDO, 1);
        PlayerPrefs.Save();
    }

    public static bool EstaRestaurando()
    {
        return PlayerPrefs.GetInt(KEY_RESTAURANDO, 0) == 1;
    }

    public static void LimpiarRestauracion()
    {
        PlayerPrefs.SetInt(KEY_RESTAURANDO, 0);
        PlayerPrefs.Save();
    }

    public static void Cargar(out Vector3 posicion, out int vida)
    {
        float x = PlayerPrefs.GetFloat(KEY_POSX, 0f);
        float y = PlayerPrefs.GetFloat(KEY_POSY, 0f);
        float z = PlayerPrefs.GetFloat(KEY_POSZ, 0f);
        posicion = new Vector3(x, y, z);
        vida = PlayerPrefs.GetInt(KEY_VIDA, 3);
    }

    public static void Limpiar()
    {
        PlayerPrefs.SetInt(KEY_EXISTE, 0);
        PlayerPrefs.Save();
    }
}
