# 🚀 Challenge Sprint 03 API

Este repositório contém a API desenvolvida para o desafio da Sprint 03, utilizando **ASP.NET Core Web API**, **Oracle** como banco de dados e diversos padrões de design e boas práticas de arquitetura de software.

---

## 👥 Integrantes do Grupo

|------|--------|
| Integrante 1 | - Rafael de Novaes – RM553934 – 2TDSPC
| Integrante 2 | - Fabiola Falcão – RM552715 – 2TDSPC
| Integrante 3 | - Carlos Henrique Furtado Nascimento – RM553597 – 2TDSPR


---

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
- **Injeção de Dependência**

---

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
git clone https://github.com/SeuUsuario/Challenge_Sprint03.git
cd Challenge_Sprint03
```

### 2️⃣ Configure o Banco *(appsettings.json)*

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=usuario;Password=senha;Data Source=..."
  }
}
```

### 3️⃣ Restaurar pacotes NuGet

```bash
dotnet restore
```

### 4️⃣ Aplicar Migrations

```bash
dotnet ef migrations add InitialMigration
dotnet ef database update
```

### 5️⃣ Executar a aplicação

```bash
dotnet run
```

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
```

### ▶️ Usuarios *(PATCH)*

```json
5
```

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


