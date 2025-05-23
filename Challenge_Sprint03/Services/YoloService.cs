using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;

public class Detection
{
    public float X, Y, Width, Height, Confidence;
    public string Label;
}

public class YoloService
{
    private readonly InferenceSession _session;
    private const int IMG_SIZE = 640;
 
    private readonly string[] classNames = { "Caries", "Cavity", "Crack", "Tooth" };

    public YoloService(string onnxPath = "best.onnx")
    {
        _session = new InferenceSession(onnxPath);
    }

    public string ProcessImage(string base64Image)
    {
        // 1) Decode base64 para Mat 
        var imgBytes = Convert.FromBase64String(base64Image);
        using var ms = new MemoryStream(imgBytes);
        using var src = Mat.FromStream(ms, ImreadModes.Color);

        // 2) Letterbox 
        var (img, scale, padW, padH) = Letterbox(src, IMG_SIZE);

        // 3) Monta tensor [1,3,IMG_SIZE,IMG_SIZE] em RGB/255
        var input = new DenseTensor<float>(new[] { 1, 3, IMG_SIZE, IMG_SIZE });
        for (int y = 0; y < IMG_SIZE; y++)
            for (int x = 0; x < IMG_SIZE; x++)
            {
                var p = img.At<Vec3b>(y, x);
                input[0, 0, y, x] = p[2] / 255f; // R
                input[0, 1, y, x] = p[1] / 255f; // G
                input[0, 2, y, x] = p[0] / 255f; // B
            }

        // 4) Inferência ONNX 
        var inputs = new[] { NamedOnnxValue.CreateFromTensor("images", input) };
        using var results = _session.Run(inputs);
        var output = results.First().AsTensor<float>();  

        int detCount = output.Dimensions[1];
        var detections = new List<Detection>(detCount);

        // 5) Parse e desfaz letterbox para coordenadas originais
        for (int i = 0; i < detCount; i++)
        {
            float x1 = (output[0, i, 0] - padW) / scale;
            float y1 = (output[0, i, 1] - padH) / scale;
            float x2 = (output[0, i, 2] - padW) / scale;
            float y2 = (output[0, i, 3] - padH) / scale;
            float score = output[0, i, 4];
            int cls = (int)output[0, i, 5];

            if (score <= 0)
                continue;

            // Valida índice de classe
            string label = (cls >= 0 && cls < classNames.Length) ? classNames[cls] : cls.ToString();

            detections.Add(new Detection
            {
                X = x1,
                Y = y1,
                Width = x2 - x1,
                Height = y2 - y1,
                Confidence = score,
                Label = label
            });
        }

        // 6) Desenha caixas com cores distintas e labels formatadas
        foreach (var d in detections)
        {
            var rect = new Rect((int)d.X, (int)d.Y, (int)d.Width, (int)d.Height);
            var color = GetColor(d.Label);
            Cv2.Rectangle(src, rect, color, 2);
            Cv2.PutText(src, $"{d.Label} {d.Confidence:0.00}",
                        new Point(d.X, d.Y - 10),
                        HersheyFonts.HersheySimplex, 0.5, color, 1);
        }

        
        return MatToBase64(src);
    }

    // Colors BGR: Cárie=Vermelho, Saudável=Verde, Restauração=Azul, Outros=Ciano
    // Colors BGR: Caries=Vermelho, Cavity=Amarelo, Crack=Laranja, Tooth=Verde, Outros=Ciano
    private static Scalar GetColor(string label) => label switch
    {
        "Caries" => new Scalar(0, 0, 255),    // vermelho
        "Cavity" => new Scalar(0, 255, 255),  // amarelo
        "Crack" => new Scalar(0, 165, 255),   // laranja
        "Tooth" => new Scalar(0, 255, 0),     // verde
        _ => new Scalar(255, 255, 0)           // ciano para desconhecidos
    };

    private static (Mat resized, float scale, int padW, int padH) Letterbox(Mat src, int newSize)
    {
        int w = src.Width, h = src.Height;
        float scale = Math.Min((float)newSize / w, (float)newSize / h);
        int newW = (int)(w * scale), newH = (int)(h * scale);
        var resized = new Mat();
        Cv2.Resize(src, resized, new Size(newW, newH));

        int padW = (newSize - newW) / 2;
        int padH = (newSize - newH) / 2;
        var padded = new Mat(newSize, newSize, MatType.CV_8UC3, new Scalar(114, 114, 114));
        resized.CopyTo(new Mat(padded, new Rect(padW, padH, newW, newH)));

        return (padded, scale, padW, padH);
    }

    private static string MatToBase64(Mat mat)
    {
        Cv2.ImEncode(".jpg", mat, out var buf);
        return Convert.ToBase64String(buf);
    }
}
