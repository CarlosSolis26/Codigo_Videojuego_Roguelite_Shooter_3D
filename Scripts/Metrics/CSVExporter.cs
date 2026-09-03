using System;
using System.IO;
using System.Text;

public static class CSVExporter
{
    private static string folderPath = Path.Combine(
        UnityEngine.Application.persistentDataPath,
        "Metrics"
    );

    private static string filePath = Path.Combine(
        folderPath,
        "GameMetrics.csv"
    );

    public static void Export(MetricsData metrics)
    {
        // Crear carpeta si no existe
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Crear archivo y encabezados si no existe
        if (!File.Exists(filePath))
        {
            CreateHeader();
        }

        // Generar ID de partida
        int gameID = GetNextGameID();

        // Mejoras
        string upgrades = string.Join(
            " | ",
            metrics.upgradesChosen
        );

        // Tiempos por oleada
        string waveTimes = string.Join(
            " | ",
            metrics.waveTimes
        );

        // Crear nueva fila
        StringBuilder row = new StringBuilder();

        row.Append(gameID);
        row.Append(";");
        //row.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //row.Append(";");
        row.Append(metrics.restartCount);
        row.Append(";");
        row.Append(metrics.totalPlayTime.ToString("F2"));
        row.Append(";");
        row.Append(metrics.maxWaveReached);
        row.Append(";");
        row.Append(metrics.zombiesKilled);
        row.Append(";");
        row.Append(metrics.shotsFired);
        row.Append(";");
        row.Append(metrics.shotsHit);
        row.Append(";");
        row.Append(metrics.accuracy.ToString("F2"));
        row.Append(";");
        row.Append(metrics.damageTaken);
        row.Append(";");
        row.Append(metrics.reloads);
        row.Append(";");
        row.Append("\"" + upgrades + "\"");
        row.Append(";");
        row.Append("\"" + waveTimes + "\"");

        // Agregar fila al archivo existente
        File.AppendAllText(
            filePath,
            row.ToString() + Environment.NewLine,
            Encoding.UTF8
        );

        UnityEngine.Debug.Log(
            "Métricas de la partida " + gameID +
            " guardadas correctamente.\n" +
            filePath
        );
    }

    private static void CreateHeader()
    {
        string header =
            "ID_Partida;" +
            //"Fecha," +
            "Reinicios;" +
            "TiempoTotal;" +
            "OleadaMaxima;" +
            "ZombiesEliminados;" +
            "DisparosRealizados;" +
            "DisparosAcertados;" +
            "Precision;" +
            "DanioRecibido;" +
            "Recargas;" +
            "Mejoras;" +
            "TiempoPorOleada" +
            Environment.NewLine;

        File.WriteAllText(
            filePath,
            header,
            Encoding.UTF8
        );
    }

    private static int GetNextGameID()
    {
        if (!File.Exists(filePath))
        {
            return 1;
        }

        string[] lines = File.ReadAllLines(filePath);

        return lines.Length;
    }

    public static string GetMetricsFolder()
    {
        return folderPath;
    }
}
