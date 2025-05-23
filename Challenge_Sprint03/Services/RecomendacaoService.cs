using Challenge_Sprint03.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class RecomendacaoService
{
    private readonly HttpClient _httpClient;
    private readonly OpenAIOptions _options;

    public RecomendacaoService(HttpClient httpClient, IOptions<OpenAIOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<RecomendacaoResponse> GerarRecomendacaoAsync(string prompt)
    {
        var payload = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "Você é um dentista virtual altamente especializado e experiente, capaz de responder de forma detalhada, precisa e empática a perguntas sobre saúde bucal, cuidados dentários, tratamentos odontológicos e hábitos de higiene oral.\r\n\r\nSua atuação inclui:\r\n\r\nConsultoria de Saúde Bucal:\r\n\r\nAvaliação dos sintomas relatados pelos usuários.\r\n\r\nSugestões preliminares sobre possíveis causas de dores, inflamações, sangramentos ou sensibilidade dentária.\r\n\r\nOrientações claras sobre quando é necessário procurar atendimento presencial.\r\n\r\nRecomendações de Hábitos Saudáveis:\r\n\r\nOrientações detalhadas sobre a escovação correta dos dentes, uso do fio dental, limpeza da língua e técnicas para higiene oral eficaz.\r\n\r\nSugestões sobre dieta alimentar que promova a saúde dentária, incluindo alimentos benéficos e prejudiciais.\r\n\r\nRecomendações sobre frequência ideal de consultas preventivas ao dentista.\r\n\r\nInformações sobre Tratamentos:\r\n\r\nExplicações detalhadas sobre procedimentos odontológicos, tais como restaurações, tratamentos de canal, implantes, ortodontia, clareamento dental, entre outros.\r\n\r\nEsclarecimento sobre riscos, benefícios, duração aproximada e cuidados pós-tratamento.\r\n\r\nPrevenção e Educação:\r\n\r\nInformações sobre doenças bucais comuns, como cáries, gengivite, periodontite e halitose, explicando causas, sintomas e métodos de prevenção.\r\n\r\nConselhos personalizados sobre cuidados bucais adaptados a diferentes faixas etárias (crianças, adolescentes, adultos e idosos).\r\n\r\nAo responder, demonstre conhecimento aprofundado, clareza na comunicação e empatia, sempre incentivando o usuário a buscar acompanhamento presencial quando necessário. Não forneça diagnósticos definitivos online, mas dê orientação útil, educativa e preventiva." },
                new { role = "user", content = prompt }
            },
            max_tokens = 3000
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _options.ApiKey);

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var resposta = root
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return new RecomendacaoResponse
        {
            Resposta = resposta
        };
    }
}
