# 🚀 Challenge Sprint 04 API

Este repositório contém a API desenvolvida para o desafio da Sprint 04, utilizando **ASP.NET Core Web API**, **Oracle** como banco de dados e diversos padrões de design e boas práticas de arquitetura de software.

---

## 👥 Integrantes do Grupo

- Rafael de Novaes – RM553934 – 2TDSPC  
- Fabiola Falcão – RM552715 – 2TDSPC  
- Carlos Henrique Furtado Nascimento – RM553597 – 2TDSPR  

---

## 🎥 Demonstração em Vídeo

Confira a demonstração completa da API no YouTube:  
[![Assista no YouTube](https://img.youtube.com/vi/p4MSzIcygHc/0.jpg)](https://www.youtube.com/watch?v=p4MSzIcygHc)


## 🛠️ Arquitetura e Design

### 🔗 Arquitetura

A API foi desenvolvida utilizando uma arquitetura em camadas, que separa as responsabilidades em:

- **Controllers:** Recebem requisições HTTP, validam dados e retornam respostas usando DTOs.
- **Services:** Contêm a lógica de negócio, operações CRUD e mapeamento entre entidades e DTOs.
- **Repositories:** Realizam acesso ao banco de dados através do Entity Framework Core.
- **Models:** Representam as entidades do domínio (**Habito**, **RegistroHabito**, **Unidade**, **Usuario**).

### 📐 Padrões de Design

- **Repository Pattern**  
- **Singleton** *(SettingsService)*  
- **Data Transfer Object (DTO)**  
- **Injeção de Dependência (Dependency Injection)**  

---

## ✅ Aplicação de SOLID e Clean Code

A API foi projetada seguindo as boas práticas de **Clean Code** e princípios **SOLID**, conforme descrito abaixo:

### 🧱 SOLID

- **Single Responsibility Principle (SRP):**  
  Cada classe possui uma única responsabilidade.  
  *Exemplo:* `HabitoService` é responsável apenas pela lógica de negócios relacionada a hábitos.

- **Open/Closed Principle (OCP):**  
  O sistema está aberto para extensão e fechado para modificação.  
  *Exemplo:* novos serviços como `YOLOService` e `OpenAIService` foram adicionados sem alterar a estrutura dos serviços existentes.

- **Liskov Substitution Principle (LSP):**  
  As abstrações, como `IRepository<T>`, podem ser substituídas por outras implementações, sem impactar a aplicação.

- **Interface Segregation Principle (ISP):**  
  Criamos interfaces específicas como `IRegistroHabitoRepository` e `IUsuarioRepository`, evitando contratos genéricos excessivamente grandes.

- **Dependency Inversion Principle (DIP):**  
  Os serviços dependem de abstrações, não de implementações concretas.  
  *Exemplo:* `HabitoService` depende da interface `IRepository<Habito>`, e não diretamente de `Repository<Habito>`.

### ✨ Clean Code

- Código legível, com nomes claros e objetivos.  
- Separação entre camadas: Controllers, Services, Repositories, Models e DTOs.  
- Métodos pequenos e coesos.  
- Uso de boas práticas REST, com retornos apropriados (`200 OK`, `201 Created`, `404 NotFound`, etc.).  
- Controllers sem lógica de negócio — responsáveis apenas pela orquestração das chamadas de serviço.

---

## 🔗 Integração com Serviço Externo

Foi implementada a integração com a API da **Brevo** (antiga Sendinblue), um serviço externo de envio de e-mails via **RESTful API**.

Quando um usuário é cadastrado na API, um e-mail transacional é enviado confirmando a realização do cadastro.


---

## 🧠 Integração com Machine Learning (ML.NET e ONNX)

Para atender ao requisito de integrar Machine Learning:

### ✔️ YOLOv8 com ONNX

Foi implementado o serviço `YoloService`, que realiza **detecção de objetos** (cáries, rachaduras, etc.) em imagens odontológicas:

- Modelo **YOLOv8** treinado e exportado em **ONNX**.  
- Processamento de imagem com **OpenCV** e **Microsoft.ML.OnnxRuntime**.  
- **Entrada:** imagem em **Base64**.  
- **Saída:** imagem anotada com caixas delimitadoras e labels.

*Exemplo:* detecção automática de **cáries** e **rachaduras** em imagens enviadas pelo usuário.

---

## 🤖 Integração com AI Generativa

Além da detecção de imagens, também foi implementada a integração com ChatGPT, usando a API oficial via **REST**.


### ✔️ RecomendacaoService

- Especializado em gerar **recomendações personalizadas** de cuidados odontológicos.  

---

## 🧪 Testes Automatizados

O projeto conta com testes organizados da seguinte forma:

---

### ✅ 1. Estrutura de Testes

O projeto segue uma divisão clara entre:

### ✔️ Testes de Unidade (Unit Tests)

**Objetivo:**  
Garantir que cada **componente isolado** do sistema funciona corretamente, sem depender de banco de dados ou serviços externos.

**Como foram implementados:**

- Utilização do **Moq** para criar implementações falsas das interfaces:
  - `IRepository<T>`
  - `IRegistroHabitoRepository`
- Garantem que os **métodos de serviço** estão chamando corretamente os métodos do repositório.
- Simulam cenários de **sucesso** e **falhas**.

**Exemplos de testes:**

- `CreateHabitoAsync` → verifica se chama `AddAsync`.
- `UpdateUsuarioAsync` → lança exceção quando usuário não encontrado.
- `UpdatePontosAsync` → confirma soma correta dos pontos.

**Benefícios:**

✅ Execução rápida  
✅ Foco em lógica isolada  
✅ Fácil manutenção



### ✔️ Testes de Integração (Integration Tests)

**Objetivo:**  
Verificar a **integração real** entre as classes: Service + Repository + DbContext.

**Como foram implementados:**

- Utilização do **EF Core InMemory**, criando um **banco temporário** e isolado para cada teste.
- Verificam a **persistência** dos dados e **integridade** nas operações CRUD.

**Exemplos de testes:**

- `CreateHabitoAsync_DeveSalvarHabitoNoBanco` → garante persistência de hábito.
- `DeleteUnidadeAsync_DeveRemoverUnidadeDoBanco` → garante remoção correta.
- `CreateUsuarioAsync_ComEmailDuplicado_DeveLancarExcecao` → valida regras de negócio.

**Benefícios:**

✅ Testam o fluxo completo: Service + Repository + Contexto  
✅ Detectam falhas de configuração no mapeamento de entidades  
✅ Sem necessidade de banco Oracle real

---

### ✅ 2. Tecnologias utilizadas

| Tecnologia                                  | Utilização                                 |
|---------------------------------------------|--------------------------------------------|
| **xUnit**                                   | Estrutura de testes                        |
| **Moq**                                     | Mocking de dependências para Unit Tests    |
| **Microsoft.EntityFrameworkCore.InMemory**  | Banco em memória para Integration Tests    |

---


### ✅ 4. Como rodar os testes

No terminal, na raiz do projeto:

```bash
dotnet test

## 💻 Tecnologias Utilizadas

- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core (Oracle)**
- **Swagger/OpenAPI**

---

## ✅ Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Oracle Database](https://www.oracle.com/database/)

---

## 🚦 Como Rodar Localmente

### 1️⃣ Clone o repositório

```bash
git clone https://github.com/SeuUsuario/Challenge_Sprint04.git
cd Challenge_Sprint04
```

### 2️⃣ Configure as credenciais no AppSettings.json*

---

### 3️⃣ Restaurar pacotes NuGet

```bash
dotnet restore
```

---

### 4️⃣ Aplicar Migrations

```bash
dotnet ef migrations add InitialMigration
dotnet ef database update
```

---

### 5️⃣ Executar a aplicação

```bash
dotnet run
```

---

🔗 API disponível em: [`https://localhost:5001`](https://localhost:5001)

📘 Swagger: [`https://localhost:5001/swagger`](https://localhost:5001/swagger)

---

## 📌 Exemplos de Testes

### ▶️ Habitos *(GET)*

```json
[
  {
    "habitoId": 1,
    "descricao": "Exercícios diários",
    "tipo": "Saúde",
    "frequenciaIdeal": 1
  }
]
```

### ▶️ RegistroHabito *(POST)*

```json
{
  "habitoId": 1,
  "imagem": "https://exemplo.com/imagem.jpg",
  "observacoes": "Realizado com sucesso"
}
```

### ▶️ Unidades *(PUT)*

```json
{
  "unidadeId": 1,
  "nome": "Unidade Central",
  "estado": "SP",
  "cidade": "São Paulo",
  "endereco": "Av. Paulista, 1500"
}


---

## 📁 DDL das Tabelas

```sql
CREATE TABLE Habito (
    HabitoId NUMBER PRIMARY KEY,
    Descricao VARCHAR2(255),
    Tipo VARCHAR2(100),
    FrequenciaIdeal NUMBER
);

CREATE TABLE RegistroHabito (
    Id NUMBER PRIMARY KEY,
    HabitoId NUMBER,
    Data DATE,
    Imagem VARCHAR2(255),
    Observacoes VARCHAR2(4000),
    FOREIGN KEY (HabitoId) REFERENCES Habito(HabitoId)
);

CREATE TABLE Unidade (
    UnidadeId NUMBER PRIMARY KEY,
    Nome VARCHAR2(255),
    Estado VARCHAR2(50),
    Cidade VARCHAR2(50),
    Endereco VARCHAR2(255)
);

CREATE TABLE Usuario (
    UsuarioId NUMBER PRIMARY KEY,
    Email VARCHAR2(255),
    Nome VARCHAR2(255),
    Senha VARCHAR2(255),
    DataCadastro DATE,
    PontosRecompensa NUMBER
);
```

---


