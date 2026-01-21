# ExchangePulse

## 🎯 Finalidade da Aplicação
ExchangePulse é um sistema para **coleta, armazenamento e análise de cotações de moedas**, cálculo de métricas financeiras e integração com APIs externas (Banco Central do Brasil para SELIC e IPCA).  
Ele permite:
- Cadastrar moedas.
- Coletar cotações automaticamente via API PTAX.
- Calcular métricas financeiras (médias móveis, volatilidade, Sharpe, drawdown, VaR).
- Integrar dados macroeconômicos (taxa de juros SELIC e inflação IPCA).
- Executar atualizações automáticas via HostedService.

---

## 📂 Classes e Métodos

### **Controllers**
- **CurrencyController**
  - `GetAll()` → Lista todas as moedas cadastradas.
  - `GetById(Guid id)` → Obtém moeda pelo ID.
  - `Create(CurrencyDTO dto)` → Cadastra nova moeda.
  - `Update(Guid id, CurrencyDTO dto)` → Atualiza moeda existente.
  - `Delete(Guid id)` → Remove moeda pelo ID.

- **ExchangeRateController**
  - `GetAll()` → Lista todas as cotações cadastradas.
  - `GetById(Guid id)` → Obtém cotação pelo ID.
  - `GetByCurrency(Guid currencyId, DateTime start, DateTime end)` → Obtém histórico de cotações de uma moeda em um período.
  - `Create(ExchangeRateDTO dto)` → Cadastra nova cotação.
  - `Update(Guid id, ExchangeRateDTO dto)` → Atualiza cotação existente.
  - `Delete(Guid id)` → Remove cotação pelo ID.

- **ExchangeMetricController**
  - `GetAll()` → Lista todas as métricas financeiras cadastradas.
  - `GetById(Guid id)` → Obtém métrica pelo ID.
  - `Create(ExchangeMetricDTO dto)` → Cadastra nova métrica.
  - `Update(Guid id, ExchangeMetricDTO dto)` → Atualiza métrica existente.
  - `Delete(Guid id)` → Remove métrica pelo ID.

---

### **Services**
- **CurrencyService**
  - CRUD completo para moedas.
- **ExchangeRateService**
  - CRUD completo para cotações + histórico por moeda/período.
- **ExchangeMetricService**
  - CRUD completo para métricas.
- **ExchangeRateUpdater**
  - Atualiza cotações USD/BRL e calcula métricas financeiras.

---

### **Repositories**
- **CurrencyRepository** → CRUD de moedas.  
- **ExchangeRateRepository** → CRUD de cotações + histórico.  
- **ExchangeMetricRepository** → CRUD de métricas.  

---

### **External Services**
- **BcbExchangeRateFetcher** → Busca cotações USD/BRL via API PTAX.  
- **BcbDataFetcher** → Busca SELIC e IPCA via API SGS.  

---

### **Background Services**
- **ExchangeRateBackgroundService** → Executa atualização automática de cotações e métricas em agendamento configurável.  

---

### **Persistence**
- **ExchangePulseDbContext** → Configuração EF Core para entidades.  
- **CurrencySeeder** → Popula moedas iniciais.  

---

### **Validators**
- **CurrencyDTOValidator** → Valida dados de entrada de moedas.  

---

## 📘 Entidades

### **Currency**
| Propriedade | Tipo        | Funcionalidade |
|-------------|-------------|----------------|
| Id          | Guid        | Identificador único da moeda. |
| Code        | string(3)   | Código ISO 4217 (ex.: BRL, USD). |
| Name        | string      | Nome da moeda. |
| Country     | string      | País ou região emissora. |

---

### **ExchangeRate**
| Propriedade | Tipo        | Funcionalidade |
|-------------|-------------|----------------|
| Id          | Guid        | Identificador único da cotação. |
| Date        | DateTime    | Data da cotação. |
| CurrencyId  | Guid        | FK para `Currency`. |
| Currency    | Currency    | Navegação para entidade `Currency`. |
| BuyPrice    | decimal     | Preço de compra. |
| SellPrice   | decimal     | Preço de venda. |
| Spread      | decimal     | Diferença entre venda e compra (não persistido). |
| Average     | decimal     | Média entre compra e venda (não persistido). |
| Volume      | long        | Volume negociado. |
| Source      | string      | Fonte da cotação (ex.: BCB). |
| MacroEvents | string      | Eventos macroeconômicos relacionados. |

---

### **ExchangeMetric**
| Propriedade        | Tipo     | Funcionalidade |
|--------------------|----------|----------------|
| Id                 | Guid     | Identificador único da métrica. |
| Date               | DateTime | Data da métrica. |
| CurrencyId         | Guid     | FK para `Currency`. |
| Currency           | Currency | Navegação para entidade `Currency`. |
| DailyVariation     | decimal  | Variação percentual diária. |
| LogReturn          | decimal  | Retorno logarítmico. |
| MovingAverage7d    | decimal  | Média móvel 7 dias. |
| MovingAverage30d   | decimal  | Média móvel 30 dias. |
| Volatility30d      | decimal  | Volatilidade 30 dias. |
| SharpeDaily        | decimal  | Índice de Sharpe diário. |
| SharpeAnnual       | decimal  | Índice de Sharpe anualizado. |
| Drawdown           | decimal  | Máxima perda relativa desde o pico. |
| Beta               | decimal  | Sensibilidade em relação a benchmark. |
| VaREmpirical95     | decimal  | Value-at-Risk empírico 95%. |
| VaRCornishFisher95 | decimal  | Value-at-Risk ajustado Cornish-Fisher. |
| InterestRate       | decimal  | Taxa SELIC. |
| Inflation          | decimal  | Inflação IPCA. |

---

## 📊 Relatórios e Finalidades

Com os dados de **cotações** e **métricas financeiras** armazenados pelo ExchangePulse, é possível gerar diversos relatórios úteis para análise econômica e tomada de decisão:

### 1. Relatório de Cotações Históricas
- **Finalidade:** acompanhar tendências cambiais e apoiar decisões de importação/exportação.
- **Exemplo:**
| Data       | Compra   | Venda   | Média |
|------------|----------|---------|-------|
| 2026-01-10 | 5.10     | 5.15    | 5.125 |
| 2026-01-11 | 5.12     | 5.18    | 5.150 |

### 2. Relatório de Médias Móveis
- **Finalidade:** identificar tendências de curto e médio prazo.
- **Exemplo:**
| Data       | Média 7d | Média 30d |
|------------|----------|-----------|
| 2026-01-12 | 5.11     | 5.09      |

### 3. Relatório de Volatilidade
- **Finalidade:** medir o risco associado à moeda.
- **Exemplo:**
| Período    | Volatilidade 30d |
|------------|------------------|
| Jan/2026   | 0.045            |

### 4. Relatório de Retornos e Índice de Sharpe
- **Finalidade:** avaliar se o retorno compensa o risco.
- **Exemplo:**
| Data       | Retorno Log | Sharpe Diário | Sharpe Anual |
|------------|-------------|---------------|--------------|
| 2026-01-12 | 0.0021      | 1.25          | 19.8         |

### 5. Relatório de Drawdown
- **Finalidade:** medir risco de queda acentuada.
- **Exemplo:**
| Período    | Máximo | Mínimo | Drawdown |
|------------|--------|--------|----------|
| Jan/2026   | 5.20   | 5.05   | -2.88%   |

### 6. Relatório de Value-at-Risk (VaR)
- **Finalidade:** estimar perda máxima esperada com 95% de confiança.
- **Exemplo:**
| Período    | VaR Empírico 95% | VaR Cornish-Fisher 95% |
|------------|------------------|------------------------|
| Jan/2026   | -0.035           | -0.032                 |

### 7. Relatório Macroeconômico
- **Finalidade:** contextualizar o câmbio dentro do cenário econômico nacional.
- **Exemplo:**
| Data       | SELIC (%) | IPCA (%) |
|------------|-----------|----------|
| 2026-01-12 | 13.75     | 0.45     |

---

## 🏛️ Decisão de Design: Uso de PTAX/SGS em vez da B3

O ExchangePulse utiliza como fonte principal de dados as APIs **PTAX** e **SGS** do **Banco Central do Brasil**.  
Essa escolha foi feita com base nos seguintes pontos:

### 🔎 Motivos da decisão
- **Dados oficiais e auditáveis**: PTAX e SGS são mantidos pelo Banco Central, garantindo confiabilidade e consistência.  
- **Gratuidade e acesso público**: não há custos de licenciamento ou barreiras de acesso.  
- **Cobertura macroeconômica**: além das cotações oficiais, o SGS fornece indicadores como **SELIC** e **IPCA**, fundamentais para análises financeiras.  
- **Relatórios consistentes**: ao usar dados oficiais, os relatórios gerados pelo sistema podem ser comparados diretamente com publicações oficiais e utilizados em contextos acadêmicos ou corporativos.  

### 📌 Limitações reconhecidas
- **Volume de negociação**: a API PTAX não fornece dados de volume ou número de negócios.  
- **Granularidade intraday**: os dados são disponibilizados em frequência diária, sem detalhamento minuto a minuto.  

### ✅ Estratégia adotada
- O campo **Volume** permanece como `0` quando a fonte não fornece essa informação.  
- Caso seja necessário incluir dados de negociação (volume, número de negócios, book de ofertas), será avaliada integração futura com a **API oficial da B3** (licenciada) ou com **APIs gratuitas de terceiros** (ex.: brapi.dev, HG Brasil).  
- Para o objetivo atual — **monitorar câmbio oficial e métricas macroeconômicas** — o uso de PTAX/SGS é a melhor estratégia para garantir **dados confiáveis e relatórios consistentes**.  

---

## ⚙️ Configuração
No `appsettings.json`:
```json
{
  "BackgroundJobs": {
    "ExchangeRateUpdateHour": 18,
    "ExchangeRateUpdateMinute": 30,
    "IsProductionSchedule": true
  },
  "BcbSgs": {
    "SelicSeriesCode": 11,
    "IpcaSeriesCode": 10844,
    "IpcaMaxFallbackMonths": 3
  }
}
