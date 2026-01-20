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
  - `GetAll()` → Lista todas as moedas.
  - `GetById(Guid id)` → Obtém moeda pelo ID.
  - `Create(CurrencyDTO dto)` → Cadastra nova moeda.
  - `Update(Guid id, CurrencyDTO dto)` → Atualiza moeda existente.
  - `Delete(Guid id)` → Remove moeda pelo ID.

- **ExchangeRateController** *(a implementar)*  
- **ExchangeMetricController** *(a implementar)*  

---

### **Services**
- **CurrencyService**
  - `GetAllAsync()` → Retorna todas as moedas.
  - `GetByIdAsync(Guid id)` → Retorna moeda pelo ID.
  - `CreateAsync(CurrencyDTO dto)` → Cria nova moeda.
  - `UpdateAsync(Guid id, CurrencyDTO dto)` → Atualiza moeda existente.
  - `DeleteAsync(Guid id)` → Remove moeda.

- **ExchangeRateService**
  - `GetAllAsync()` → Retorna todas as cotações.
  - `GetByIdAsync(Guid id)` → Retorna cotação pelo ID.
  - `CreateAsync(ExchangeRateDTO dto)` → Cria nova cotação.
  - `UpdateAsync(Guid id, ExchangeRateDTO dto)` → Atualiza cotação.
  - `DeleteAsync(Guid id)` → Remove cotação.
  - `GetByCurrencyAsync(Guid currencyId, DateTime start, DateTime end)` → Retorna histórico de cotações por moeda e período.

- **ExchangeMetricService**
  - `GetAllAsync()` → Retorna todas as métricas.
  - `GetByIdAsync(Guid id)` → Retorna métrica pelo ID.
  - `CreateAsync(ExchangeMetricDTO dto)` → Cria nova métrica.
  - `UpdateAsync(Guid id, ExchangeMetricDTO dto)` → Atualiza métrica.
  - `DeleteAsync(Guid id)` → Remove métrica.

- **ExchangeRateUpdater**
  - `UpdateUsdBrlPeriodAsync(Guid currencyId, DateTime start, DateTime end)` → Busca cotações USD/BRL, salva no banco e calcula métricas financeiras.
  - `Variance(IEnumerable<double> values)` → Função auxiliar para cálculo de variância.

---

### **Repositories**
- **CurrencyRepository**
  - CRUD completo para `Currency`.

- **ExchangeRateRepository**
  - CRUD completo para `ExchangeRate`.
  - `GetByCurrencyAsync(Guid currencyId, DateTime start, DateTime end)` → Histórico de cotações.

- **ExchangeMetricRepository**
  - CRUD completo para `ExchangeMetric`.

---

### **External Services**
- **BcbExchangeRateFetcher**
  - `GetUsdBrlRatesAsync(DateTime start, DateTime end)` → Busca cotações USD/BRL via API PTAX.

- **BcbDataFetcher**
  - `GetSelicAsync(DateTime date)` → Busca taxa SELIC diária via API SGS.
  - `GetIpcaAsync(DateTime date)` → Busca IPCA mensal via API SGS com fallback.

---

### **Background Services**
- **ExchangeRateBackgroundService**
  - Executa atualização automática de cotações e métricas em agendamento configurável.
  - Suporta modo produção (horário fixo) e modo teste (execução a cada minuto).

---

### **Persistence**
- **ExchangePulseDbContext**
  - Configuração EF Core para `Currency`, `ExchangeRate`, `ExchangeMetric`.
  - Define constraints, tipos de coluna e relacionamentos.

- **CurrencySeeder**
  - Popula moedas iniciais (USD, BRL, EUR, etc.).

---

### **Validators**
- **CurrencyDTOValidator**
  - Valida código ISO (3 letras maiúsculas).
  - Valida nome e país (não vazios, máximo 100 caracteres).

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
