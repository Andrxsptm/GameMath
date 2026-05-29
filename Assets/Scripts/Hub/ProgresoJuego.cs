using UnityEngine;

public class ProgresoJuego : MonoBehaviour
{
    public static int nivelDesbloqueado = 1;

    public static void DesbloquearSiguienteNivel(int nivel)
    {
        if (nivel > nivelDesbloqueado)
        {
            nivelDesbloqueado = nivel;
            PlayerPrefs.SetInt("NivelDesbloqueado", nivelDesbloqueado);
            PlayerPrefs.Save();
        }
    }

    public static void CargarProgreso()
    {
        nivelDesbloqueado = PlayerPrefs.GetInt("NivelDesbloqueado", 1);
    }

    public static void ReiniciarProgreso()
    {
        nivelDesbloqueado = 1;
        PlayerPrefs.SetInt("NivelDesbloqueado", 1);
        PlayerPrefs.Save();
    }

    public static string ObtenerUltimoNivel()
    {
        switch (nivelDesbloqueado)
        {
            case 1: return "nivel1";
            case 2: return "Lvl1";
            case 3: return "lvl2";
            case 4: return "Lvl3";
            default: return "nivel1";
        }
    }


#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    static void ReiniciarEnEditor()
    {
        PlayerPrefs.SetInt("NivelDesbloqueado", 1);
        PlayerPrefs.Save();
    }
#endif
}